using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpParser.Filters;
using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public abstract partial class FilterViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _userId;
        [ObservableProperty]
        private string? _convertingPlayer;
        [ObservableProperty]
        private bool? _conversionKilled;
        [ObservableProperty]
        private int? _startingPercent;
        [ObservableProperty]
        private int? _endingPercent;
        [ObservableProperty]
        private int[]? _movesUsed;
        [ObservableProperty]
        private int? _startingMove;
        [ObservableProperty]
        private string? _openingType;

        public FilterType FilterType { get; init; }

        public abstract FilterSettingsBuilder Builder { get; }
        
        public abstract Filter Filter { get; }

        public FilterViewModel()
        {
            ConvertingPlayerToMeCommand = new RelayCommand(ConvertingPlayerToMe);
            ConvertingPlayerToOppCommand = new RelayCommand(ConvertingPlayerToOpp);
            ConversionDidKillCommand = new RelayCommand(ConversionDidKill);
            ConversionDidNotKillCommand = new RelayCommand(ConversionDidNotKill);
        }

        public void applyFilter(List<GameConversions> allGameConversions, PlaybackQueue playbackQueue)
        {
            FilterSettings fSettings = Builder.Build();
            foreach (GameConversions conversions in allGameConversions)
            {
                this.Filter.addToQueue(conversions, playbackQueue, fSettings);
            }
        }

/*        private bool checkGameSettings(GameConversions gameConversions)
        {
            // checking if CharID >= 0 after checking for null because of workaround used in MainViewModel
            // if user selects a character and then selects "any character", the CharId will be -1
            if ((userCharId != null) && (userCharId >= 0)) 
            {
                string userCodeCaps = UserID.ToUpper();
                List<Player> gamePlayers = gameConversions.GameSettings.Players;
                return gamePlayers.Exists(x => (x.connectCode == userCodeCaps) && (x.characterId == userCharId));
            }
            else if ((opponentCharId != null) && (userCharId >= 0))
            {
                string userCodeCaps = UserID.ToUpper();
                List<Player> gamePlayers = gameConversions.GameSettings.Players;
                return gamePlayers.Exists(x => (x.connectCode != userCodeCaps) && (x.characterId == opponentCharId));
            }
            else if (stageId != null)
            {
                int? gameStageId = gameConversions.GameSettings.StageId;
                return stageId.Equals(gameStageId);
            }
            else { return true; }
        }*/

        public override string ToString()
        {
            return this.FilterType.ToString();
        }

        public ICommand ConvertingPlayerToMeCommand { get; }
        private void ConvertingPlayerToMe()
        {
            this.ConvertingPlayer = "user";
        }

        public ICommand ConvertingPlayerToOppCommand { get; }

        private void ConvertingPlayerToOpp()
        {
            this.ConvertingPlayer = "opponent";
        }

        public ICommand ConversionDidKillCommand { get; }

        private void ConversionDidKill()
        {
            this.ConversionKilled = true;
        }

        public ICommand ConversionDidNotKillCommand { get; }

        private void ConversionDidNotKill()
        {
            this.ConversionKilled = false;
        }

        partial void OnUserIdChanged(string? value)
        {
            Builder.addUserID(value);
        }

        partial void OnConvertingPlayerChanged(string? value)
        {
            Builder.addConvertingPlayer(value);
        }

        partial void OnConversionKilledChanged(bool? value)
         {
            Builder.addConversionKilled(value);
        }
    }
}
