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
        GameConversions testConversions = JsonSerializer.Deserialize<GameConversions>(File.ReadAllText(userVars.edgeguardConversionsTxt));
        Edgeguards<EdgeguardSettings> edgeguardFilter = new Edgeguards<EdgeguardSettings>(); 

        [TestMethod]
        public void testIsEdgeguard()
        {
            Conversion shouldBeEdgeuard = testConversions.conversionList.ElementAt(6);
            Conversion notAnEdgeguard = testConversions.conversionList.ElementAt(2);
            Assert.IsTrue(edgeguardFilter.IsInstance(shouldBeEdgeuard, testConversions.gameSettings));
            Assert.IsFalse(edgeguardFilter.IsInstance(notAnEdgeguard, testConversions.gameSettings));
        }

        [TestMethod]
        public void testAddToQueue()
        {
            EdgeguardSettings eSettings = new EdgeguardSettings();
            List<GameConversions> conversionList = new List<GameConversions>();
            conversionList.Add(testConversions);
            PlaybackQueue pbackQueue = edgeguardFilter.AddToQueue(conversionList, eSettings);
            Assert.AreEqual(pbackQueue.queue.Count(), 12);
        }

       /* [TestMethod]
        public void testPlayingQueue()
        {
            string cmdText;
            EdgeguardSettings eSettings = new EdgeguardSettings();
            PlaybackQueue pbackQueue = Edgeguards.addToQueue(testConversions, eSettings);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string edgeguardJson = JsonSerializer.Serialize(pbackQueue, options);
            string jsonPath = userVars.edgeguardJsonPath;
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
        }*/
    }
}
