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
        /*        private string gameLocation;
                private Dictionary<String, String> settings = new();
                private List<Conversion> conversions = new();*/
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
