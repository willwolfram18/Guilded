using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Selama.Data.DAL
{
    public interface IEntityRepo<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> whereClause);
        IQueryable<TEntity> Get(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> whereClause,Func<IQueryable<TEntity>, 
            IOrderedQueryable<TEntity>> orderBy
        );

        TEntity FindById(object id);
        Task<TEntity> FindByIdAsync(object id);

        void Add(TEntity entityToAdd);

        void Update(TEntity entityToUpdate);

        void Remove(TEntity entityToRemove);
        void RemoveById(object id);
        Task RemoveByIdAsync(object id);
    }
}