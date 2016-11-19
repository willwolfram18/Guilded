using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Data.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        void Reload(object entity);
        Task ReloadAsync(object entity);

        void SaveChanges();
        Task SaveChangesAsync();

        bool TrySaveChanges();
        Task<bool> TrySaveChangesAsync();
    }
}
