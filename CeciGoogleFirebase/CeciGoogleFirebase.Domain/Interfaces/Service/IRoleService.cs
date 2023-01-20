using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.Role;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service
{
    public interface IRoleService
    {
        Task<ResultDataResponse<IEnumerable<RoleResultDTO>>> GetAsync();
        Task<ResultResponse> AddAsync(RoleAddDTO obj);
        Task<ResultResponse> DeleteAsync(int id);
        Task<ResultResponse> UpdateAsync(RoleUpdateDTO obj);
        Task<ResultResponse<RoleResultDTO>> GetByIdAsync(int id);
    }
}
