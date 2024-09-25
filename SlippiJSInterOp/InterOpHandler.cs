using CSharpParser.JSON_Objects;
using Jering.Javascript.NodeJS;
using System.Data.Common;
using System.Reflection;

namespace SlippiJSInterOp

{
    public class InterOpHandler
    {
        
        public static void configureNodeService(string projectPath)
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
    }
}
