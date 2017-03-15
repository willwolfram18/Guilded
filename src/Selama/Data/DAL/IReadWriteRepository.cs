using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selama.Data.DAL
{
    public interface IReadWriteRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entityToAdd);

        void Update(TEntity entityToUpdate);

        void Remove(TEntity entityToRemove);
        void RemoveById(object id);
        Task RemoveByIdAsync(object id);
    }
}
