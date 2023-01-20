using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.Email;
using CeciGoogleFirebase.Domain.DTO.ValidationCode;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Infra.CrossCutting.Extensions;
using CeciGoogleFirebase.Infra.CrossCutting.Helper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Services
{
    public class ValidationCodeService : IValidationCodeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobClient _jobClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidationCodeService(IUnitOfWork uow,
            IEmailService emailService,
            IBackgroundJobClient jobClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _emailService = emailService;
            _jobClient = jobClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultResponse> SendAsync()
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var user = await _uow.User
                    .GetUserByIdAsync(Convert.ToInt32(userId));

                var code = PasswordExtension.GeneratePassword(0, 0, 6, 0);
                
                await _uow.ValidationCode.AddAsync(new ValidationCode {
                    Code = PasswordExtension.EncryptPassword(StringHelper.Base64Encode(code)),
                    UserId = user.Id,
                    Expires = System.DateTime.UtcNow.AddMinutes(10)
                });
                await _uow.CommitAsync();

                user.Validated = false;
                _uow.User.Update(user);
                await _uow.CommitAsync();

                response.Message = "Code sent successfully.";

                _jobClient.Enqueue(() => _emailService.SendEmailAsync(new EmailRequestDTO
                {
                    Body = "A new validation code was requested. Use the code <b>" + code + "</b> to complete validation.",
                    Subject = user.Name,
                    ToEmail = user.Email
                }));
            }
            catch (Exception ex)
            {
                response.Message = "Could not send code.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> ValidateCodeAsync(ValidationCodeValidateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var validationCode = await _uow.ValidationCode
                    .GetFirstOrDefaultNoTrackingAsync(x => x.UserId.Equals(Convert.ToInt32(userId)));

                if (validationCode == null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "Invalid or expired validation code.";
                    return response;
                }

                if (!PasswordExtension.DecryptPassword(validationCode.Code).Equals(obj.Code)
                    || validationCode.IsExpired)
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "Invalid or expired validation code.";
                    return response;
                }

                var user = await _uow.User
                    .GetUserByIdAsync(Convert.ToInt32(userId));

                user.Validated = true;
                _uow.User.Update(user);
                await _uow.CommitAsync();

                response.Message = "Code validated successfully.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not validate code.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
