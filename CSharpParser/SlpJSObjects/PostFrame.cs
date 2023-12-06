using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.SlpJSObjects
{
    internal class PostFrame
    {
        private int? frame; // number of the frame (starts at 0, frame 0 is when timer starts counting down)
        private int? playerIndex; // between 0 and 3, port is index + 1
        private bool? isFollower; // 1 for nana and 0 otherwise
        private int? internalCharacterId; // internal character ID, can only change throughout game for Zelda/Sheik
        private int? actionStateId; // indicates action state that character is in
        private double? positionX; // x position of character
        private double? positionY; // y position of character
        private int? facingDirection; // -1 is facing left, 1 is facing right
        private double? percent; // current damage percent
        private double? shieldSize; // current size of shield from 0-60
        private int? lastAttackLanded; // ID of last attack that hit a player (attacks that hit reflectors, counters, absorbers, or link's shield do not modify this field)
        private int? currentComboCount; // combo count as defined by game
        private int? lastHitBy; // player that last hit this player
        private int? stocksRemaining; // number of stocks remaining
        private double? actionStateCounter; // number of frames action state has been active, can have fractional component for some actions
        private double? miscActionState; // while in hitstun, contains hitstun frames remaining
        private bool? isAirborne; // 0 = grounded, 1 = airborne
        private int? lastGroundId; // ID of last ground that the character stood on
        private int? jumpsRemaining; // number of jumps remaining
        private int? lCancelStatus; // 0 = none, 1 = successful, 2 = unsuccessful
        private int? hurtboxCollisionState; // 0 = vulnerable, 1 = invulnerable, 2 = intangible
        private SelfInducedSpeeds? selfInducedSpeeds;
        private int? hitlagRemaining; // 0 means not in hitlag
        private int? animationIndex; // indicates animation the character is in

        public PostFrame()
        {
            
        }
    }
}
