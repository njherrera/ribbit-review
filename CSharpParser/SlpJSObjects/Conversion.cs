namespace CSharpParser.SlpJSObjects
{
    public class Conversion
    {

        /* one Conversion holds all the post-update frames for both players involved in the conversion, where beingHitFrames = player being hit and hittingFrames = player hitting 
         * at the moment this CANNOT handle doubles conversions, since slippi stats itself isn't functional for doubles replays
        */
        public int playerBeingHit { get; set; }
        public string beingHitConnectCode { get; set; }
        public int playerHitting { get; set; }
        public string hittingConnectCode { get; set; }
        public bool didKill { get; set; }
        public float startPercent { get; set; }
        public float? endPercent { get; set; }
        public List<Move> moves { get; set; }
        public string openingType { get; set; }
        public List<PostFrame> beingHitFrames { get; set; }
        public List<PostFrame> hittingFrames { get; set; }
    }
}
