using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.Filters.Settings
{
    public abstract class FilterSettingsBuilder
    {
        private FilterSettings _settings;
        public FilterSettingsBuilder addConvertingPlayer(string? player)
        {
            _settings.convertingPlayer = player;
            return this;
        }

        public FilterSettingsBuilder addConversionKilled(bool? killed)
        {
            _settings.conversionKilled = killed;
            return this;
        }

        public FilterSettingsBuilder addStartingPercent(int? percent)
        {
            _settings.startingPercent = percent;
            return this;
        }

        public FilterSettingsBuilder addEndingPercent(int? percent)
        {
            _settings.endingPercent = percent;
            return this;
        }

        public FilterSettingsBuilder addMovesUsed(int[]? movesUsed)
        {
            _settings.movesUsed = movesUsed;
            return this;
        }

        public FilterSettingsBuilder addStartingMove(int? startingMove)
        {
            _settings.startingMove = startingMove;
            return this;
        }

        public FilterSettingsBuilder addOpeningType(string? openingType)
        {
            _settings.openingType = openingType;
            return this;
        }

        public abstract FilterSettings build();
    }
}
