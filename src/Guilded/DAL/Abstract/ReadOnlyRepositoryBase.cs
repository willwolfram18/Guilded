using Guilded.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Guilded.DAL.Abstract
{
    public abstract class ReadOnlyRepositoryBase<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class
    {
        #region Properties
        #region Protected properties
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> Source;
        #endregion
        #endregion

        #region Constructor
        protected ReadOnlyRepositoryBase(ApplicationDbContext context)
        {
            Context = context;
            Source = Context.Set<TEntity>();
        }
        #endregion

        #region Methods
        #region Public methods
        public void Dispose()
        {
            Context.Dispose();
        }

        public virtual TEntity GetById(object id)
        {
            return Source.Find(id);
        }

        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return Source.FindAsync(id);
        }

        public virtual IQueryable<TEntity> Get()
        {
            return Get(null, null);
        }

        public virtual IQueryable<TEntity> Get(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            return Get(null, orderBy);
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> whereClause)
        {
            return Get(whereClause, null);
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> whereClause, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            var result = Source.AsQueryable();
            if (whereClause != null)
            {
                result = result.Where(whereClause);
            }
            if (orderBy != null)
            {
                result = orderBy(result).AsQueryable();
            }
            return result;
        }
        #endregion
        #endregion
    }
}
