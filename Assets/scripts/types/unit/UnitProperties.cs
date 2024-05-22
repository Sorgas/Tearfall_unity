namespace types.unit {
// Describes all unit properties in game. Properties affect most unit activities in game.
// Some base values can be defined by race. Status effects can change values (See UnitStatusEffectUtility).
public class UnitProperties {
    public static readonly UnitProperty MOOD = new("mood", 0f);
    public static readonly UnitProperty MOVESPEED = new("movespeed ", 1f); // tiles per real time second
    public static readonly UnitProperty CARRYWEIGHT = new("carryweight", 1f);
    public static readonly UnitProperty WORKSPEED = new("workspeed", 1f); // multiplier
    public static readonly UnitProperty STUNRESIST = new("stunresist", 1f);
    public static readonly UnitProperty PAINRESIST = new("painresist", 1f);
    public static readonly UnitProperty HEATRESIST = new("heatresist", 1f);
    public static readonly UnitProperty FROSTRESIST = new("frostresist", 1f);
    public static readonly UnitProperty LIGHTNINGRESIST = new("lightningresist", 1f);
    public static readonly UnitProperty POISONRESIST = new("poisonresist", 1f);
    public static readonly UnitProperty DISEASERESIST = new("diseaseresist", 1f);
    public static readonly UnitProperty DAMAGEMODIFIER = new("damagemodifier", 1f);
    public static readonly UnitProperty ACCURACY = new("accuracy", 1f);
    public static readonly UnitProperty CASTSPEED = new("castspeed", 1f);
    public static readonly UnitProperty SPELLSLOTS = new("spellslots", 1f);
    public static readonly UnitProperty MANA = new("mana", 1f);
    public static readonly UnitProperty DODGECHANCE = new("dodgechance", 1f);
    public static readonly UnitProperty LEARNINGSPEED = new("learningspeed", 1f);
    public static readonly UnitProperty CRITCHANCE = new("critchance", 1f);
    public static readonly UnitProperty MAGICRESIST = new("magicresist", 1f);

    public static readonly UnitProperty[] all = {
        MOOD, MOVESPEED, CARRYWEIGHT, WORKSPEED, STUNRESIST, PAINRESIST, HEATRESIST, FROSTRESIST, LIGHTNINGRESIST,
        POISONRESIST, DISEASERESIST, DAMAGEMODIFIER, ACCURACY, CASTSPEED, SPELLSLOTS, MANA, DODGECHANCE,
        LEARNINGSPEED, CRITCHANCE, MAGICRESIST,
    };
}

public class UnitProperty {
    public readonly string name;
    public readonly float baseValue;

    public UnitProperty(string name, float baseValue) {
        this.name = name;
        this.baseValue = baseValue;
    }
}
}