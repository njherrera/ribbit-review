using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public abstract class Filter<T> where T : FilterSettings
    {
        public FilterType FilterType { get; init; }

        public PlaybackQueue? AddToQueue(List<GameConversions>? allConversions, T fSettings)
        {
            if (allConversions != null)
            {
                PlaybackQueue pbackQueue = new PlaybackQueue();
                foreach (GameConversions gameConversions in allConversions)
                {
                    InitializeStageVars(gameConversions.gameSettings);
                    CheckGameConversions(gameConversions, fSettings, pbackQueue);
                }
                return pbackQueue;
            } else return null;
        }

        private void CheckGameConversions(GameConversions gameConversions, T fSettings, PlaybackQueue pbackQueue)
        {
            foreach (Conversion conversion in gameConversions.conversionList)
            {
                if (conversion.victimFrames.Count() > 0 && IsInstance(conversion, gameConversions.gameSettings) == true && CheckSettings(conversion, fSettings, gameConversions.gameSettings.players) == true)
                {
                    int? startFrame = conversion.victimFrames.First().frame;
                    int? endFrame = conversion.victimFrames.Last().frame;
                    QueueItem qi = new QueueItem(gameConversions.gameLocation, startFrame, endFrame);
                    pbackQueue.queue.Add(qi);
                }
                else continue;
            }
        }

        protected abstract bool CheckSettings(Conversion conversion, T fSettings, List<Player> gamePlayers);

        protected bool CheckVictim(Conversion conversion, T fSettings, List<Player> gamePlayers)
        {
            if (fSettings.convertingPlayer.Equals("user") && (fSettings.isLocalReplay == false))
            {
                string userIDCaps = fSettings.userID.ToUpper();
                bool passesCheck = userIDCaps.Equals(conversion.attackerConnectCode);
                return passesCheck;
            } else if (fSettings.convertingPlayer.Equals("user") && (fSettings.isLocalReplay == true))
            {
                string userIDCaps = fSettings.userID.ToUpper();
                bool passesCheck = userIDCaps.Equals(conversion.attackerNametag);
                return passesCheck;
            } else if (fSettings.convertingPlayer.Equals("opponent") && (fSettings.isLocalReplay == true))
            {
                string userIDCaps = fSettings.userID.ToUpper();
                bool passesCheck = userIDCaps.Equals(conversion.victimNametag);
                return passesCheck;
            }
            else
            {
                string userIDCaps = fSettings.userID.ToUpper();
                bool passesCheck = userIDCaps.Equals(conversion.victimConnectCode);
                return passesCheck;
            }
        }

        protected abstract void InitializeStageVars(GameSettings settings);
        public abstract bool IsInstance(Conversion conversion, GameSettings settings);

        public override string ToString()
        {
            return this.FilterType.ToString();
        }
    }
}
