using CSharpParser.JSON_Objects;
using SlippiJSInterOp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            InterOpHandler.setProjectPath(userVars.interOpPath);

            GameConversions? testConversions = await InterOpHandler.GetFileConversions(userVars.edgeguardSlpPath);
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
            InterOpHandler.setProjectPath(userVars.interOpPath);

            string dummyConstraints = "userId: userChar: oppChar: stageId: ";
            List<GameConversions?> testConversions = await InterOpHandler.GetAllConversions(dummyConstraints, "file:\\" + userVars.edgeguardSlpPath + "," + "file:\\" + userVars.meVsPinkSlpPath);
            
            Assert.AreEqual(2, testConversions.Count);
            string edgeguardUri = new System.Uri(userVars.edgeguardSlpPath).AbsoluteUri;
            Assert.AreEqual(edgeguardUri, testConversions[0].gameLocation);

            string meVsPinkUri = new System.Uri(userVars.meVsPinkSlpPath).AbsoluteUri;
            Assert.AreEqual(meVsPinkUri, testConversions[1].gameLocation);

            Assert.AreEqual(testConversions[0].gameSettings.players[1].connectCode, testConversions[1].gameSettings.players[0].connectCode);
            Assert.AreEqual(testConversions[0].gameSettings.players[1].characterId, testConversions[1].gameSettings.players[0].characterId);

            Assert.AreEqual(testConversions[0].gameSettings.stageId, testConversions[1].gameSettings.stageId);
        }

        [TestMethod]
        public async Task testGetConversionsWithConstraints()
        {
            InterOpHandler.setProjectPath(userVars.interOpPath);

            string dummyConstraints1 = "userId:MMRP#834 userChar: oppChar: stageId:8";
            List<GameConversions?> testConversions = 
                await InterOpHandler.GetAllConversions(dummyConstraints1, "file:\\" + userVars.edgeguardSlpPath + "," + "file:\\" + userVars.meVsPinkSlpPath + "," + "file:\\" + userVars.meVsIcsYoshis + "," + "file:\\" + userVars.oogaVsFalconYoshis);

            Assert.AreEqual(1, testConversions.Count);

            string dummyConstraints2 = "userId: userChar: oppChar: stageId:8";
            List<GameConversions?> testConversions2 = 
                await InterOpHandler.GetAllConversions(dummyConstraints2, "file:\\" + userVars.edgeguardSlpPath + "," + "file:\\" + userVars.meVsPinkSlpPath + "," + "file:\\" + userVars.meVsIcsYoshis + "," + "file:\\" + userVars.oogaVsFalconYoshis);
            Assert.AreEqual(2, testConversions2.Count);
        }
    }
}
