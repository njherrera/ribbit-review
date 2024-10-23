using CSharpParser.Filters.Settings;
using CSharpParser.Filters;

namespace GUI.ViewModels
{
    public partial class EdgeguardViewModel : FilterViewModel
    {
        public override FilterSettingsBuilder Builder
        {
            get
            {
                return this._builder;
            }
        }

        private EdgeguardSettingsBuilder _builder = new EdgeguardSettingsBuilder();

        public override Filter Filter
        {
            get
            {
                return this._filter;
            }
        }

        private Edgeguards _filter = new Edgeguards();

        /*
        public override void applyFilter(List<GameConversions> allGameConversions, PlaybackQueue playbackQueue)
        {
            
            EdgeguardSettings edgeguardSettings = _builder.Build();
            foreach (GameConversions conversions in allGameConversions)
            {
                if (checkGameSettings(conversions) == true) 
                {
                    this.Filter.addToQueue(conversions, playbackQueue, edgeguardSettings);
                }
            }
        }
        */
    }
}
