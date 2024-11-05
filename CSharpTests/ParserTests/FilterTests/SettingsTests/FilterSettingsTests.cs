using CSharpParser.Filters;
using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using Newtonsoft.Json.Linq;
using SlippiJSInterOp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GUI.ViewModels;
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
        GameConversions edgeguardConversions = JsonSerializer.Deserialize<GameConversions>(File.ReadAllText(userVars.edgeguardConversionsTxt));
        List<GameConversions> testConversions = new List<GameConversions>();


        [TestMethod]
        public async Task testConvertingPlayer()
        {
            testConversions.Add(edgeguardConversions);
            EdgeguardSettingsBuilder sheikBuilder = new EdgeguardSettingsBuilder();
            sheikBuilder.addUserID("mmrp#834");
            sheikBuilder.addConvertingPlayer("user");
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
            testConversions.Add(edgeguardConversions);
            EdgeguardSettingsBuilder sheikBuilder = new EdgeguardSettingsBuilder();
            sheikBuilder.addUserID("MMRP#834");
            sheikBuilder.addConvertingPlayer("user");
            sheikBuilder.addConversionKilled(true);
            EdgeguardSettings sheikKilled = (EdgeguardSettings)sheikBuilder.Build();

            PlaybackQueue sheikQueue = edgeguardFilter.AddToQueue(testConversions, sheikKilled);

            EdgeguardSettingsBuilder falcoBuilder = new EdgeguardSettingsBuilder();
            falcoBuilder.addUserID("mmrp#834");
            falcoBuilder.addConvertingPlayer("opponent");
            falcoBuilder.addConversionKilled(true);
            EdgeguardSettings falcoConverting = (EdgeguardSettings)falcoBuilder.Build();

            PlaybackQueue falcoQueue = edgeguardFilter.AddToQueue(testConversions, falcoConverting);

            Assert.AreEqual(3, sheikQueue.queue.Count());
            Assert.AreEqual(1, falcoQueue.queue.Count());
        }
    }
}
