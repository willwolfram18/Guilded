using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Selama.Data.DAL.Abstract
{
    public abstract class EntityRepoBase<TEntity> : IEntityRepo<TEntity>
        where TEntity : class
    {
        #region Properties
        #region Private properties
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _source;
        #endregion
        #endregion

        #region Constructor
        public EntityRepoBase(ApplicationDbContext context)
        {
            _context = context;
            _source = _context.Set<TEntity>();
        }
        #endregion

        #region Methods
        #region Public methods
        public void Add(TEntity entityToAdd)
        {
            _source.Add(entityToAdd);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public TEntity FindById(object id)
        {
            return _source.Find(id);
        }

        public Task<TEntity> FindByIdAsync(object id)
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

        public void Remove(TEntity entityToRemove)
        {
            if (entityToRemove != null)
            {
                if (_context.Entry(entityToRemove).State == EntityState.Detached)
                {
                    _context.Attach(entityToRemove);
                }
                _source.Remove(entityToRemove);
            }
        }

        public void RemoveById(object id)
        {
            TEntity entityToRemove = FindById(id);
            Remove(entityToRemove);
        }

        public async Task RemoveByIdAsync(object id)
        {
            TEntity entityToRemove = await FindByIdAsync(id);
            Remove(entityToRemove);
        }

        public void Update(TEntity entityToUpdate)
        {
            _source.Update(entityToUpdate);
        }
        #endregion
        #endregion
    }
}
