﻿namespace CSharpParser.JSON_Objects
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
            string encodedPath = filePath;
            string decodedPath = Uri.UnescapeDataString(encodedPath);
            this.path = decodedPath.Replace(@"file:///", "");  // cuts out "file:///" from the file's path
            this.startFrame = startingFrame;
            this.endFrame = endingFrame;
            this.gameStartAt = "";
            this.gameStation = "";
        }
    }
}
