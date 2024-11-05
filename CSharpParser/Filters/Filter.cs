using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public abstract class Filter<T> where T : FilterSettings
    {
        public FilterType FilterType { get; init; }

        // TODO: handle List<GameConversions> with no GameConversions objects in it (null check)
        public PlaybackQueue? AddToQueue(List<GameConversions>? allConversions, T fSettings)
        {
            if (allConversions != null)
            {
                PlaybackQueue pbackQueue = new PlaybackQueue();
                foreach (GameConversions gameConversions in allConversions)
                {
                    CheckGameConversions(gameConversions, fSettings, pbackQueue);
                }
                return pbackQueue;
            } else return null;
        }

        private void CheckGameConversions(GameConversions gameConversions, T fSettings, PlaybackQueue pbackQueue)
        {
            foreach (Conversion conversion in gameConversions.conversionList)
            {
                if (conversion.beingHitFrames.Count() > 0 && IsInstance(conversion, gameConversions.gameSettings) == true &&  CheckSettings(conversion, fSettings) == true)
                {
                    int? startFrame = conversion.beingHitFrames.First().frame;
                    int? endFrame = conversion.beingHitFrames.Last().frame;
                    QueueItem qi = new QueueItem(gameConversions.gameLocation, startFrame, endFrame);
                    pbackQueue.queue.Add(qi);
                }
                else continue;
            }
        }

        protected abstract bool CheckSettings(Conversion conversion, T fSettings);

        protected bool CheckVictim(Conversion conversion, string userID, string convertingPlayer)
        {
            if (convertingPlayer.Equals("user"))
            {
                string userIDCaps = userID.ToUpper();
                bool passesCheck = userIDCaps.Equals(conversion.hittingConnectCode);
                return passesCheck;
            }
            else // if we're calling this and the user isn't marked as the converting player, the only other option is that the opponent is marked as the converting player
            {
                string userIDCaps = userID.ToUpper();
                bool passesCheck = userIDCaps.Equals(conversion.beingHitConnectCode);
                return passesCheck;
            }
        }

        public abstract bool IsInstance(Conversion conversion, GameSettings settings);

        public override string ToString()
        {
            return this.FilterType.ToString();
        }
    }
}
