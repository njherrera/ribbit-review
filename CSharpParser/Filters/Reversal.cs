using CSharpParser.Filters.Settings;
using CSharpParser.SlpJSObjects;

namespace CSharpParser.Filters
{
    public abstract class Reversal<T> : Filter<T> where T : ReversalSettings
    {
        private (PostFrame atkFrame, PostFrame vicFrame) reversalFrames;
        private Move reversalMove;
        public override void InitializeStageVars(GameSettings settings)
        {
            throw new NotImplementedException();
        }

        protected abstract override bool IsInstance(Conversion conversion, GameSettings settings);

        protected abstract override bool CheckSettings(Conversion conversion, T fSettings, List<Player> gamePlayers);
    }
}
