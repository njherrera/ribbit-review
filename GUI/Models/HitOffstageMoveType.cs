using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models
{
    public enum HitOffstageMoveType
    {
        [Description("No selection")]
        None = 0,
        [Description("Any move")]
        Any = 1,
        [Description("Jab 1")]
        Jab1 = 2,
        [Description("Jab 2")]
        Jab2 = 3,
        [Description("Jab 3")]
        Jab3 = 4,
        [Description("Rapid Jabs")]
        RapidJab = 5,
        [Description("Dash Attack")]
        DashAttack = 6,
        [Description("Forward Tilt")]
        FTilt = 7,
        [Description("Up Tilt")]
        UTilt = 8,
        [Description("Down Tilt")]
        DTilt = 9,
        [Description("Forward Smash")]
        FSmash = 10,
        [Description("Up Smash")]
        USmash = 11,
        [Description("Down Smash")]
        DSmash = 12,
        [Description("Neutral Air")]
        NAir = 13,
        [Description("Forward Air")]
        FAir = 14,
        [Description("Back Air")]
        BAir = 15,
        [Description("Up Air")]
        UAir = 16,
        [Description("Down Air")]
        DAir = 17,
        [Description("Neutral B")]
        NeutralB = 18,
        [Description("Side B")]
        SideB = 19,
        [Description("Up B")]
        UpB = 20,
        [Description("Down B")]
        DownB = 21,
        [Description("Getup Attack")]
        GetupAttack = 50,
        [Description("Getup Attack (Slow)")]
        SlowGetUpAttack = 51,
        [Description("Getup Attack From Ledge (Slow)")]
        LedgeGetUpAttackSlow = 61,
        [Description("Getup Attack From Ledge")]
        LedgeGetUpAttack = 62,
    }
}
