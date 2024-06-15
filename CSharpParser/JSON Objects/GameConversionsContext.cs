using System.Text.Json.Serialization;

namespace CSharpParser.JSON_Objects
{
    [JsonSerializable(typeof(GameConversions))]
    public partial class GameConversionsContext : JsonSerializerContext { }
}
