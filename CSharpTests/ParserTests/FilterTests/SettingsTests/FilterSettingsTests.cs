using CSharpParser.Filters;
using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using Jering.Javascript.NodeJS;
namespace CSharpTests.ParserTests.FilterTests.SettingsTests
{
    [TestClass]
    public class FilterSettingsTests
    {
        /* Using EdgeguardTestSheikFalco.slp (Sheik player = MMRP#834, Falco player = THIQ#306)
         * # of Sheik edgeguard situations: 7
         * # of Falco edgeguard situations: 5
         * # of successful Sheik edgeguards: 3
         * # of successful Falco edgeguards: 1
         * # of unsuccessful Sheik edgeguards: 4
         * # of unsuccessful Falco edgeguards: 4
         */
        Edgeguards<EdgeguardSettings> edgeguardFilter = new Edgeguards<EdgeguardSettings>();

        [TestMethod]
        public async Task testConvertingPlayer()
        {
            string dummyConstraints = "userId: userChar: oppChar: stageId: isLocal: ";
            List<string> testPaths = new List<string> { @"file:\\" + userVars.edgeguardSlpPath};
            object[] args = { dummyConstraints, string.Join(",", testPaths) };

            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            List<GameConversions> testConversions = await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/interop.js", "getAllConversions", args);

            EdgeguardSettingsBuilder sheikBuilder = new EdgeguardSettingsBuilder();
            sheikBuilder.addUserID("mmrp#834");
            sheikBuilder.addConvertingPlayer("user");
            sheikBuilder.addIsLocalReplay(false);
            EdgeguardSettings sheikConverting = (EdgeguardSettings)sheikBuilder.Build();
            
            PlaybackQueue pbackQueueSheik = edgeguardFilter.AddToQueue(testConversions, sheikConverting);

            EdgeguardSettingsBuilder falcoBuilder = new EdgeguardSettingsBuilder();
            falcoBuilder.addUserID("mmrp#834");
            falcoBuilder.addConvertingPlayer("opponent");
            EdgeguardSettings falcoConverting = (EdgeguardSettings)falcoBuilder.Build();

            PlaybackQueue pbackQueueFalco = edgeguardFilter.AddToQueue(testConversions, falcoConverting);

            Assert.AreEqual(7, pbackQueueSheik.queue.Count());
            Assert.AreEqual(5, pbackQueueFalco.queue.Count());
        }

        [TestMethod]
        public async Task testConversionKilled()
        {
            string dummyConstraints = "userId: userChar: oppChar: stageId: isLocal: ";
            List<string> testPaths = new List<string> { @"file:\\" + userVars.edgeguardSlpPath };
            object[] args = { dummyConstraints, string.Join(",", testPaths) };

            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            List<GameConversions> testConversions = await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/interop.js", "getAllConversions", args);

            EdgeguardSettingsBuilder sheikBuilder = new EdgeguardSettingsBuilder();
            sheikBuilder.addUserID("MMRP#834");
            sheikBuilder.addConvertingPlayer("user");
            sheikBuilder.addConversionKilled(true);
            sheikBuilder.addIsLocalReplay(false);
            EdgeguardSettings sheikKilled = (EdgeguardSettings)sheikBuilder.Build();

            PlaybackQueue sheikQueue = edgeguardFilter.AddToQueue(testConversions, sheikKilled);

            EdgeguardSettingsBuilder falcoBuilder = new EdgeguardSettingsBuilder();
            falcoBuilder.addUserID("mmrp#834");
            falcoBuilder.addConvertingPlayer("opponent");
            falcoBuilder.addConversionKilled(true);
            falcoBuilder.addIsLocalReplay(false);
            EdgeguardSettings falcoConverting = (EdgeguardSettings)falcoBuilder.Build();

            PlaybackQueue falcoQueue = edgeguardFilter.AddToQueue(testConversions, falcoConverting);

            Assert.AreEqual(3, sheikQueue.queue.Count());
            Assert.AreEqual(1, falcoQueue.queue.Count());
        }

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
    }
}
