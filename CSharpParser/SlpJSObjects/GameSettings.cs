using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSharpParser.SlpJSObjects
{
    internal class GameSettings
    {
        private string? slpVersion;
        private int? timerType; 
        private int? inGameMode; 
        private bool? friendlyFireEnabled;
        private bool? isTeams; // 0 if teams, non-zero if not
        private int? stageId; // in-game stage ID
        private int? startingTimerSeconds; // number of seconds timer starts at
        private int? itemSpawnBehavior; // how frequently items spawn, -1 is off, 0 very low, 1 low, and so on
        private int? enabledItems;
        private List<Player> players; // info of players playing in game
        private int? scene; // should always be 2
        private int? gameMode; // 2 = versus mode, 8 = online
        private int? language; // 0 = japanese, 1 = english
        private Dictionary<string, int>? gameInfoBlock;
        private int? randomSeed; // random seed before the game start
        private bool? isPAL; 
        private bool? isFrozenPS;
        private Dictionary<string, string>? matchInfo;

        public GameSettings()
        {
            
        }
    }
}
