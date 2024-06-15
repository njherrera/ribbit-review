using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpParser.SlpJSObjects;
using Newtonsoft.Json.Linq;
using CSharpParser.JSON_Objects;
using System.Diagnostics;
using System.Text.Json;

namespace CSharpTests.ParserTests.JsonObjectTests
{
    [TestClass]
    public class GameConversionsTests
    {
        JObject o1 = JObject.Parse(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardTest.txt"));
        GameConversions testConversions = JsonSerializer.Deserialize<GameConversions>(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardTest.txt"));
        [TestMethod]
        public void testProps()
        {
            Assert.IsNotNull(testConversions);

            Assert.AreEqual(testConversions.gameLocation, o1["gameLocation"]);
            Assert.AreEqual(testConversions.conversionList.Count(), o1["conversionList"].Count());
            Assert.AreEqual(testConversions.gameSettings.players.First().connectCode, "THIQ#306");
            Assert.AreEqual(testConversions.gameSettings.players.Last().connectCode, "MMRP#834");
        }
    }
}
