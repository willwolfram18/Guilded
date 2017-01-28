using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama_SPA.Data.DAL
{
    public interface IReadOnlyDataContext : IDisposable
    {
        void Reload(object entity);
        Task ReloadAsync(object entity);
    }
}
