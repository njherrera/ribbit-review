using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpParser.SlpJSObjects;
using Newtonsoft.Json;

namespace CSharpParser.JSON_Objects
{
    public class GameConversions
    {
        /* holds all the conversions that occurred in a given replay, along with the game's path and settings
         * using public props instead of private because of newtonsoft constraints, will look into ways that we can incorporate private variables instead
         * also using the "JsonProperty" tag on these props + GameSettings props because it was a possible fix for some issues i tried out, but newtonsoft.json is actually case-insensitive when matching fields and it wasn't necessary
        */
        [JsonProperty("gameLocation")]
        public string GameLocation { get; set; }
        [JsonProperty("gameSettings")]
        public GameSettings GameSettings { get; set; }
        [JsonProperty("conversionList")]  
        public IList<Conversion>? ConversionList { get; set; }


        public override string ToString()
        {
            return $"Game location: {this.GameLocation}, Settings: {this.GameSettings.ToString}, Number of conversions: {this.ConversionList.Count()}";
        }
    }
}
