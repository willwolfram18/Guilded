using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Guilded.Data.DAL.Abstract
{
    public abstract class ReadOnlyDataContextBase : IReadOnlyDataContext
    {
        #region Properties
        #region Protected properties
        protected readonly ApplicationDbContext _context;
        #endregion

        #region Pirvate properties
        private bool _isDisposed = false;
        #endregion
        #endregion

        #region Constructors
        public ReadOnlyDataContextBase(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        #region Public methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Reload(object entity)
        {
            _context.Entry(entity).Reload();
        }

        public async Task ReloadAsync(object entity)
        {
            await _context.Entry(entity).ReloadAsync();
        }
        #endregion

        #region Private methods
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing && _context != null)
                {
                    _context.Dispose();
                }
            }
            _isDisposed = true;
        }
        #endregion
        #endregion
    }
}
