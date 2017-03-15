using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selama.Data.DAL.Abstract
{
    public abstract class ReadWriteRepositoryBase<TEntity> : ReadOnlyRepositoryBase<TEntity>, IReadWriteRepository<TEntity>
        where TEntity : class
    {
        #region Constructors
        public ReadWriteRepositoryBase(ApplicationDbContext context) : base(context)
        {
        }
        #endregion

        #region Methods
        #region Public methods
        public void Add(TEntity entityToAdd)
        {
            _source.Add(entityToAdd);
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
            TEntity entityToRemove = GetById(id);
            Remove(entityToRemove);
        }

        public async Task RemoveByIdAsync(object id)
        {
            TEntity entityToRemove = await GetByIdAsync(id);
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
