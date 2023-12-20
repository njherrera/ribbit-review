using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpParser.Filters;
using CSharpParser.JSON_Objects;
using Newtonsoft.Json;
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

        [TestMethod]
        public void testPlayingQueue()
        {
            string cmdText;
            PlaybackQueue pbackQueue = new PlaybackQueue();
            Edgeguards.addToQueue(testConversions, pbackQueue);
            string edgeguardJson = JsonConvert.SerializeObject(pbackQueue);
            cmdText = "/C C:\\Users\\mucho\\AppData\\Roaming\\Slippi Launcher\\playback\\Slippi Dolphin -i" + edgeguardJson;

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            // cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.Arguments = cmdText;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.Start();
            
            cmd.WaitForExit();
            Debug.WriteLine(cmd.StandardOutput.ToString());
            string cmdOutput = cmd.StandardOutput.ReadToEnd();
            Assert.IsFalse(cmdOutput.Contains("Error message text"));
        }
    }
}
