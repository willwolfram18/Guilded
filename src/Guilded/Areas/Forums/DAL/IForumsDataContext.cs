using Guilded.Data.Forums;
using System.Linq;
using System.Threading.Tasks;

namespace Guilded.Areas.Forums.DAL
{
    public interface IForumsDataContext
    {
        IQueryable<ForumSection> GetActiveForumSections();

        Task<Forum> GetForumByIdAsync(int id);
        Task<Forum> GetForumBySlugAsync(string slug);

        Task<Thread> GetThreadByIdAsync(int id);
        Task<Thread> GetThreadBySlugAsync(string slug);
        Task<Thread> CreateThreadAsync(Thread thread);
        Task<Thread> UpdateThreadContentByIdAsync(int id, string content);
        Task LockThreadAsync(Thread thread);
        Task UnlockThreadAsync(Thread thread);
        Task PinThreadAsync(Thread thread);
        Task UnpinThreadAsync(Thread thread);
        Task DeleteThreadAsync(Thread thread);

        Task<Reply> CreateReplyAsync(Reply reply);
        Task<Reply> GetReplyByIdAsync(int id);
        Task<Reply> UpdateReplyContentByIdAsync(int id, string content);
        Task DeleteReplyAsync(Reply reply);
    }
}
