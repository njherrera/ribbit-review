using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Settings
{
    public class UserPaths
    {
        public string PlaybackExePath;

        public string MeleeIsoPath;

        public UserPaths(string ExePath, string IsoPath) 
        {
            PlaybackExePath = ExePath;
            MeleeIsoPath = IsoPath;
        }

        private UserPaths() { }
    }
}
