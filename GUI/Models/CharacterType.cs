using System.ComponentModel;

namespace GUI.Models
{
    public enum CharacterType
    {
        // since melee/slippi stats character IDs start at 0 with falcon, this messes up using a default "Any character" enum with a value of 0
        // as a workaround, character IDs are shifted up by 1 here and then shifted back down in MainViewModel after the user selects a character
        [Description("Any character")]
        None = 0,
        [Description("Captain Falcon")]
        Captain_Falcon = 1,
        [Description("Donkey Kong")]
        Donkey_Kong = 2,
        [Description("Fox")]
        Fox = 3,
        [Description("Mr. Game & Watch")]
        Game_and_Watch = 4,
        [Description("Kirby")]
        Kirby =  5,
        [Description("Bowser")]
        Bowser = 6,
        [Description("Link")]
        Link = 7,
        [Description("Luigi")]
        Luigi = 8,
        [Description("Mario")]
        Mario = 9,
        [Description("Marth")]
        Marth = 10,
        [Description("Mewtwo")]
        Mewtwo = 11,
        [Description("Ness")]
        Ness = 12,
        [Description("Peach")]
        Peach = 13,
        [Description("Pikachu")]
        Pkachu = 14,
        [Description("Ice Climbers")]
        Ice_Climbers = 15,
        [Description("Jigglypuff")]
        Jigglypuff = 16,
        [Description("Samus")]
        Samus = 17,
        [Description("Yoshi")]
        Yoshi = 18,
        [Description("Zelda")]
        Zelda = 19,
        [Description("Sheik")]
        Sheik = 20,
        [Description("Falco")]
        Falco = 21,
        [Description("Young Link")]
        Young_Link = 22,
        [Description("Dr. Mario")]
        Dr_Mario = 23,
        [Description("Roy")]
        Roy = 24,
        [Description("Pichu")]
        Pichu = 25,
        [Description("Ganondorf")]
        Ganondorf = 26
    }
}
