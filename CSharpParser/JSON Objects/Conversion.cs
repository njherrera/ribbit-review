using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpParser.SlpJSObjects;
using Newtonsoft.Json;

namespace CSharpParser.JSON_Objects
{
    public class Conversion
    {
  
        /* one Conversion holds all the post-update frames for both players involved in the conversion, where beingHitFrames = player being hit and hittingFrames = player hitting 
         * at the moment this CANNOT handle doubles conversions of only two lists, also because slippi stats itself isn't fully functional for doubles replays
        */ 
        public List<PostFrame> beingHitFrames { get; set; }
        public List<PostFrame> hittingFrames { get; set; }
    }
}
