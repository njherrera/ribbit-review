using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpParser.Filters;
using CSharpParser.Filters.Settings;
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
        private string? userID;
        [ObservableProperty]
        private string? convertingPlayer;
        [ObservableProperty]
        private bool? conversionKilled;
        [ObservableProperty]
        private int? startingPercent;
        [ObservableProperty]
        private int? endingPercent;
        [ObservableProperty]
        private int[]? movesUsed;
        [ObservableProperty]
        private int? startingMove;
        [ObservableProperty]
        private string? openingType;
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

        partial void OnUserIDChanged(string? value)
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
