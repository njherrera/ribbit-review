using CSharpParser.Filters;
using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        GameConversions testConversions = JsonSerializer.Deserialize<GameConversions>(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardSettingsTest.json"));
        Edgeguards Edgeguards = new Edgeguards();

        [TestMethod]
        public void testConvertingPlayer()
        {
            EdgeguardSettingsBuilder sheikBuilder = new EdgeguardSettingsBuilder();
            sheikBuilder.addUserID("mmrp#834");
            sheikBuilder.addConvertingPlayer("user");
            EdgeguardSettings sheikConverting = (EdgeguardSettings)sheikBuilder.Build();
            
            PlaybackQueue pbackQueueSheik = new PlaybackQueue();
            Edgeguards.addToQueue(testConversions, pbackQueueSheik, sheikConverting);

            EdgeguardSettingsBuilder falcoBuilder = new EdgeguardSettingsBuilder();
            falcoBuilder.addUserID("mmrp#834");
            falcoBuilder.addConvertingPlayer("opponent");
            EdgeguardSettings falcoConverting = (EdgeguardSettings)falcoBuilder.Build();

            PlaybackQueue pbackQueueFalco = new PlaybackQueue();
            Edgeguards.addToQueue(testConversions, pbackQueueFalco, falcoConverting);

            Assert.AreEqual(7, pbackQueueSheik.queue.Count());
            Assert.AreEqual(5, pbackQueueFalco.queue.Count());
        }

        [TestMethod]
        public void testConversionKilled()
        {
            EdgeguardSettingsBuilder sheikBuilder = new EdgeguardSettingsBuilder();
            sheikBuilder.addUserID("MMRP#834");
            sheikBuilder.addConvertingPlayer("user");
            sheikBuilder.addConversionKilled(true);
            EdgeguardSettings sheikKilled = (EdgeguardSettings)sheikBuilder.Build();

            PlaybackQueue sheikQueue = new PlaybackQueue();
            Edgeguards.addToQueue(testConversions, sheikQueue, sheikKilled);

            EdgeguardSettingsBuilder falcoBuilder = new EdgeguardSettingsBuilder();
            falcoBuilder.addUserID("mmrp#834");
            falcoBuilder.addConvertingPlayer("opponent");
            falcoBuilder.addConversionKilled(true);
            EdgeguardSettings falcoConverting = (EdgeguardSettings)falcoBuilder.Build();

            PlaybackQueue pbackQueueFalco = new PlaybackQueue();
            Edgeguards.addToQueue(testConversions, pbackQueueFalco, falcoConverting);

            Assert.AreEqual(3, sheikQueue.queue.Count());
            Assert.AreEqual(1, pbackQueueFalco.queue.Count());
        }
    }
}
