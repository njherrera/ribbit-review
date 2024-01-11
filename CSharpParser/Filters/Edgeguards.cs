using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public class Edgeguards : IFilter
    {

        public static void addToQueue(GameConversions gameConversions, PlaybackQueue pbackQueue)
        {
            foreach (Conversion conversion in gameConversions.ConversionList)
            {
                if (conversion.beingHitFrames.Count() > 0 && isInstance(conversion, gameConversions.GameSettings) == true)
                {
                    int? startFrame = conversion.beingHitFrames.First().frame;
                    int? endFrame = conversion.beingHitFrames.Last().frame;
                    QueueItem qi = new QueueItem(gameConversions.GameLocation, startFrame, endFrame);
                    pbackQueue.queue.Add(qi);
                }
                else continue;
            }
        }

        public static bool isInstance(Conversion conversion, GameSettings settings)
        {
            bool isEdgeguardPosition = false;
            double ledgePosition = getLedgePositions(settings.StageId);
            double leftLedge = ledgePosition * -1;
            double rightLedge = ledgePosition;

            for (int i = 0; i < conversion.beingHitFrames.Count(); i++)
            {
                double? converterXPosition = conversion.hittingFrames.ElementAt(i).positionX;
                double? converteeXPosition = conversion.beingHitFrames.ElementAt(i).positionX;

                if (((converteeXPosition < leftLedge) && (converteeXPosition < converterXPosition)) || ((converteeXPosition > rightLedge) && (converteeXPosition > converterXPosition)))
                {
                    isEdgeguardPosition = true;
                } else continue;
            }
            return isEdgeguardPosition;
        }

        public static double getLedgePositions(int? stageId)
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
