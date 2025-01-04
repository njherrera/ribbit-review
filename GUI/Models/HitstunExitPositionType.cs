using System.ComponentModel;

namespace GUI.Models
{
    public enum HitstunExitPositionType
    {
        [Description("Either")]
        None = 0,
        [Description("Above Ledge")]
        Above = 1,
        [Description("Below Ledge")]
        Below = 2
    }
}
