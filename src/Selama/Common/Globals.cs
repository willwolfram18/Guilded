using System.Runtime.InteropServices;

namespace Selama.Common
{
    public static class Globals
    {
        public static bool OSX
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            }
        }
    }
}
