using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Guilded.Data.DAL.Abstract
{
    public abstract class ReadWriteRepositoryBase<TEntity> : ReadOnlyRepositoryBase<TEntity>, IReadWriteRepository<TEntity>
        where TEntity : class
    {
        #region Constructors
        protected ReadWriteRepositoryBase(ApplicationDbContext context) : base(context)
        {
        }
        #endregion

        #region Methods
        #region Public methods
        public void Add(TEntity entityToAdd)
        {
            Source.Add(entityToAdd);
        }

        public void Remove(TEntity entityToRemove)
        {
            if (entityToRemove == null)
            {
                throw new ArgumentNullException(nameof(entityToRemove));
            }

            if (Context.Entry(entityToRemove).State == EntityState.Detached)
            {
                Context.Attach(entityToRemove);
            }
            Source.Remove(entityToRemove);
        }

        public void RemoveById(object id)
        {
            var entityToRemove = GetById(id);
            Remove(entityToRemove);
        }

        public async Task RemoveByIdAsync(object id)
        {
            var entityToRemove = await GetByIdAsync(id);
            Remove(entityToRemove);
        }

        public void Update(TEntity entityToUpdate)
        {
            Source.Update(entityToUpdate);
        }
        #endregion
        #endregion
    }
}
