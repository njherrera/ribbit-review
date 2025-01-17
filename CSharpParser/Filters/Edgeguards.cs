using CSharpParser.Filters.Settings;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public class Edgeguards<T> : Filter<T> where T : EdgeguardSettings
    {
        private int ledgeCrossFrame;
        private (double left, double right) ledgeCoords;

        public override bool IsInstance(Conversion conversion, GameSettings settings)
        {
            ledgeCrossFrame = -1;
            bool isEdgeguardPosition = false;

            for (int i = 0; i < conversion.victimFrames.Count(); i++)
            {
                double? attackerXPos = conversion.attackerFrames.ElementAt(i).positionX;
                double? victimXPos = conversion.victimFrames.ElementAt(i).positionX;

                // if victim's x position is past either ledge, attacker's x position is closer to stage, and ledgeCrossFrame is -1
                // it's first time so far in conversion that they've gone offstage and it's an edgeguard position
                if (((victimXPos < ledgeCoords.left) && (victimXPos < attackerXPos))
                    || ((victimXPos > ledgeCoords.right) && (victimXPos > attackerXPos)))
                {
                    ledgeCrossFrame = (int)conversion.victimFrames[i].frame;
                    isEdgeguardPosition = true;
                    return isEdgeguardPosition;
                } else continue;
            }
            return isEdgeguardPosition;
        }

        protected override bool CheckSettings(Conversion conversion, T fSettings, List<Player> gamePlayers)
        {
            // by default going to return true, since if every setting is null then user wants to see every instance of situation
            bool passesCheck = true;

            // check for edgeguarding player
            if (fSettings.convertingPlayer != null && fSettings.userID != null)
            {
                if (CheckVictim(conversion, fSettings, gamePlayers) == false) { return false; }
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
            if (fSettings.hitstunExitBelowLedge != null)
            {
                (double xPos, double yPos)? HSExitPos = CheckHitstunExitPos(conversion);
                if (fSettings.hitstunExitBelowLedge == true && HSExitPos is not null)
                {
                    bool meetsCondition = HSExitPos.Value.yPos < 0;
                    if (meetsCondition == false) { return false; }
                }
                else if (fSettings.hitstunExitBelowLedge == false && HSExitPos is not null)
                {
                    bool meetsCondition = HSExitPos.Value.yPos > 0;
                    if (meetsCondition == false) { return false; }
                }
                else return false;
            }
            if (fSettings.offstageMove != null)
            {
                bool meetsCondition = CheckOffstageMove(conversion, fSettings.offstageMove.Value);
                if (meetsCondition == false) { return false; }
            }
            return passesCheck;
        }

        private bool CheckOffstageMove(Conversion conversion, int offstageMoveID)
        {
            Dictionary<int, int> vicFrameIndices = conversion.moves.ToDictionary(move => move.frame, move => move.frame - conversion.victimFrames.ElementAt(0).frame.Value);
            List<Move> offstageMoves = conversion.moves.Where(move => move.frame > ledgeCrossFrame && move.playerIndex == conversion.attackerIndex 
                                                                    && (conversion.victimFrames.ElementAt(vicFrameIndices[move.frame]).positionX < ledgeCoords.left
                                                                    || conversion.victimFrames.ElementAt(vicFrameIndices[move.frame]).positionX > ledgeCoords.right)).ToList();
            List<int> offstageMoveIDs;
            if (offstageMoves.Count == 0) { return false; }
            else
            { 
                offstageMoveIDs = offstageMoves.Select(move => move.moveID).ToList();
            }

            if (offstageMoveID == 0) { return true; } 
            else 
            {
                bool passesCheck = offstageMoveIDs.Contains(offstageMoveID);
                return passesCheck;
            }
        }
        private int CheckSendOffMove(Conversion conversion)
        {
            int moveID = -1;
            Move sendOffMove = conversion.moves.Last(move => move.frame < ledgeCrossFrame && move.playerIndex == conversion.attackerIndex);
            moveID = sendOffMove.moveID;
            return moveID;
        }

        private (double xPos, double yPos)? CheckHitstunExitPos(Conversion conversion)
        {
            PostFrame? exitFrame = conversion.victimFrames.FirstOrDefault(frame
                => (frame.positionX < ledgeCoords.left || frame.positionX > ledgeCoords.right)
                    && frame.miscActionState == 0);
            if (exitFrame != null && exitFrame.positionX.HasValue && exitFrame.positionY.HasValue)
            {
                (double xPos, double yPos) hitstunExitPos = (exitFrame.positionX.Value, exitFrame.positionY.Value);
                return hitstunExitPos;
            }
            else return null;
        }
        private static double GetLedgePositions(int? stageId)
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

        public override void InitializeStageVars(GameSettings settings)
        {
            double ledgePosition = GetLedgePositions(settings.stageId);
            ledgeCoords.left = ledgePosition * -1;
            ledgeCoords.right = ledgePosition;
        }
    }
}
