using System;
using System.Threading.Tasks;

namespace Guilded.Data.DAL.Abstract
{
    public abstract class ReadOnlyDataContextBase : IReadOnlyDataContext
    {
        #region Properties
        #region Protected properties
        protected readonly ApplicationDbContext Context;
        #endregion

        #region Pirvate properties
        private bool _isDisposed = false;
        #endregion
        #endregion

        #region Constructors
        protected ReadOnlyDataContextBase(ApplicationDbContext context)
        {
            Context = context;
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
            Context.Entry(entity).Reload();
        }

        public async Task ReloadAsync(object entity)
        {
            await Context.Entry(entity).ReloadAsync();
        }
        #endregion

        #region Private methods
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Context?.Dispose();
                }
            }
            _isDisposed = true;
        }
        #endregion
        #endregion
    }
}
