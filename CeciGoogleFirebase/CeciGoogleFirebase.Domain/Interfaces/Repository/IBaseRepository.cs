using CeciGoogleFirebase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity obj);

        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> obj);

        void Update(TEntity obj);

        void Delete(TEntity obj);

        Task <IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllNoTrackingAsync();

        Task <IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);

        Task <TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetFirstOrDefaultNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> GetTotalAsync(Expression<Func<TEntity, bool>> predicate);

        void Dispose();
    }
}
