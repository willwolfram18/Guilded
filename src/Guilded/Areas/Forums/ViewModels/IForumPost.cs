using System.Security.Claims;

namespace Guilded.Areas.Forums.ViewModels
{
    public interface IForumPost
    {
        bool IsLocked { get; }

        bool IsUserTheAuthor(ClaimsPrincipal user);
    }
}
