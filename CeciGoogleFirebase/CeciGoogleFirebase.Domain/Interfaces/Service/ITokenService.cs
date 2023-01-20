using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service
{
    public interface ITokenService
    {
        public string GenerateToken(UserResultDTO model);
    }
}
