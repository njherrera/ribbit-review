using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Helpers;

namespace GUI.Models
{
    public enum CharacterType
    {
        [Description("Any character")]
        None,
        [Description("Captain Falcon")]
        Captain_Falcon = 0,
        [Description("Donkey Kong")]
        Donkey_Kong = 1,
        [Description("Fox")]
        Fox = 2,
        [Description("Mr. Game & Watch")]
        Game_and_Watch = 3,
        [Description("Kirby")]
        Kirby =  4,
        [Description("Bowser")]
        Bowser = 5,
        [Description("Link")]
        Link = 6,
        [Description("Luigi")]
        Luigi = 7,
        [Description("Mario")]
        Mario = 8,
        [Description("Marth")]
        Marth = 9,
        [Description("Mewtwo")]
        Mewtwo = 10,
        [Description("Ness")]
        Ness = 11,
        [Description("Peach")]
        Peach = 12,
        [Description("Pikachu")]
        Pkachu = 13,
        [Description("Ice Climbers")]
        Ice_Climbers = 14,
        [Description("Jigglypuff")]
        Jigglypuff = 15,
        [Description("Samus")]
        Samus = 16,
        [Description("Yoshi")]
        Yoshi = 17,
        [Description("Zelda")]
        Zelda = 18,
        [Description("Sheik")]
        Sheik = 19,
        [Description("Falco")]
        Falco = 20,
        [Description("Young Link")]
        Young_Link = 21,
        [Description("Dr. Mario")]
        Dr_Mario = 22,
        [Description("Roy")]
        Roy = 23,
        [Description("Pichu")]
        Pichu = 24,
        [Description("Ganondorf")]
        Ganondorf = 25
    }
}
