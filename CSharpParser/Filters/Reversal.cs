using CSharpParser.Filters.Settings;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public class Reversal<T> : Filter<T> where T : ReversalSettings
    {
        public override void InitializeStageVars(GameSettings settings)
        {
            throw new NotImplementedException();
        }

        public override bool IsInstance(Conversion conversion, GameSettings settings)
        {
            throw new NotImplementedException();
        }

        protected override bool CheckSettings(Conversion conversion, T fSettings, List<Player> gamePlayers)
        {
            throw new NotImplementedException();
        }
    }
}
