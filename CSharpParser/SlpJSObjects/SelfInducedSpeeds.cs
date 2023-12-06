using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.SlpJSObjects
{
    internal class SelfInducedSpeeds
    {
        private double? airX; // 	Negative means left, Positive means right
        private double? y; // 		Negative means down, Positive means up
        private double? attackX; //  Negative means left, Positive means right
        private double? attackY; //  Negative means down, Positive means up
        private double? groundX; //  Negative means left, Positive means right
    }
}
