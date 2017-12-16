using System.Threading.Tasks;

namespace Guilded.DAL
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
