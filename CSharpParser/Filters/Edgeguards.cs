using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public class Edgeguards : Filter
    {

        public override bool isInstance(Conversion conversion, GameSettings settings)
        {
            bool isEdgeguardPosition = false;
            double ledgePosition = getLedgePositions(settings.stageId);
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
