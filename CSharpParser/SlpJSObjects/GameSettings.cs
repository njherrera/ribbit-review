using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSharpParser.SlpJSObjects
{
    public class GameSettings
    {
        [JsonProperty("slpVersion")]
        public string SlpVersion { get; set; }
        [JsonProperty("timerType")]
        public int? TimerType { get; set; }
        [JsonProperty("inGameMode")]
        public int? InGameModen { get; set; }
        [JsonProperty("friendlyFireEnabled")]
        public bool? FriendlyFireEnabled { get; set; }
        [JsonProperty("isTeams")]
        public bool? IsTeams { get; set; }  // 0 if teams, non-zero if not
        [JsonProperty("itemSpawnBehavior")]
        public int? ItemSpawnBehavior { get; set; }  // how frequently items spawn, -1 is off, 0 very low, 1 low, and so on
        [JsonProperty("stageId")]
        public int? StageId { get; set; }  // in-game stage ID
        [JsonProperty("startingTimerSeconds")]
        public int? StartingTimerSeconds { get; set; }  // number of seconds timer starts at
        [JsonProperty("enabledItems")]
        public double? EnabledItems { get; set; }
        [JsonProperty("players")]
        public List<Player> Players { get; set; }  // info of players playing in game
        [JsonProperty("scene")]
        public int? Scene { get; set; }  // should always be 2
        [JsonProperty("gameMode")]
        public int? GameMode { get; set; }  // 2 = versus mode, 8 = online
        [JsonProperty("language")]
        public int? Language { get; set; }  // 0 = japanese, 1 = english
        [JsonProperty("gameInfoBlock")]
        public Dictionary<string, string>? GameInfoBlock { get; set; }
        [JsonProperty("randomSeed")]
        public int? RandomSeed { get; set; }  // random seed before the game start
        [JsonProperty("isPAL")]
        public bool? IsPAL { get; set; }
        [JsonProperty("isFrozenPS")]
        public bool? IsFrozenPS { get; set; }
        [JsonProperty("matchInfo")]
        public Dictionary<string, string>? MatchInfo { get; set; }

    }
}
