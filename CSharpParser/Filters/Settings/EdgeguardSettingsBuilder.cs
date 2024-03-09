using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.Filters.Settings
{
    public class EdgeguardSettingsBuilder : FilterSettingsBuilder
    {
        private EdgeguardSettings _settings = new EdgeguardSettings();

        public override FilterSettings build()
        {
            return _settings;
        }
    }
}
