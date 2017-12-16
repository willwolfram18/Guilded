using Guilded.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Guilded.DAL.Abstract
{
    public abstract class ReadWriteDataContextBase : ReadOnlyDataContextBase, IReadWriteDataContext
    {
        #region Constructors
        protected ReadWriteDataContextBase(ApplicationDbContext context) : base(context)
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
                Context.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                foreach (var entity in concurrencyException.Entries)
                {
                    entity.Reload();
                }
            }
            catch
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
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                foreach (var entity in concurrencyException.Entries)
                {
                    await entity.ReloadAsync();
                }
            }
            catch
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
