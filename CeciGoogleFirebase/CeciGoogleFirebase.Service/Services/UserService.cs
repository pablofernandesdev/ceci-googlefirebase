using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CeciGoogleFirebase.Domain.DTO.Commons;
using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Infra.CrossCutting.Extensions;
using CeciGoogleFirebase.Domain.DTO.Email;
using Hangfire;
using Microsoft.AspNetCore.Http;

namespace CeciGoogleFirebase.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobClient _jobClient;

        public UserService(IUnitOfWork uow,
            IMapper mapper,
            IEmailService emailService,
            IBackgroundJobClient jobClient)
        {
            _uow = uow;
            _mapper = mapper;
            _emailService = emailService;
            _jobClient = jobClient;
        }

        public async Task<ResultDataResponse<IEnumerable<UserResultDTO>>> GetAsync(UserFilterDTO filter)
        {
            var response = new ResultDataResponse<IEnumerable<UserResultDTO>>();

            try
            {
                response.Data = _mapper.Map<IEnumerable<UserResultDTO>>(await _uow.User.GetByFilterAsync(filter));
                response.TotalItems = await _uow.User.GetTotalByFilterAsync(filter);
                response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / filter.PerPage);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> AddAsync(UserAddDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                obj.Password = PasswordExtension.EncryptPassword(obj.Password);
                await _uow.User.AddAsync(_mapper.Map<User>(obj));
                await _uow.CommitAsync();

                response.Message = "User successfully added.";

                _jobClient.Enqueue(() => _emailService.SendEmailAsync(new EmailRequestDTO
                {
                    Body = "User successfully added.",
                    Subject = obj.Name,
                    ToEmail = obj.Email
                }));
            }
            catch (Exception ex)
            {
                response.Message = "Could not add user.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> DeleteAsync(UserDeleteDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var user = await _uow.User.GetFirstOrDefaultAsync(c => c.Id == obj.UserId);

                _uow.User.Delete(user);
                await _uow.CommitAsync();

                response.Message = "User successfully deleted.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not deleted user.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> UpdateAsync(UserUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var emailRegistered = await _uow.User
                    .GetFirstOrDefaultAsync(c => c.Email == obj.Email && c.Id != obj.UserId);

                if (emailRegistered != null)
                {
                    return new ResultResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "E-mail already registered"
                    };
                }

                var user = await _uow.User.GetFirstOrDefaultAsync(c => c.Id == obj.UserId);

                user = _mapper.Map(obj, user);

                _uow.User.Update(user);
                await _uow.CommitAsync();

                response.Message = "User successfully updated.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not updated user.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> UpdateRoleAsync(UserUpdateRoleDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var user = await _uow.User.GetFirstOrDefaultAsync(c => c.Id == obj.UserId);

                user = _mapper.Map(obj, user);

                _uow.User.Update(user);
                await _uow.CommitAsync();

                response.Message = "User role updated successfully.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not updated user role.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse<UserResultDTO>> GetByIdAsync(int id)
        {
            var response = new ResultResponse<UserResultDTO>();

            try
            {
                response.Data = _mapper.Map<UserResultDTO>(await _uow.User.GetUserByIdAsync(id));
            }
            catch (Exception ex)
            {
                response.Message = "It was not possible to search the user.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
