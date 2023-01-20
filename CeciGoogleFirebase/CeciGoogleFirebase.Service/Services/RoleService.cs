using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.Role;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResultDataResponse<IEnumerable<RoleResultDTO>>> GetAsync()
        {
            var response = new ResultDataResponse<IEnumerable<RoleResultDTO>>();

            try
            {
                response.Data = _mapper.Map<IEnumerable<RoleResultDTO>>(await _uow.Role.GetAllAsync());
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Add new role
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<ResultResponse> AddAsync(RoleAddDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                await _uow.Role.AddAsync(_mapper.Map<Role>(obj));
                await _uow.CommitAsync();

                response.Message = "Role successfully added.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not add role.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> DeleteAsync(int id)
        {
            var response = new ResultResponse();

            try
            {
                var role = await _uow.Role.GetFirstOrDefaultAsync(c => c.Id == id);

                if (role == null)
                {
                    return new ResultResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Role not found"
                    };
                }

                _uow.Role.Delete(role);
                await _uow.CommitAsync();

                response.Message = "Role successfully delete.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not update role.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> UpdateAsync(RoleUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var role = await _uow.Role.GetFirstOrDefaultAsync(c => c.Id == obj.RoleId);

                if (role == null)
                {
                    return new ResultResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Role not found"
                    };
                }

                role = _mapper.Map(obj, role);

                _uow.Role.Update(role);
                await _uow.CommitAsync();

                response.Message = "Role successfully update.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not update role.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse<RoleResultDTO>> GetByIdAsync(int id)
        {
            var response = new ResultResponse<RoleResultDTO>();

            try
            {
                response.Data = _mapper.Map<RoleResultDTO>(await _uow.Role.GetFirstOrDefaultAsync(c => c.Id == id));
            }
            catch (Exception ex)
            {
                response.Message = "It was not possible to search the role.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
