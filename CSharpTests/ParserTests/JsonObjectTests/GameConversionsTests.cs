using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSharpParser;
using Newtonsoft.Json.Linq;
using CSharpParser.JSON_Objects;
using System.Diagnostics;

namespace CSharpTests.ParserTests.JsonObjectTests
{
    [TestClass]
    public class GameConversionsTests
    {
        JObject o1 = JObject.Parse(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardTest.txt"));
        GameConversions testConversions = GameConversions.jsonToConversions(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardTest.txt"));
        [TestMethod]
        public void testProps()
        {
            Assert.IsNotNull(testConversions);

            Assert.AreEqual(testConversions.GameLocation, o1["gameLocation"]);
            Assert.AreEqual(testConversions.ConversionList.Count(), o1["conversionList"].Count());
            Assert.AreEqual(testConversions.GameSettings.Players.First().connectCode, "THIQ#306");
            Assert.AreEqual(testConversions.GameSettings.Players.Last().connectCode, "MMRP#834");
        }
    }
}
