using Guilded.DAL.Abstract;
using Guilded.Data;
using Guilded.Data.Forums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Areas.Forums.DAL
{
    public class ForumsDataContext : ReadWriteDataContextBase, IForumsDataContext
    {
        private IQueryable<Forum> Forums => Context.Forums.Include(f => f.ForumSection)
            .Include(f => f.Threads)
            .ThenInclude(t => t.Author);

        private IQueryable<Thread> Threads => Context.Threads.Include(t => t.Author)
            .Include(t => t.Forum)
            .Include(t => t.Replies)
            .ThenInclude(r => r.Author);


        public ForumsDataContext(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<ForumSection> GetActiveForumSections()
        {
            return Context.ForumSections.Include(f => f.Forums)
                .Where(f => f.IsActive);
        }

        public Task<Forum> GetForumByIdAsync(int id)
        {
            return Forums.FirstOrDefaultAsync(f => f.Id == id);
        }

        public Task<Forum> GetForumBySlugAsync(string slug)
        {
            return Forums.FirstOrDefaultAsync(f => f.Slug == slug);
        }

        public Task<Thread> GetThreadByIdAsync(int id)
        {
            return Threads.FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<Thread> GetThreadBySlugAsync(string slug)
        {
            return Threads.FirstOrDefaultAsync(t => t.Slug == slug);
        }
    }
}
