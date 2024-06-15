using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters.Settings
{
    public class EdgeguardSettings : FilterSettings
    {
        // this checks for whichever properties can be selected on the edgeguard settings view, which is currently just converting player and whether it killed
        public override bool checkConversion(Conversion conversion)
        {
            // by default going to return true, since if every setting is null then user wants to see every instance of situation
            bool passesCheck = true;

            // check for edgeguarding player
            if (this.convertingPlayer != null && this.userID != null &&  this.convertingPlayer.Equals("user"))
            {
                string userIDCaps = this.userID.ToUpper(); // calling ToUpper in case the user input their connect code with lowercase
                bool meetsCondition = userIDCaps.Equals(conversion.hittingConnectCode);
                if (meetsCondition == false) { return false; }
            } 
            else if (this.convertingPlayer != null && this.userID != null && this.convertingPlayer.Equals("opponent"))
            {
                string userIDCaps = this.userID.ToUpper();
                bool meetsCondition = userIDCaps.Equals(conversion.beingHitConnectCode);
                if (meetsCondition == false ) { return false; }
            }

            // check for whether conversion killed
            if (this.conversionKilled != null)
            {
                bool meetsCondition = this.conversionKilled.Equals(conversion.didKill);
                if (meetsCondition == false ) { return false; }
            }
            return passesCheck;
        }
    }
}
