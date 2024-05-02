using System.Collections.Generic;
using UnityEngine;

namespace types.unit.skill {
// Lists all skills of units in game. Some actions can use skill level of performer to provide bonuses.
// Actions can give skill experience to performer.
public class UnitSkills {
    // colors
    private const float MinR = 235;
    private const float MaxR = 255;
    private const float MinG = 230;
    private const float MaxG = 255;
    private const float MinB = 197;
    private const float MaxB = 95;
    
    // paper 235 230 197
    // yellow 255 225 95
    
    public static readonly Skill MINING = new("mining", "miner");
    public static readonly Skill WOODCUTTING = new("woodcutting", "woodcutter");
    public static readonly Skill FARMING = new("farming", "farmer");
    public static readonly Skill FORAGING = new("foraging", "forager");
    public static readonly Skill FISHING = new("fishing", "fisher");
    public static readonly Skill HUNTING = new("hunting", "hunter");
    public static readonly Skill CARPENTRY = new("carpentry", "carpenter");
    public static readonly Skill MASONRY = new("masonry", "mason");
    public static readonly Skill SMITHING = new("smithing", "smith");
    public static readonly Skill JEWELRY = new("jewelry", "jeweler");
    public static readonly Skill MECHANICS = new("mechanics", "mechanic");
    public static readonly Skill TAILORING = new("tailoring", "tailor");
    public static readonly Skill LEATHERWORKING = new("leatherworking", "leatherworker");
    public static readonly Skill COOKING = new("cooking", "cook");
    public static readonly Skill BREWING = new("brewing", "brewer");
    public static readonly Skill SCHOLARSHIP = new("scholarship", "scholar");
    public static readonly Skill MEDICINE = new("medicine", "doctor");
    public static readonly Skill ALCHEMY = new("alchemy", "alchemist");
    public static readonly Skill TRADING = new("trading", "trader");
    public static readonly Skill PERFORMANCE = new("performance", "performer");
    
    public static readonly Skill MELEE = new("melee", "fighter");
    public static readonly Skill RANGED = new("ranged", "ranger");
    public static readonly Skill MAGIC = new("magic", "mage");
    public static readonly Skill BLOCKING = new("blocking", null);
    public static readonly Skill DODGING = new("dodging", null);

    public const int MAX_VALUE = 15;
    public const int MIN_VALUE = 0;

    public static readonly Skill[] allSkills = {
        MINING, WOODCUTTING, FARMING, FORAGING, FISHING, HUNTING, CARPENTRY, MASONRY,
        SMITHING, JEWELRY, MECHANICS, TAILORING, LEATHERWORKING, COOKING, BREWING, SCHOLARSHIP, MEDICINE, ALCHEMY,
        TRADING, PERFORMANCE, MELEE, RANGED, MAGIC, BLOCKING, DODGING
    };

    public static readonly Dictionary<string, Skill> skills = new ();
    public static readonly string[][] skillPresets = { };

    public static readonly int[] expValues = { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750 };

    static UnitSkills() {
        foreach (Skill skill in allSkills) {
            skills.Add(skill.name, skill);
        }
    }

    public static Color getColor(int skillLevel, Color color) {

        return new Color(
            (MinR + (MaxR - MinR) / MAX_VALUE * skillLevel) / 255,
            (MinG + (MaxG - MinG) / MAX_VALUE * skillLevel) / 255,
            (MinB + (MaxB - MinB) / MAX_VALUE * skillLevel) / 255 
        );
        // color.r = MinR + (MaxR - MinR) / MAX_VALUE * skillLevel;
        // color.g = MinG + (MaxG - MinG) / MAX_VALUE * skillLevel;
        // color.b = MinB + (MaxB - MinB) / MAX_VALUE * skillLevel;
        // return color;
    }
}

public class Skill {
    public readonly string name;
    public readonly string professionName; // displayed in unit menu
    public readonly float speedFactor; // additive workspeed increase for each performer level in this skill
    
    public Skill(string name, string professionName) {
        this.name = name;
        this.professionName = professionName;
        speedFactor = 0.04f;
    }
}
}