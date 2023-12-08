using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.SlpJSObjects
{
    public class Player
    {
        public int playerIndex { get; set; }  // index of player, 0-3 depending on port
        public int port { get; set; }  // player index + 1
        public int? characterId { get; set; }  // internal character ID, can only change throughout game for Zelda/Sheik
        public int? type { get; set; }  // 0 = human, 1 = CPU, 2 = demo, 3 = empty
        public int? startStocks { get; set; }  // stocks this player starts with
        public int? characterColor { get; set; }  // indicates which costume index player used 
        public int? teamShade { get; set; }  // coloration changes for multiples of character on same team, 0 = normal, 1 = light, 2 = dark
        public int? handicap { get; set; }  // handicap set on player
        public int? teamId { get; set; }  // only relevant if isTeams is true, 0 = red, 1 = blue, 2 = green
        public bool? staminaMode { get; set; }
        public bool? silentCharacter { get; set; }
        public bool? invisible { get; set; }
        public bool? lowGravity { get; set; }
        public bool? blackStockIcon { get; set; }
        public bool? metal { get; set; }
        public bool? startOnAngelPlatform { get; set; }
        public bool? rumbleEnabled { get; set; }
        public int? cpuLevel { get; set; } // indicates what level CPU is, still specified on human players
        public int? offenseRatio { get; set; } // knockback multiplier when this player hits another
        public int? defenseRatio { get; set; } // knockback multiplier when this player is hit
        public double? modelScale { get; set; } // multiplier on size scaling of the character's model
        public string? controllerFix { get; set; } // controller fix (UCF) being used
        public string? nametag { get; set; } // in-game tag used by player
        public string displayName { get; set; } // display name used by player for slippi online
        public string connectCode { get; set; } // connect code used by player for slippi online
        public string userId { get; set; } // firebase user ID of player for slippi online

    }
}
