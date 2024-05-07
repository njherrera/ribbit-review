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

        public override FilterSettings Build()
        {
            return this._settings;
        }
    }
}
