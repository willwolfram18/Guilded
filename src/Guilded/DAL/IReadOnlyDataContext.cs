using System;
using System.Threading.Tasks;

namespace Guilded.DAL
{
    public interface IReadOnlyDataContext : IDisposable
    {
        void Reload(object entity);
        Task ReloadAsync(object entity);
    }
}
