namespace CSharpParser.SlpJSObjects
{
    public class GameSettings
    {
        public string slpVersion { get; set; }
        public int? timerType { get; set; }
        public int? inGameMode { get; set; }
        public bool? friendlyFireEnabled { get; set; }
        public bool? isTeams { get; set; }  // 0 if teams, non-zero if not
        public int? itemSpawnBehavior { get; set; }  // how frequently items spawn, -1 is off, 0 very low, 1 low, and so on
        public int? stageId { get; set; }  // in-game stage ID
        public int? startingTimerSeconds { get; set; }  // number of seconds timer starts at
        public double? enabledItems { get; set; }
        public List<Player> players { get; set; }  // info of players playing in game
        public int? scene { get; set; }  // should always be 2
        public int? gameMode { get; set; }  // 2 = versus mode, 8 = online
        public int? language { get; set; }  // 0 = japanese, 1 = english
        public Dictionary<string, object>? gameInfoBlock { get; set; } // having this be a <string, object> dictionary is a bit stinky, these values will also not be accessed
        public int? randomSeed { get; set; }  // random seed before the game start
        public bool? isPAL { get; set; }
        public bool? isFrozenPS { get; set; }
        public MatchInfo matchInfo { get; set; }

    }
}
