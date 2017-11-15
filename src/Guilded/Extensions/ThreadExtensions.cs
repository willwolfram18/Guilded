using Guilded.Data.Forums;

namespace Guilded.Extensions
{
    public static class ThreadExtensions
    {
        public static bool IsNotFound(this Thread thread)
        {
            return thread == null || thread.IsDeleted || !thread.Forum.IsActive;
        }
    }
}
