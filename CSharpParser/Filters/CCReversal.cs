using CSharpParser.Filters.Settings;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public class CCReversal<T> : Reversal<T> where T : CCReversalSettings
    {
        protected override bool CheckSettings(Conversion conversion, T fSettings, List<Player> gamePlayers)
        {
            throw new NotImplementedException();
        }

        protected override bool IsInstance(Conversion conversion, GameSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
