using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.Email;
using CeciGoogleFirebase.Domain.DTO.Register;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Infra.CrossCutting.Extensions;
using DocumentFormat.OpenXml.Spreadsheet;
using Hangfire;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBackgroundJobClient _jobClient;
        private readonly IEmailService _emailService;

        public RegisterService(IUnitOfWork uow,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IBackgroundJobClient jobClient,
            IEmailService emailService)
        {
            _uow = uow;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jobClient = jobClient;
            _emailService = emailService;
        }

        public async Task<ResultResponse<UserResultDTO>> GetLoggedInUserAsync()
        {
            var response = new ResultResponse<UserResultDTO>();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                //validate if it is an external provider
                var provider = _httpContextAccessor.HttpContext.User.GetByType("firebase");

                if (!string.IsNullOrEmpty(provider))
                {
                    var user = _mapper.Map<UserResultDTO>(await _uow.User.GetUserByExternalIdAsync(userId));
                    if (user == null)
                    {
                        return new ResultResponse<UserResultDTO>
                        {
                            StatusCode = System.Net.HttpStatusCode.Unauthorized
                        };
                    }

                    response.Data = user;
                }
                else
                {
                    response.Data = _mapper.Map<UserResultDTO>(await _uow.User.GetUserByIdAsync(Convert.ToInt32(userId)));
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> SelfRegistrationAsync(UserSelfRegistrationDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var basicProfile = await _uow.Role.GetBasicProfile();

                obj.Password = PasswordExtension.EncryptPassword(obj.Password);

                var newUser = _mapper.Map<User>(obj);
                newUser.RoleId = basicProfile.Id;

                await _uow.User.AddAsync(newUser);
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

        public async Task<ResultResponse> RegisterWithLoginProviderAsync()
        {
            var response = new ResultResponse();

            try
            {
                var emailRegistered = await _uow.User
                    .GetFirstOrDefaultNoTrackingAsync(c => c.Email == _httpContextAccessor.HttpContext.User.GetLoggedInUserEmail());

                if (emailRegistered != null)
                {
                    return response;
                }

                var basicProfile = await _uow.Role.GetBasicProfile();

                var newUser = new User { 
                    Name = _httpContextAccessor.HttpContext.User.GetByType("name"),
                    Email = _httpContextAccessor.HttpContext.User.GetLoggedInUserEmail(),
                    ExternalId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId(),
                    IsExternalProvider = true,
                    RoleId = basicProfile.Id
                };

                await _uow.User.AddAsync(newUser);
                await _uow.CommitAsync();

                response.Message = "User successfully added.";

                _jobClient.Enqueue(() => _emailService.SendEmailAsync(new EmailRequestDTO
                {
                    Body = "User successfully added.",
                    Subject = newUser.Name,
                    ToEmail = newUser.Email
                }));
            }
            catch (Exception ex)
            {
                response.Message = "Could not add user.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> UpdateLoggedUserAsync(UserLoggedUpdateDTO obj)
        {
            var response = new ResultResponse();
            User user;

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                //validate if it is an external provider
                var provider = _httpContextAccessor.HttpContext.User.GetByType("firebase");

                if (!string.IsNullOrEmpty(provider))
                {
                    user = await _uow.User.GetUserByExternalIdAsync(userId);
                    
                    if (user == null)
                    {
                        return new ResultResponse<UserResultDTO>
                        {
                            StatusCode = System.Net.HttpStatusCode.Unauthorized
                        };
                    }

                    user.Name = obj.Name;
                }
                else
                {
                    var emailRegistered = await _uow.User
                        .GetFirstOrDefaultNoTrackingAsync(c => c.Email == obj.Email && c.Id != Convert.ToInt32(userId));

                    if (emailRegistered != null)
                    {
                        return new ResultResponse
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "E-mail already registered"
                        };
                    }

                    user = await _uow.User.GetFirstOrDefaultAsync(c => c.Id == Convert.ToInt32(userId));

                    user = _mapper.Map(obj, user);
                }

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

        public async Task<ResultResponse> RedefinePasswordAsync(UserRedefinePasswordDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var user = await _uow.User
                    .GetFirstOrDefaultAsync(c => c.Id.Equals(Convert.ToInt32(userId)));

                if (!PasswordExtension.DecryptPassword(user.Password).Equals(obj.CurrentPassword))
                {
                    response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    response.Message = "Password incorret.";
                    return response;
                };

                user.Password = PasswordExtension.EncryptPassword(obj.NewPassword);

                _uow.User.Update(user);
                await _uow.CommitAsync();

                response.Message = "Password user successfully updated.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not updated password user.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> AddLoggedUserAddressAsync(AddressLoggedUserAddDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var addressEntity = _mapper.Map<Address>(obj);

                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                //validate if it is an external provider
                var provider = _httpContextAccessor.HttpContext.User.GetByType("firebase");

                if (!string.IsNullOrEmpty(provider))
                {
                    var userProvider = await _uow.User.GetUserByExternalIdAsync(userId);

                    if (userProvider == null)
                    {
                        return new ResultResponse<UserResultDTO>
                        {
                            StatusCode = System.Net.HttpStatusCode.Unauthorized
                        };
                    }

                    addressEntity.UserId = userProvider.Id;
                }
                else
                {
                    addressEntity.UserId = Convert.ToInt32(userId);
                }

                await _uow.Address.AddAsync(addressEntity);
                await _uow.CommitAsync();

                response.Message = "Address successfully added.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not add address.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> UpdateLoggedUserAddressAsync(AddressLoggedUserUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                Address address;

                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                //validate if it is an external provider
                var provider = _httpContextAccessor.HttpContext.User.GetByType("firebase");

                if (!string.IsNullOrEmpty(provider))
                {
                    var userProvider = await _uow.User.GetUserByExternalIdAsync(userId);

                    if (userProvider == null)
                    {
                        return new ResultResponse<UserResultDTO>
                        {
                            StatusCode = System.Net.HttpStatusCode.Unauthorized
                        };
                    }

                    address = await _uow.Address.GetFirstOrDefaultAsync(x => x.User.ExternalId == userId
                        && x.Id == obj.AddressId);
                }
                else
                {
                    address = await _uow.Address.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(userId)
                        && x.Id == obj.AddressId);
                }

                if (address == null)
                {
                    return new ResultResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Address not found."
                    };
                }

                address = _mapper.Map(obj, address);

                _uow.Address.Update(address);
                await _uow.CommitAsync();

                response.Message = "Address successfully updated.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not updated address.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> InactivateLoggedUserAddressAsync(AddressDeleteDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                Address address;

                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                //validate if it is an external provider
                var provider = _httpContextAccessor.HttpContext.User.GetByType("firebase");

                if (!string.IsNullOrEmpty(provider))
                {
                    var userProvider = await _uow.User.GetUserByExternalIdAsync(userId);

                    if (userProvider == null)
                    {
                        return new ResultResponse<UserResultDTO>
                        {
                            StatusCode = System.Net.HttpStatusCode.Unauthorized
                        };
                    }

                    address = await _uow.Address.GetFirstOrDefaultAsync(x => x.User.ExternalId == userId
                        && x.Id == obj.AddressId);
                }
                else
                {
                    address = await _uow.Address.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(userId)
                        && x.Id == obj.AddressId);
                }

                if (address == null)
                {
                    return new ResultResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Address not found."
                    };
                }

                address.Active = false;

                _uow.Address.Update(address);
                await _uow.CommitAsync();

                response.Message = "Address successfully deactivated.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not deactivated address.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultDataResponse<IEnumerable<AddressResultDTO>>> GetLoggedUserAddressesAsync(AddressFilterDTO filter)
        {
            var response = new ResultDataResponse<IEnumerable<AddressResultDTO>>();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                response.Data = _mapper.Map<IEnumerable<AddressResultDTO>>(await _uow.Address.GetLoggedUserAddressesAsync(Convert.ToInt32(userId), filter));
                response.TotalItems = await _uow.Address.GetTotalLoggedUserAddressesAsync(Convert.ToInt32(userId), filter);
                response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / filter.PerPage);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse<AddressResultDTO>> GetLoggedUserAddressAsync(AddressIdentifierDTO obj)
        {
            var response = new ResultResponse<AddressResultDTO>();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var address = await _uow.Address.GetFirstOrDefaultNoTrackingAsync(x => x.UserId == Convert.ToInt32(userId)
                        && x.Id == obj.AddressId);

                if (address == null)
                {
                    return new ResultResponse<AddressResultDTO>()
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                    };
                }

                response.Data = _mapper.Map<AddressResultDTO>(address);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }
    }
}
