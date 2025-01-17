using CSharpParser.Filters.Settings;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public class ShieldReversal<T> : Reversal<T> where T : ShieldReversalSettings
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
