using CSharpParser.SlpJSObjects;

namespace CSharpParser.JSON_Objects
{
    public class GameConversions
    {
        /* holds all the conversions that occurred in a given replay, along with the game's path and settings
         * using public props instead of private because of newtonsoft constraints, will look into ways that we can incorporate private variables instead
         * also using the "JsonProperty" tag on these props + GameSettings props because it was a possible fix for some issues i tried out, but newtonsoft.json is actually case-insensitive when matching fields and it wasn't necessary
        */
        public string gameLocation { get; set; }
        public GameSettings gameSettings { get; set; }
        public IList<Conversion>? conversionList { get; set; }


        public override string ToString()
        {
            return $"Game location: {this.gameLocation}, Settings: {this.gameSettings.ToString}, Number of conversions: {this.conversionList.Count()}";
        }

    }
}
