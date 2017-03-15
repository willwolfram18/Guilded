using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SelamaApi.Data.DAL
{
    public interface IReadOnlyRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> whereClause);
        IQueryable<TEntity> Get(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> whereClause,Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy
        );

        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);
    }
}