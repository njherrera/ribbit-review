namespace CSharpParser.JSON_Objects
{
    public class PlaybackQueue
    {
        public string mode { get; set; }
        public string  replay { get; set; } // field needs to be present for minimum viable JSON, but doesn't need to be filled out
        public List<QueueItem> queue { get; set; }

        public PlaybackQueue()
        {
            mode = "queue";
            replay = "";
            queue = new List<QueueItem>();
            
        }
    }
}
