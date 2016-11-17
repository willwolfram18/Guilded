#define OS_X

namespace Selama.Common
{
    public static class Globals
    {
        public static bool OS_X
        {
            get
            {
#if OS_X
                return true;
#else
                return false;
#endif
            }
        }
    }
}