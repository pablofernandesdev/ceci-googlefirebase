using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext appDbcontext) : base(appDbcontext)
        {
        }

        public async Task<IEnumerable<Address>> GetLoggedUserAddressesAsync(int userId, AddressFilterDTO filter)
        {
            Expression<Func<Address, bool>> query = c =>
                    (c.UserId == userId) &&
                    (!string.IsNullOrEmpty(filter.District) ? c.District.Contains(filter.District) : true) &&
                    (!string.IsNullOrEmpty(filter.Locality) ? c.Locality.Equals(filter.Locality) : true) &&
                    (!string.IsNullOrEmpty(filter.Uf) ? c.Uf.Equals(filter.Uf) : true) &&
                    (!string.IsNullOrEmpty(filter.Search)
                        ? (c.Complement.Contains(filter.Search) || c.Street.Contains(filter.Search))
                        : true);

            return await _appDbContext.Set<Address>()
                  .AsNoTracking()
                  .Where(query)
                  .Skip((filter.Page - 1) * filter.PerPage)
                  .Take(filter.PerPage)
                  .OrderByDescending(c => c.Id)
                  .ToListAsync();
        }

        public async Task<int> GetTotalLoggedUserAddressesAsync(int userId, AddressFilterDTO filter)
        {
            Expression<Func<Address, bool>> query = c =>
                    (c.UserId == userId) &&
                    (!string.IsNullOrEmpty(filter.District) ? c.District.Contains(filter.District) : true) &&
                    (!string.IsNullOrEmpty(filter.Locality) ? c.Locality.Equals(filter.Locality) : true) &&
                    (!string.IsNullOrEmpty(filter.Uf) ? c.Uf.Equals(filter.Uf) : true) &&
                    (!string.IsNullOrEmpty(filter.Search)
                        ? (c.Complement.Contains(filter.Search) || c.Street.Contains(filter.Search))
                        : true);

            return await _appDbContext.Set<Address>()
                  .AsNoTracking()
                  .Where(query)
                  .CountAsync();
        }

        public async Task<IEnumerable<Address>> GetByFilterAsync(AddressFilterDTO filter)
        {
            Expression<Func<Address, bool>> query = c =>
                    (!string.IsNullOrEmpty(filter.District) ? c.District.Contains(filter.District) : true) &&
                    (!string.IsNullOrEmpty(filter.Locality) ? c.Locality.Equals(filter.Locality) : true) &&
                    (!string.IsNullOrEmpty(filter.Uf) ? c.Uf.Equals(filter.Uf) : true) &&
                    (!string.IsNullOrEmpty(filter.Search)
                        ? (c.Complement.Contains(filter.Search) || c.Street.Contains(filter.Search))
                        : true);

            return await _appDbContext.Set<Address>()
                  .AsNoTracking()
                  .Where(query)
                  .Skip((filter.Page - 1) * filter.PerPage)
                  .Take(filter.PerPage)
                  .OrderByDescending(c => c.Id)
                  .ToListAsync();
        }

        public async Task<int> GetTotalByFilterAsync(AddressFilterDTO filter)
        {
            Expression<Func<Address, bool>> query = c =>
                    (!string.IsNullOrEmpty(filter.District) ? c.District.Contains(filter.District) : true) &&
                    (!string.IsNullOrEmpty(filter.Locality) ? c.Locality.Equals(filter.Locality) : true) &&
                    (!string.IsNullOrEmpty(filter.Uf) ? c.Uf.Equals(filter.Uf) : true) &&
                    (!string.IsNullOrEmpty(filter.Search)
                        ? (c.Complement.Contains(filter.Search) || c.Street.Contains(filter.Search))
                        : true);

            return await _appDbContext.Set<Address>()
                  .AsNoTracking()
                  .Where(query)
                  .CountAsync();
        }
    }
}
