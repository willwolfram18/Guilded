using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Data.DAL
{
    public interface IReadWriteDataContext : IReadOnlyDataContext
    {
        void SaveChanges();
        Task SaveChangesAsync();

        bool TrySaveChanges();
        Task<bool> TrySaveChangesAsync();
    }
}
