using CSharpParser.Filters;
using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;
using Jering.Javascript.NodeJS;


namespace CSharpTests.ParserTests.FilterTests
{
    [TestClass]
    public class EdgeguardTests
    {
        Edgeguards<EdgeguardSettings> edgeguardFilter = new Edgeguards<EdgeguardSettings>(); 

        [TestMethod]
        public async Task testIsEdgeguard()
        {
            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            object[] args = { userVars.edgeguardSlpPath };
            GameConversions testConversions = await StaticNodeJSService.InvokeFromFileAsync<GameConversions>("./JavaScript/interop.js", "getGameConversions", args);

            edgeguardFilter.InitializeStageVars(testConversions.gameSettings);
            Conversion shouldBeEdgeuard = testConversions.conversionList.ElementAt(6);
            Conversion notEdgeguard = testConversions.conversionList.ElementAt(2);

            Assert.IsTrue(edgeguardFilter.IsInstance(shouldBeEdgeuard, testConversions.gameSettings));
            Assert.IsFalse(edgeguardFilter.IsInstance(notEdgeguard, testConversions.gameSettings));
        }

        [TestMethod]
        public async Task testAddToQueue()
        {
            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            object[] args = { userVars.edgeguardSlpPath };
            GameConversions testConversions = await StaticNodeJSService.InvokeFromFileAsync<GameConversions>("./JavaScript/interop.js", "getGameConversions", args);

            EdgeguardSettings eSettings = new EdgeguardSettings();
            List<GameConversions> conversionList = new List<GameConversions>();
            conversionList.Add(testConversions);
            PlaybackQueue pbackQueue = edgeguardFilter.AddToQueue(conversionList, eSettings);
            Assert.AreEqual(pbackQueue.queue.Count(), 12);
        }
    }
}
