using System.Runtime.InteropServices;

namespace Guilded.Common
{
    public static class Globals
    {
        public static bool OSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
}
