namespace CSharpParser.SlpJSObjects
{
    public class Conversion
    {

        /* one Conversion holds all the post-update frames for both players involved in the conversion, where beingHitFrames = player being hit and hittingFrames = player hitting 
         * at the moment this CANNOT handle doubles conversions, since slippi stats itself isn't functional for doubles replays
        */
        public int victimIndex { get; set; }
        public string victimConnectCode { get; set; }
        public string victimNametag { get; set; }
        public int attackerIndex { get; set; }
        public string attackerConnectCode { get; set; }
        public string attackerNametag { get; set; }
        public bool didKill { get; set; }
        public float startPercent { get; set; }
        public float? endPercent { get; set; }
        public List<Move> moves { get; set; }
        public string openingType { get; set; }
        // TODO: change List<PostFrame> to Dictionary<int, PostFrame> or similar HashSet?
        // might result in better performance from being able to use LINQ to query dictionaries, with int being frame in context of conversion (e.g. 1 = first frame of conversion, 2 = second frame, etc.)
        // need to look into what performance gains we get (if any) from using LINQ to query dictionaries as opposed to looping through list, also any changes to memory usage
        public List<PostFrame> victimFrames { get; set; }
        public List<PostFrame> attackerFrames { get; set; }
    }
}
