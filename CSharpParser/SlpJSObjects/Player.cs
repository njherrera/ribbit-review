using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.SlpJSObjects
{
    internal class Player
    {
        private int playerIndex; // index of player, 0-3 depending on port
        private int port; // player index + 1
        private int? characterId; // internal character ID, can only change throughout game for Zelda/Sheik
        private int? type; // 0 = human, 1 = CPU, 2 = demo, 3 = empty
        private int? startStocks; // stocks this player starts with
        private int? characterColor; // indicates which costume index player used 
        private int? teamShade; // coloration changes for multiples of character on same team, 0 = normal, 1 = light, 2 = dark
        private int? handicap; // handicap set on player
        private int? teamId; // only relevant if isTeams is true, 0 = red, 1 = blue, 2 = green
        private bool? staminaMode; 
        private bool? silentCharacter;
        private bool? invisible;
        private bool? lowGravity;
        private bool? blackStockIcon;
        private bool? metal;
        private bool? startOnAngelPlatform;
        private bool? rumbleEnabled;
        private int? cpuLevel; // indicates what level CPU is, still specified on human players
        private int? offenseRatio; // knockback multiplier when this player hits another
        private int? defenseRatio; // knockback multiplier when this player is hit
        private double? modelScale; // multiplier on size scaling of the character's model
        private string? controllerFix; // controller fix (UCF) being used
        private string? nametag; // in-game tag used by player
        private string displayName; // display name used by player for slippi online
        private string connectCode; // connect code used by player for slippi online
        private string userId; // firebase user ID of player for slippi online

        public Player()
        {
            
        }
    }
}
