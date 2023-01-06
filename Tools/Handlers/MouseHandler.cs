using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Handlers
{
    public static class MouseHandler
    {

        public const int MouseeventRightDown = 0x08;
        public const int MouseeventRightUp = 0x10;

        //Win32 API call
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
        
    }
}
