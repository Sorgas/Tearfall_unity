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
    public static readonly UnitProperty BLEEDRESIST = new("bleedresist", 1f);
    public static readonly UnitProperty DAMAGEMODIFIER = new("damagemodifier", 1f);
    public static readonly UnitProperty ACCURACY = new("accuracy", 1f);
    public static readonly UnitProperty CASTSPEED = new("castspeed", 1f);
    public static readonly UnitProperty SPELLSLOTS = new("spellslots", 1f);
    public static readonly UnitProperty MANA = new("mana", 1f);
    public static readonly UnitProperty DODGECHANCE = new("dodgechance", 1f);
    public static readonly UnitProperty LEARNINGSPEED = new("learningspeed", 1f);
    public static readonly UnitProperty CRITCHANCE = new("critchance", 1f);
    public static readonly UnitProperty MAGICRESIST = new("magicresist", 1f);
    public static readonly UnitProperty PAIN = new("pain", 1f);
    public static readonly UnitProperty BLOOD = new("blood", 1f);
    public static readonly UnitProperty HUNGERRATE = new("hunger rate", 1f);
    public static readonly UnitProperty THIRSTRATE = new("thirst rate", 1f);
    public static readonly UnitProperty FATIGUERATE = new("fatigue rate", 1f);
    public static readonly UnitProperty CONSCIOUSNESS = new("consciousness", 1f);
    public static readonly UnitProperty BREATHING = new("breathing", 1f);
    public static readonly UnitProperty EATING = new("eating", 1f);
    public static readonly UnitProperty DRINKING = new("drinking", 1f);
    public static readonly UnitProperty SIGHT = new("sight", 1f);

    public static readonly UnitProperty[] ALL = {
        MOOD, MOVESPEED, CARRYWEIGHT, WORKSPEED, STUNRESIST, PAINRESIST, HEATRESIST, FROSTRESIST, LIGHTNINGRESIST,
        POISONRESIST, DISEASERESIST, BLEEDRESIST, DAMAGEMODIFIER, ACCURACY, CASTSPEED, SPELLSLOTS, MANA, DODGECHANCE,
        LEARNINGSPEED, CRITCHANCE, MAGICRESIST, PAIN, BLOOD, HUNGERRATE, THIRSTRATE, FATIGUERATE, CONSCIOUSNESS, BREATHING, 
        EATING, DRINKING, SIGHT,
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