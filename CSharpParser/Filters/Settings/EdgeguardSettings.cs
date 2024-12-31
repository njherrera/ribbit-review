using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters.Settings
{
    public class EdgeguardSettings : FilterSettings
    {
        public int? sendOffMove { get; set; }
        public bool? hitstunExitBelowLedge { get; set; }
    }
}

