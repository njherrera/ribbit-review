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
            InterOpHandler.configureNodeService(userVars.interOpPath);
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
    }
}
