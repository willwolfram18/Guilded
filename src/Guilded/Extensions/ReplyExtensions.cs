using Guilded.Data.Forums;

namespace Guilded.Extensions
{
    public static class ReplyExtensions
    {
        public static bool IsNotFound(this Reply reply)
        {
            return reply == null || reply.IsDeleted ||
                   reply.Thread.IsDeleted || !reply.Thread.Forum.IsActive;
        }
    }
}
