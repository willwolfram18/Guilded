using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public const string JWT_CLAIM_TYPE = "Selama Ashalanore Member";
        public const string JWT_CLAIM_VALUE = "Is Member";
    }
}
