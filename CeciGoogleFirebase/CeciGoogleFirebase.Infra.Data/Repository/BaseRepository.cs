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
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _appDbContext;

        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<TEntity> AddAsync(TEntity obj)
        {
            await _appDbContext.Set<TEntity>()
                .AddAsync(obj);
            return obj;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> obj)
        {
            await _appDbContext.Set<TEntity>()
                .AddRangeAsync(obj);
            return obj;
        }

        public void Update(TEntity obj)
        {
            _appDbContext.Entry(obj).State = EntityState.Modified;
            _appDbContext.Set<TEntity>()
                .Update(obj);
        }

        public void Delete(TEntity obj)
        {
            _appDbContext.Set<TEntity>()
                .Remove(obj);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _appDbContext.Set<TEntity>()
                .OrderByDescending(x => x.Id)
                .ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllNoTrackingAsync() =>
            await _appDbContext.Set<TEntity>()
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .ToListAsync();

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _appDbContext.Set<TEntity>()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(predicate);

        public async Task<TEntity> GetFirstOrDefaultNoTrackingAsync(Expression<Func<TEntity, bool>> predicate) =>
          await _appDbContext.Set<TEntity>()
                .AsNoTracking()
                .OrderByDescending(x=> x.Id)
                .FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _appDbContext.Set<TEntity>()
                .Where(predicate)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

        public async Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _appDbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

        public async Task<int> GetTotalAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _appDbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .CountAsync();

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
