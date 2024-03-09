﻿using CSharpParser.SlpJSObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.Filters.Settings
{
    // this class serves as a way to control when and how a filter is applied
    public abstract class FilterSettings
    {
        // properties all nullable, a null value corresponds to the user not selecting one for a given setting
        public string? convertingPlayer { get; set; } // who's the active/converting player, user or opponent?
        public bool? conversionKilled { get; set; }
        public int? startingPercent { get; set; }
        public int? endingPercent { get; set; }
        public int[]? movesUsed { get; set; } // when user selects a move, its integer move ID will be used to set this and/or startingMove
        public int? startingMove { get; set; }
        public string? openingType { get; set; }

        public abstract bool checkConversion(Conversion conversion);
    }
}