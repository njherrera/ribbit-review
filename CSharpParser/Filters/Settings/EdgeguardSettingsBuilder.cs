namespace CSharpParser.Filters.Settings
{
    public class EdgeguardSettingsBuilder : FilterSettingsBuilder
    {
        private EdgeguardSettings _settings = new EdgeguardSettings();
        public override FilterSettings Settings
        {
            get
            {
                return this._settings;
            }
        }

        public EdgeguardSettings Build()
        {
            return this._settings;
        }
    }
}
