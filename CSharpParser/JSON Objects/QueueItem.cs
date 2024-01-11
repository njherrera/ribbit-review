using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.JSON_Objects
{
    public class QueueItem
    {
        public string path { get; set; }
        public int? startFrame { get; set; }
        public int? endFrame { get; set; }
        public string gameStartAt { get; set; }
        public string gameStation { get; set; }

        public QueueItem(string filePath, int? startingFrame, int? endingFrame)
        {
            this.path = filePath;
            this.startFrame = startingFrame;
            this.endFrame = endingFrame;
            this.gameStartAt = "";
            this.gameStation = "";
        }
    }
}
