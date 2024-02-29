using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.Filters
{
    public interface IFilter
    {
        public static abstract void addToQueue(GameConversions gameConversions, PlaybackQueue pbackQueue);
        public static abstract bool isInstance(Conversion conversion, GameSettings settings);
    }
}
