using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpParser.Filters;
using CSharpParser.JSON_Objects;
using Newtonsoft.Json.Linq;

namespace CSharpTests.ParserTests.FilterTests
{
    [TestClass]
    public class EdgeguardTests
    {
        JObject o1 = JObject.Parse(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardTest.txt"));
        GameConversions testConversions = GameConversions.jsonToConversions(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardTest.txt"));
        [TestMethod]
        public void testGetLedgePositions()
        {
            Assert.AreEqual(Edgeguards.getLedgePositions(2), 63.35);
            Assert.AreEqual(Edgeguards.getLedgePositions(3), 87.75);
            Assert.AreEqual(Edgeguards.getLedgePositions(8), 56);
            Assert.AreEqual(Edgeguards.getLedgePositions(28), 77.27);
            Assert.AreEqual(Edgeguards.getLedgePositions(31), 68.4);
            Assert.AreEqual(Edgeguards.getLedgePositions(32), 85.57);
            Assert.AreEqual(Edgeguards.getLedgePositions(null), 0);
            Assert.AreEqual(Edgeguards.getLedgePositions(100), 0);

            Assert.AreEqual(Edgeguards.getLedgePositions(testConversions.GameSettings.StageId), 68.4);
        }

        [TestMethod]
        public void testIsEdgeguard()
        {
            Conversion shouldBeEdgeuard = testConversions.ConversionList.ElementAt(6);
            Conversion notAnEdgeguard = testConversions.ConversionList.ElementAt(2);
            Assert.IsTrue(Edgeguards.isEdgeguard(shouldBeEdgeuard, testConversions.GameSettings.StageId));
            Assert.IsFalse(Edgeguards.isEdgeguard(notAnEdgeguard, testConversions.GameSettings.StageId));
        }

        [TestMethod]
        public void testAddToQueue()
        {
            PlaybackQueue pbackQueue = new PlaybackQueue();
            Edgeguards.addToQueue(testConversions, pbackQueue);
            Assert.AreEqual(pbackQueue.queue.Count(), 12);
        }
    }
}
