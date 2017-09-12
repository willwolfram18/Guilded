using System.Security.Claims;

namespace Guilded.Areas.Forums.ViewModels
{
    public interface IForumPost
    {
        bool IsUserTheAuthor(ClaimsPrincipal user);
    }
}
