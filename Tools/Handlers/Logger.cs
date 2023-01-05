using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Handlers
{
    public static class Logger
    {
        public static string CurrentlyLoggedInUuid { get; set; }
        public static string CurrentlyLoggedInName { get; set; }
        public static string CurrentlyLoggedInEmail { get; set; }

        public static string CurrentlyLoggedInProfileImage { get; set; }
    }
}
