namespace CSharpParser.Filters.Settings.Builders
{
    public class EdgeguardSettingsBuilder : FilterSettingsBuilder
    {
        private EdgeguardSettings _settings = new EdgeguardSettings();
        public override FilterSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        public EdgeguardSettingsBuilder addSendOffMove(int? moveID)
        {
            _settings.sendOffMove = moveID;
            return this;
        }

        public EdgeguardSettingsBuilder addHitstunExitBelowLedge(bool? below)
        {
            _settings.hitstunExitBelowLedge = below;
            return this;
        }

        public EdgeguardSettingsBuilder addOffstageMove(int? moveID)
        {
            _settings.offstageMove = moveID;
            return this;
        }
    }
}
