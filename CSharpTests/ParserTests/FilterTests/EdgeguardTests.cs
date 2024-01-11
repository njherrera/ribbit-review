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
            Edgeguards edgeguards = new Edgeguards();
            Conversion shouldBeEdgeuard = testConversions.ConversionList.ElementAt(6);
            Conversion notAnEdgeguard = testConversions.ConversionList.ElementAt(2);
            Assert.IsTrue(edgeguards.isInstance(shouldBeEdgeuard, testConversions.GameSettings));
            Assert.IsFalse(edgeguards.isInstance(notAnEdgeguard, testConversions.GameSettings));
        }

        [TestMethod]
        public void testAddToQueue()
        {
            Edgeguards edgeguards = new Edgeguards();
            PlaybackQueue pbackQueue = new PlaybackQueue();
            edgeguards.addToQueue(testConversions, pbackQueue);
            Assert.AreEqual(pbackQueue.queue.Count(), 12);
        }

        [TestMethod]
        public void testPlayingQueue()
        {
            Edgeguards edgeguards = new Edgeguards();
            string cmdText;
            PlaybackQueue pbackQueue = new PlaybackQueue();
            edgeguards.addToQueue(testConversions, pbackQueue);
            string edgeguardJson = JsonConvert.SerializeObject(pbackQueue, Formatting.Indented);
            string jsonPath = @"Q:\programming\ribbit-review\testJSONs\EdgeguardsJSON.json";
            File.WriteAllText(jsonPath, edgeguardJson);
            cmdText = "/C " + userVars.dolphinPath + " -i " + jsonPath + " -e " + userVars.meleePath;

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            // cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.Arguments = cmdText;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.OutputDataReceived += (sender, args) => Debug.WriteLine("received output: {0}", args.Data);
            cmd.Start();

            cmd.WaitForExit();
            string output = cmd.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
        }
    }
}
