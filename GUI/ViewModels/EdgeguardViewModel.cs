using CSharpParser.Filters.Settings;
using CSharpParser.Filters;
using CommunityToolkit.Mvvm.ComponentModel;
using GUI.Models;
using System;
using CSharpParser.JSON_Objects;
using System.Collections.Generic;
using Serilog;

namespace GUI.ViewModels
{
    public partial class EdgeguardViewModel : FilterViewModel
    {
        public MoveType[] AvailableMoves { get; } = Enum.GetValues<MoveType>();

        public HitstunExitPositionType[] HitstunExits { get; } = Enum.GetValues<HitstunExitPositionType>();

        public HitOffstageMoveType[] AvailableOffstageMoves { get; } = Enum.GetValues<HitOffstageMoveType>();

        [ObservableProperty]
        private MoveType _selectedSendOffMove;

        [ObservableProperty]
        private HitstunExitPositionType _selectedHSEP;

        [ObservableProperty]
        private HitOffstageMoveType _selectedOffstageMove;

        partial void OnSelectedSendOffMoveChanged(MoveType value)
        {
            if ((int)value == 0)
            {
                _builder.addSendOffMove(null);
            }
            else { _builder.addSendOffMove((int)value);}
        }

        partial void OnSelectedHSEPChanged(HitstunExitPositionType value)
        {
            switch ((int)value)
            {
                case 0:
                    _builder.addHitstunExitBelowLedge(null);
                    break;
                case 1: 
                    _builder.addHitstunExitBelowLedge(false);
                    break;
                case 2: 
                    _builder.addHitstunExitBelowLedge(true);
                    break;
            }
        }

        partial void OnSelectedOffstageMoveChanged(HitOffstageMoveType value)
        {
            switch ((int)value)
            {
                case 0:
                    _builder.addOffstageMove(null);
                    break;
                case 1:
                    _builder.addOffstageMove(0);
                    break;
                default:
                    _builder.addOffstageMove((int)(value));
                    break;
            }
        }

        public override PlaybackQueue applyFilter(List<GameConversions> allGameConversions)
        {
            EdgeguardSettings fSettings = (EdgeguardSettings)_builder.Settings;
            PlaybackQueue pBackQueue = new PlaybackQueue();

            try
            {
                pBackQueue = _filter.AddToQueue(allGameConversions, fSettings);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred when calling active FilterViewModel's applyFilter method");
            }
            return pBackQueue;
        }

        public override FilterSettingsBuilder Builder
        {
            get
            {
                return this._builder;
            }
        }

        private EdgeguardSettingsBuilder _builder = new EdgeguardSettingsBuilder();

        private Edgeguards<EdgeguardSettings> _filter = new Edgeguards<EdgeguardSettings>();


    }
}
