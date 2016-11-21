using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Selama.Data.DAL.Abstract
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        #region Properties
        #region Private properties
        private readonly ApplicationDbContext _context;
        private bool _isDisposed = false;
        #endregion
        #endregion

        #region Constructors
        public UnitOfWorkBase(ApplicationDbContext context)
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

        public void SaveChanges()
        {
            TrySaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await TrySaveChangesAsync();
        }

        public bool TrySaveChanges()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                foreach (var entity in concurrencyException.Entries)
                {
                    entity.Reload();
                }
            }
            catch (Exception e)
            {
            }

            // If we've reached here an error occurred
            return false;
        }

        public async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                foreach (var entity in concurrencyException.Entries)
                {
                    await entity.ReloadAsync();
                }
            }
            catch (Exception e)
            {
            }

            // If we've reached here an error occurred
            return false;
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
