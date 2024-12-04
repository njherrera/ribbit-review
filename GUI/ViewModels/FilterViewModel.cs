using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpParser.Filters;
using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using Serilog;
using System;
using System.Collections.Generic;
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
        [ObservableProperty]
        private bool? _isLocalReplay;

        public FilterType FilterType { get; init; }

        public abstract FilterSettingsBuilder Builder { get; }

        public FilterViewModel()
        {
            ConvertingPlayerToMeCommand = new RelayCommand(ConvertingPlayerToMe);
            ConvertingPlayerToOppCommand = new RelayCommand(ConvertingPlayerToOpp);
            ConversionDidKillCommand = new RelayCommand(ConversionDidKill);
            ConversionDidNotKillCommand = new RelayCommand(ConversionDidNotKill);
            ResetConversionKilledCommand = new RelayCommand(ResetConversionKilled);
        }

        public abstract PlaybackQueue applyFilter(List<GameConversions> allGameConversions);

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

        public ICommand ResetConversionKilledCommand { get; }

        private void ResetConversionKilled()
        {
            this.ConversionKilled = null;
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

        partial void OnIsLocalReplayChanged(bool? value)
        {
            Builder.addIsLocalReplay(value);
        }
    }
}
