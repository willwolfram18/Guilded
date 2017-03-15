using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SelamaApi.Data.DAL.Abstract
{
    public abstract class ReadWriteDataContextBase : ReadOnlyDataContextBase, IReadWriteDataContext
    {
        #region Constructors
        public ReadWriteDataContextBase(ApplicationDbContext context) : base(context)
        {
        }
        #endregion

        #region Methods
        #region Public methods
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
                // TODO: What happens in case of other exception types?
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
                // TODO: What happens in case of other exception types?
            }

            // If we've reached here an error occurred
            return false;
        }
        #endregion
        #endregion
    }
}
