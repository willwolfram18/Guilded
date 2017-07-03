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
    }
}
