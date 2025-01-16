using CSharpParser.Filters.Settings;
using CSharpParser.Filters;
using CSharpParser.JSON_Objects;
using Jering.Javascript.NodeJS;

namespace CSharpTests.ParserTests.FilterTests.SettingsTests
{
    [TestClass]
    public class EdgeguardSettingsTests
    {
        Edgeguards<EdgeguardSettings> edgeguardFilter = new Edgeguards<EdgeguardSettings>();

        [TestMethod]
        public async Task testHitstunExitBelowLedge()
        {
            string dummyConstraints = "userId: userChar: oppChar: stageId: isLocal: ";
            List<string> testPaths = new List<string> { @"file:\\" + userVars.edgeguardSlpPath };
            object[] args = { dummyConstraints, string.Join(",", testPaths) };

            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            List<GameConversions> testConversions = await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/interop.js", "getAllConversions", args);

            EdgeguardSettingsBuilder HEPBuilder = new EdgeguardSettingsBuilder();
            HEPBuilder.addHitstunExitBelowLedge(true);
            EdgeguardSettings HEPBelowLedge = (EdgeguardSettings)HEPBuilder.Build();

            PlaybackQueue belowLedgeQueue = edgeguardFilter.AddToQueue(testConversions, HEPBelowLedge);
            Assert.AreEqual(8, belowLedgeQueue.queue.Count());
        }

        [TestMethod]
        public async Task testHitstunExitAboveLedge()
        {
            string dummyConstraints = "userId: userChar: oppChar: stageId: isLocal: ";
            List<string> testPaths = new List<string> { @"file:\\" + userVars.edgeguardSlpPath };
            object[] args = { dummyConstraints, string.Join(",", testPaths) };

            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            List<GameConversions> testConversions = await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/interop.js", "getAllConversions", args);

            EdgeguardSettingsBuilder HEPBuilder = new EdgeguardSettingsBuilder();
            HEPBuilder.addHitstunExitBelowLedge(false);
            EdgeguardSettings HEPAboveLedge = (EdgeguardSettings)HEPBuilder.Build();

            PlaybackQueue aboveLedgeQueue = edgeguardFilter.AddToQueue(testConversions, HEPAboveLedge);
            Assert.AreEqual(1, aboveLedgeQueue.queue.Count);
        }

        [TestMethod]
        public async Task testMovesUsedOffstage()
        {
            string dummyConstraints = "userId: userChar: oppChar: stageId: isLocal: ";
            List<string> testPaths = new List<string> { @"file:\\" + userVars.puffVsMarthYoshis };
            object[] args = { dummyConstraints, string.Join(",", testPaths) };

            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            List<GameConversions> testConversions = await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/interop.js", "getAllConversions", args);

            EdgeguardSettingsBuilder offstageMoveBuilder = new EdgeguardSettingsBuilder();
            offstageMoveBuilder.addOffstageMove(0);
            EdgeguardSettings anyOffstageMove = (EdgeguardSettings)offstageMoveBuilder.Build();

            PlaybackQueue offstageMoveUsedQueue = edgeguardFilter.AddToQueue(testConversions, anyOffstageMove);
            Assert.AreEqual(4, offstageMoveUsedQueue.queue.Count);
        }
    }
}
