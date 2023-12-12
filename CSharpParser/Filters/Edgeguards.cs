using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpParser.JSON_Objects;

namespace CSharpParser.Filters
{
    public static class Edgeguards
    {
        public static void addToJson(GameConversions gameConversions, PlaybackQueue dolphinQueue) 
        {
            foreach (Conversion conversion in gameConversions.ConversionList)
            {
                if (conversion.beingHitFrames.Count() > 0 && isEdgeguard(conversion, gameConversions.GameSettings.StageId) == true)
                {
                    QueueItem qi = new QueueItem(gameConversions.GameLocation, conversion.beingHitFrames.First().frame, conversion.beingHitFrames.Last().frame);
                    dolphinQueue.queue.Add(qi);
                }
                else continue;
            }
        }

        private static bool isEdgeguard(Conversion conversion, int? stageId)
        {
            bool isEdgeguardPosition = false;
            double ledgePosition = getLedgePositions(stageId);
            double leftLedge = ledgePosition * -1;
            double rightLedge = ledgePosition;

            for (int i = 0; i <= conversion.beingHitFrames.Count(); i++)
            {
                double? convertingXPosition = conversion.hittingFrames.ElementAt(i).positionX;
                double? converteeXPosition = conversion.beingHitFrames.ElementAt(i).positionX;

                if (((converteeXPosition < leftLedge) && (converteeXPosition < convertingXPosition)) || ((converteeXPosition > rightLedge) && (converteeXPosition > convertingXPosition)))
                {
                    isEdgeguardPosition = true;
                } else continue;
            }
            return isEdgeguardPosition;
        }

        private static double getLedgePositions(int? stageId)
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
