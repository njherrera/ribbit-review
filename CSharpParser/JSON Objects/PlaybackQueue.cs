using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpParser.JSON_Objects
{
    public class PlaybackQueue
    {
        public string mode { get; set; }
        public string  replay { get; set; }
        public List<QueueItem> queue { get; set; }

        public PlaybackQueue()
        {
            mode = "queue";
            replay = "";
            queue = new List<QueueItem>();
            
        }
    }
}
