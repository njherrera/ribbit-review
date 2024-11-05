namespace CSharpParser.Filters.Settings
{
    public abstract class FilterSettingsBuilder
    {
        public abstract FilterSettings Settings { get; }

        public FilterSettingsBuilder addUserID(string? ID)
        {
            Settings.userID = ID;
            return this;
        }
        public FilterSettingsBuilder addConvertingPlayer(string? player)
        {
            Settings.convertingPlayer = player;
            return this;
        }

        public FilterSettingsBuilder addConversionKilled(bool? killed)
        {
            Settings.conversionKilled = killed;
            return this;
        }

        public FilterSettingsBuilder addStartingPercent(int? percent)
        {
            Settings.startingPercent = percent;
            return this;
        }

        public FilterSettingsBuilder addEndingPercent(int? percent)
        {
            Settings.endingPercent = percent;
            return this;
        }

        public FilterSettingsBuilder addMovesUsed(int[]? movesUsed)
        {
            Settings.movesUsed = movesUsed;
            return this;
        }

        public FilterSettingsBuilder addStartingMove(int? startingMove)
        {
            Settings.startingMove = startingMove;
            return this;
        }

        public FilterSettingsBuilder addOpeningType(string? openingType)
        {
            Settings.openingType = openingType;
            return this;
        }

        public FilterSettings Build()
        {
            return this.Settings;
        }
    }
}
