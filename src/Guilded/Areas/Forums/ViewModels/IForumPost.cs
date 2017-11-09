using System.Security.Claims;

namespace Guilded.Areas.Forums.ViewModels
{
    public interface IForumPost
    {
        string ShareLink { get; }

        bool IsLocked { get; }

        bool IsUserTheAuthor(ClaimsPrincipal user);
    }
}
