using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Selama_SPA.Data.DAL.Abstract
{
    public abstract class ReadOnlyRepositoryBase<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class
    {
        #region Properties
        #region Protected properties
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _source;
        #endregion
        #endregion

        #region Constructor
        public ReadOnlyRepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _source = _context.Set<TEntity>();
        }
        #endregion

        #region Methods
        #region Public methods
        public void Dispose()
        {
            _context.Dispose();
        }

        public TEntity GetById(object id)
        {
            return _source.Find(id);
        }

        public Task<TEntity> GetByIdAsync(object id)
        {
            return _source.FindAsync(id);
        }

        public IQueryable<TEntity> Get()
        {
            return Get(null, null);
        }

        public IQueryable<TEntity> Get(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            return Get(null, orderBy);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> whereClause)
        {
            return Get(whereClause, null);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> whereClause, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            var result = _source.AsQueryable();
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
