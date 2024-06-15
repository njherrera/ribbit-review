using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CSharpParser.Filters;
using CSharpParser.Filters.Settings;
using CSharpParser.JSON_Objects;
using CSharpParser.SlpJSObjects;


namespace CSharpTests.ParserTests.FilterTests
{
    [TestClass]
    public class EdgeguardTests
    {
        GameConversions testConversions = JsonSerializer.Deserialize<GameConversions>(File.ReadAllText(@"Q:\\programming\\ribbit-review\\testJSONs\\EdgeguardTest.txt"));
        Edgeguards Edgeguards = new Edgeguards();

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

            Assert.AreEqual(Edgeguards.getLedgePositions(testConversions.gameSettings.stageId), 68.4);
        }

        [TestMethod]
        public void testIsEdgeguard()
        {
            Conversion shouldBeEdgeuard = testConversions.conversionList.ElementAt(6);
            Conversion notAnEdgeguard = testConversions.conversionList.ElementAt(2);
            Assert.IsTrue(Edgeguards.isInstance(shouldBeEdgeuard, testConversions.gameSettings));
            Assert.IsFalse(Edgeguards.isInstance(notAnEdgeguard, testConversions.gameSettings));
        }

        [TestMethod]
        public void testAddToQueue()
        {
            EdgeguardSettings eSettings = new EdgeguardSettings();
            PlaybackQueue pbackQueue = new PlaybackQueue();
            Edgeguards.addToQueue(testConversions, pbackQueue, eSettings);
            Assert.AreEqual(pbackQueue.queue.Count(), 12);
        }

        [TestMethod]
        public void testPlayingQueue()
        {
            string cmdText;
            PlaybackQueue pbackQueue = new PlaybackQueue();
            EdgeguardSettings eSettings = new EdgeguardSettings();
            Edgeguards.addToQueue(testConversions, pbackQueue, eSettings);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string edgeguardJson = JsonSerializer.Serialize(pbackQueue, options);
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
