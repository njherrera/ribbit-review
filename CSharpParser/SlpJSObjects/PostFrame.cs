using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.SlpJSObjects
{
    public class PostFrame
    {
        public int? frame { get; set; } // number of the frame (starts at 0, frame 0 is when timer starts counting down)
        public int? playerIndex { get; set; }  // between 0 and 3, port is index + 1
        public bool? isFollower { get; set; }  // 1 for nana and 0 otherwise
        public int? internalCharacterId { get; set; }  // internal character ID, can only change throughout game for Zelda/Sheik
        public int? actionStateId { get; set; }  // indicates action state that character is in
        public double? positionX { get; set; }  // x position of character
        public double? positionY { get; set; }  // y position of character
        public int? facingDirection { get; set; }  // -1 is facing left, 1 is facing right
        public double? percent { get; set; }  // current damage percent
        public double? shieldSize { get; set; }  // current size of shield from 0-60
        public int? lastAttackLanded { get; set; }  // ID of last attack that hit a player (attacks that hit reflectors, counters, absorbers, or link's shield do not modify this field)
        public int? currentComboCount { get; set; }  // combo count as defined by game
        public int? lastHitBy { get; set; }  // player that last hit this player
        public int? stocksRemaining { get; set; }  // number of stocks remaining
        public double? actionStateCounter { get; set; }  // number of frames action state has been active, can have fractional component for some actions
        public double? miscActionState { get; set; }  // while in hitstun, contains hitstun frames remaining
        public bool? isAirborne { get; set; }  // 0 = grounded, 1 = airborne
        public int? lastGroundId { get; set; }  // ID of last ground that the character stood on
        public int? jumpsRemaining { get; set; }  // number of jumps remaining
        public int? lCancelStatus { get; set; }  // 0 = none, 1 = successful, 2 = unsuccessful
        public int? hurtboxCollisionState { get; set; }  // 0 = vulnerable, 1 = invulnerable, 2 = intangible
        public Dictionary<string, double>? selfInducedSpeeds { get; set; }
        public int? hitlagRemaining { get; set; }  // 0 means not in hitlag
        public uint? animationIndex { get; set; }  // indicates animation the character is in

    }
}
