using CSharpParser.Filters.Settings;
using CSharpParser.SlpJSObjects;
using System.Runtime;

namespace CSharpParser.Filters
{
    public class Edgeguards<T> : Filter<T> where T : EdgeguardSettings
    {
        private int ledgeCrossFrame;

        public override bool IsInstance(Conversion conversion, GameSettings settings)
        {
            bool isEdgeguardPosition = false;
            double ledgePosition = GetLedgePositions(settings.stageId);
            double leftLedge = ledgePosition * -1;
            double rightLedge = ledgePosition;

            for (int i = 0; i < conversion.beingHitFrames.Count(); i++)
            {
                double? converterXPosition = conversion.hittingFrames.ElementAt(i).positionX;
                double? converteeXPosition = conversion.beingHitFrames.ElementAt(i).positionX;

                if (((converteeXPosition < leftLedge) && (converteeXPosition < converterXPosition)) || ((converteeXPosition > rightLedge) && (converteeXPosition > converterXPosition)))
                {
                    ledgeCrossFrame = (int)conversion.beingHitFrames[i].frame;
                    isEdgeguardPosition = true;
                } else continue;
            }
            return isEdgeguardPosition;
        }

        protected override bool CheckSettings(Conversion conversion, T fSettings)
        {
            // by default going to return true, since if every setting is null then user wants to see every instance of situation
            bool passesCheck = true;

            // check for edgeguarding player
            if (fSettings.convertingPlayer != null && fSettings.userID != null)
            {
                if (CheckVictim(conversion, fSettings.userID, fSettings.convertingPlayer) == false) { return false; }
            }
            // check for whether conversion killed
            if (fSettings.conversionKilled != null)
            {
                bool meetsCondition = fSettings.conversionKilled.Equals(conversion.didKill);
                if (meetsCondition == false) { return false; }
            }
            if (fSettings.sendOffMove != null)
            {
                bool meetsCondition = fSettings.sendOffMove.Equals(CheckSendOffMove(conversion));
                if (meetsCondition == false) { return false; }
            }
            return passesCheck;
        }

        private int CheckSendOffMove(Conversion conversion)
        {
            int moveID = -1;
            Move sendOffMove = conversion.moves.Last(move => move.frame < ledgeCrossFrame && move.playerIndex == conversion.playerHitting);
            moveID = sendOffMove.moveID;
            return moveID;
        }

        public static double GetLedgePositions(int? stageId)
        {
            switch (stageId)
            {
                case 2: // FoD
                    return 63.35;

                case 3: // Stadium
                    return 87.75;

                case 8: // Yoshi's
                    return 56;

                case 28: // Dream Land
                    return 77.27;

                case 31: // Battlefield
                    return 68.4;

                case 32: // FD
                    return 85.57;

                default:
                    return 0;
            }
        }
    }
}
