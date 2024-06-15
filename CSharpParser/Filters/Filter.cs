using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public abstract class Filter
    {
        public FilterType FilterType { get; init; }

        public void addToQueue(GameConversions gameConversions, PlaybackQueue pbackQueue, FilterSettings fSettings)
        {
            foreach (Conversion conversion in gameConversions.conversionList)
            {
                if (conversion.beingHitFrames.Count() > 0 && fSettings.checkConversion(conversion) == true && isInstance(conversion, gameConversions.gameSettings) == true)
                {
                    int? startFrame = conversion.beingHitFrames.First().frame;
                    int? endFrame = conversion.beingHitFrames.Last().frame;
                    QueueItem qi = new QueueItem(gameConversions.gameLocation, startFrame, endFrame);
                    pbackQueue.queue.Add(qi);
                }
                else continue;
            }
        }

        public abstract bool isInstance(Conversion conversion, GameSettings settings);

        public override string ToString()
        {
            return this.FilterType.ToString();
        }
    }
}
