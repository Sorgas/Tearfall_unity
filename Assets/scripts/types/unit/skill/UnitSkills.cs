using System.Collections.Generic;

namespace types.unit.skill {
public class UnitSkills {
    public const string MINING = "mining";
    public const string WOODCUTTING = "woodcutting";
    public const string FARMING = "farming";
    public const string FORAGING = "foraging";
    public const string FISHING = "fishing";
        
    public const string CARPENTRY = "carpentry"; // crafting wood items and building from wood
    public const string MASONRY = "masonry"; // crafting stone items and building from stone
    public const string SMITHING = "smithing";
    public const string TAILORING = "tailoring";
    public const string COOKING = "cooking";

    public const string SCHOLARSHIP = "scholarship";
    public const string MEDICINE = "medicine";
    public const string ALCHEMY = "alchemy";
    public const string TRADING = "trading";
    public const string PERFORMANCE = "performance";

    public const string MELEE = "melee"; 
    public const string RANGED = "ranged";
    public const string MAGIC = "magic"; 
    public const string BLOCKING = "blocking";
    public const string DODGING = "dodging";

    public const int MAX_VALUE = 15;
    public const int MIN_VALUE = 0;
    
    public static readonly string[] allSkills = {
        MINING, WOODCUTTING, FARMING, FORAGING, FISHING,
        CARPENTRY, MASONRY, SMITHING, TAILORING, COOKING,
        SCHOLARSHIP, MEDICINE, ALCHEMY, TRADING, PERFORMANCE,
        MELEE, RANGED, MAGIC, BLOCKING, DODGING
    };
    
    public static readonly string[][] skillPresets = { };

    public static readonly int[] expValues = { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750 };
}
}