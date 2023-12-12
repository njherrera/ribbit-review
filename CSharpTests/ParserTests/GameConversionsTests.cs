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

namespace CSharpTests.ParserTests
{
    [TestClass]
    public class GameConversionsTests
    {
        [TestMethod]
        public void testProps()
        {
            JObject o1 = JObject.Parse(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardTest.txt"));
            GameConversions conversions = GameConversions.jsonToConversions(o1.ToString());

            Assert.IsNotNull(conversions);

            Assert.AreEqual(conversions.GameLocation, o1["gameLocation"]);
            Assert.AreEqual(conversions.ConversionList.Count(), o1["conversionList"].Count());
        }
    }
}
