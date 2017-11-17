using Guilded.DAL.Abstract;
using Guilded.Data;
using Guilded.Data.Forums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private IQueryable<Reply> Replies => Context.Replies.Include(r => r.Author)
            .Include(r => r.Thread)
            .ThenInclude(t => t.Forum);

        public ForumsDataContext(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<ForumSection> GetActiveForumSections() => Context.ForumSections.Include(f => f.Forums)
                .Where(f => f.IsActive);

        public Task<Forum> GetForumByIdAsync(int id) => Forums.FirstOrDefaultAsync(f => f.Id == id);

        public Task<Forum> GetForumBySlugAsync(string slug) => Forums.FirstOrDefaultAsync(f => f.Slug == slug);

        public Task<Thread> GetThreadByIdAsync(int id) => Threads.FirstOrDefaultAsync(t => t.Id == id);

        public Task<Thread> GetThreadBySlugAsync(string slug) => Threads.FirstOrDefaultAsync(t => t.Slug == slug);

        public async Task<Thread> CreateThreadAsync(Thread thread)
        {
            if (thread == null)
            {
                throw new ArgumentNullException(nameof(thread));
            }

            thread.Title = thread.Title.Trim();
            thread.Content = thread.Content.Trim();
            thread.Slug = GenerateSlug(thread.Title);
            thread.CreatedAt = DateTime.Now;

            await Context.Threads.AddAsync(thread);
            await SaveChangesAsync();

            return await GetThreadBySlugAsync(thread.Slug);
        }

        public Task LockThreadAsync(Thread thread)
        {
            thread.IsLocked = true;

            Context.Update(thread);

            return Context.SaveChangesAsync();
        }

        public Task UnlockThreadAsync(Thread thread)
        {
            throw new NotImplementedException();
        }

        public Task PinThread(Thread thread)
        {
            throw new NotImplementedException();
        }

        public Task UnpinThread(Thread thread)
        {
            throw new NotImplementedException();
        }

        public Task DeleteThreadAsync(Thread thread)
        {
            thread.IsDeleted = true;

            Context.Update(thread);

            return Context.SaveChangesAsync();
        }

        public Task<Reply> GetReplyByIdAsync(int replyId) => Replies.FirstOrDefaultAsync(r => r.Id == replyId);
        
        public async Task<Reply> CreateReplyAsync(Reply reply)
        {
            var dbReply = await Context.Replies.AddAsync(reply);
            await SaveChangesAsync();

            return await GetReplyByIdAsync(dbReply.Entity.Id);
        }

        public Task DeleteReplyAsync(Reply reply)
        {
            reply.IsDeleted = true;

            Context.Update(reply);

            return Context.SaveChangesAsync();
        }

        public static string GenerateSlug(string title)
        {
            var slug = title.ToLower().Trim();

            slug = RemoveDiacritics(slug);

            slug = Regex.Replace(slug, @"\W", " ");
            
            // Convert multiple spaces to one.
            slug = Regex.Replace(slug, @"\s+", " ").Trim();

            // Shorten title, if necessary.
            if (slug.Length > Thread.SlugMaxLength)
            {
                slug = slug.Substring(0, Thread.SlugMaxLength).Trim();
            }

            slug = Regex.Replace(slug, @"\s", "-");
            
            // TODO: Catch similar slugs

            return slug;
        }

        private static string RemoveDiacritics(string text)
        {
            var s = new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return s.Normalize(NormalizationForm.FormC);
        }
    }
}
