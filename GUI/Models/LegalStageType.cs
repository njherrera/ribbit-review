using GUI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models
{
    public enum LegalStageType
    {
        [Description("Any Stage")]
        None,
        [Description("Fountain of Dreams")]
        Fountain_of_Dreams = 2,
        [Description("Pokemon Stadium")]
        Pokemon_Stadium = 3,
        [Description("Yoshi's Story")]
        Yoshis_Story = 8,
        [Description("Dream Land")]
        Dreamland = 18,
        [Description("Battlefield")]
        Battlefield = 31,
        [Description("Final Destination")]
        Final_Destination = 32
    }
}
