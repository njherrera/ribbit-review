using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.Filters
{
    public abstract class Filter
    {
        public FilterType FilterType { get; init; }
        public void addToQueue(GameConversions gameConversions, PlaybackQueue pbackQueue, FilterSettings fSettings)
        {
            foreach (Conversion conversion in gameConversions.ConversionList)
            {
                if (conversion.beingHitFrames.Count() > 0 && fSettings.checkConversion(conversion) == true && isInstance(conversion, gameConversions.GameSettings) == true)
                {
                    int? startFrame = conversion.beingHitFrames.First().frame;
                    int? endFrame = conversion.beingHitFrames.Last().frame;
                    QueueItem qi = new QueueItem(gameConversions.GameLocation, startFrame, endFrame);
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
