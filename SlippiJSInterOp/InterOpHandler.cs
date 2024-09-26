using CSharpParser.JSON_Objects;
using Jering.Javascript.NodeJS;
using System.Data.Common;
using System.Reflection;

namespace SlippiJSInterOp

{
    public class InterOpHandler
    {
        
        public static void setProjectPath(string projectPath)
        {
            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = projectPath);
        }

        public static async Task<GameConversions?> GetFileConversions(string location)
        {
            object[] args = { location };
            
            GameConversions? requestConversions =
                await StaticNodeJSService.InvokeFromFileAsync<GameConversions>("./JavaScript/ConversionGetter.js", "getGameConversions", args);

            return requestConversions;
        }

        public static async Task<List<GameConversions?>> GetAllConversions(string constraints, string locations)
        {
            object[] args = { constraints, locations };

            List<GameConversions>? requestConversions =
                await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/ConversionGetter.js", "getAllConversions", args);

            return requestConversions;
        }
    }
}
