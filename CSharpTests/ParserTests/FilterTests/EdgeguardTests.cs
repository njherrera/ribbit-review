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
            Assert.IsTrue(Edgeguards.isInstance(shouldBeEdgeuard, testConversions.GameSettings));
            Assert.IsFalse(Edgeguards.isInstance(notAnEdgeguard, testConversions.GameSettings));
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
            string edgeguardJson = JsonConvert.SerializeObject(pbackQueue, Formatting.Indented);
            string jsonPath = @"Q:\programming\ribbit-review\testJSONs\EdgeguardsJSON.json";
            File.WriteAllText(jsonPath, edgeguardJson);
            cmdText ="-i " + jsonPath + " -e " + userVars.meleePath;

            Process cmd = new Process();
            cmd.StartInfo.FileName = userVars.dolphinPath;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            //cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Arguments = cmdText;

            cmd.ErrorDataReceived += (object sender, DataReceivedEventArgs args) => Debug.Write("error received: " + args.Data);
            cmd.OutputDataReceived += (object sender, DataReceivedEventArgs args) => Debug.WriteLine("received output: {0}", args.Data);
            cmd.EnableRaisingEvents = true;

            cmd.Start();
            cmd.BeginOutputReadLine();
            cmd.BeginErrorReadLine();

            //cmd.StandardInput.WriteLine(cmdText);

            cmd.WaitForExit();
            Assert.IsTrue(cmd.ExitCode == 0);
        }
    }
}
