using System.Threading.Tasks;

namespace Guilded.DAL
{
    public interface IReadWriteDataContext : IReadOnlyDataContext
    {
        void SaveChanges();
        Task SaveChangesAsync();

        bool TrySaveChanges();
        Task<bool> TrySaveChangesAsync();
    }
}
