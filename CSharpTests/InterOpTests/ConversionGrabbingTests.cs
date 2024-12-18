using CSharpParser.JSON_Objects;
using Jering.Javascript.NodeJS;

namespace CSharpTests.InterOpTests
{
    [TestClass]
    public class ConversionGrabbingTests
    {

        /* Using EdgeguardTestSheikFalco.slp
         * file path: ...\ribbit-review\.slp files\EdgeguardTestSheikFalco.slp
         * gameSettings.players[0].connectCode = "THIQ#306" (...players[0].characterId = 20 (Falco))
         * gameSettings.players[1].connectCode = "MMRP#834" (...players[1].characterId = 19 (Sheik))
         * gameSettings.stageId = 31
         * 19 total conversions (18 if not counting LRA start at end)
        */

        [TestMethod]
        public async Task testGetConversions()
        {
            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            object[] args = { userVars.edgeguardSlpPath };
            GameConversions testConversions = await StaticNodeJSService.InvokeFromFileAsync<GameConversions>("./JavaScript/interop.js", "getGameConversions", args);

            Assert.IsNotNull(testConversions);
            Assert.AreEqual(testConversions.gameLocation, userVars.edgeguardSlpPath);
            Assert.AreEqual(19, testConversions.conversionList.Count);
            Assert.AreEqual(31, testConversions.gameSettings.stageId);
            Assert.AreEqual("THIQ#306", testConversions.gameSettings.players[0].connectCode);
            Assert.AreEqual("MMRP#834", testConversions.gameSettings.players[1].connectCode);
            Assert.AreEqual(20, testConversions.gameSettings.players[0].characterId);
            Assert.AreEqual(19, testConversions.gameSettings.players[1].characterId);
        }

        [TestMethod]
        public async Task testGetAllConversions()
        {
            string dummyConstraints = "userId: userChar: oppChar: stageId: isLocal: ";
            List<string> testPaths = new List<string>{ @"file:\\" + userVars.edgeguardSlpPath, @"file:\\" + userVars.meVsPinkSlpPath };
            object[] args = { dummyConstraints, string.Join(",", testPaths) };

            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            List<GameConversions> testConversions = await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/interop.js", "getAllConversions", args);

            string edgeguardUri = new System.Uri(userVars.edgeguardSlpPath).AbsoluteUri;
            string meVsPinkUri = new System.Uri(userVars.meVsPinkSlpPath).AbsoluteUri;

            Assert.AreEqual(edgeguardUri, testConversions[0].gameLocation);
            Assert.AreEqual(2, testConversions.Count);
            Assert.AreEqual(meVsPinkUri, testConversions[1].gameLocation);
            Assert.AreEqual(testConversions[0].gameSettings.players[1].connectCode, testConversions[1].gameSettings.players[0].connectCode);
            Assert.AreEqual(testConversions[0].gameSettings.players[1].characterId, testConversions[1].gameSettings.players[0].characterId);
            Assert.AreEqual(testConversions[0].gameSettings.stageId, testConversions[1].gameSettings.stageId);
        }

        [TestMethod]
        public async Task testGetConversionsWithConstraints()
        {
            string dummyConstraints1 = "userId:MMRP#834 userChar: oppChar: stageId:8 isLocal:False";
            List<string> testPaths = new List<string> { @"file:\\" + userVars.edgeguardSlpPath, @"file:\\" + userVars.meVsPinkSlpPath, @"file:\\" + userVars.meVsIcsYoshis, @"file:\\" + userVars.oogaVsFalconYoshis };
            object[] args = { dummyConstraints1, string.Join(",", testPaths) };

            StaticNodeJSService.Configure<NodeJSProcessOptions>(options => options.ProjectPath = userVars.interOpPath);
            List<GameConversions> testConversions = await StaticNodeJSService.InvokeFromFileAsync<List<GameConversions>>("./JavaScript/interop.js", "getAllConversions", args);

            Assert.AreEqual(1, testConversions.Count);
        }
    }
}
