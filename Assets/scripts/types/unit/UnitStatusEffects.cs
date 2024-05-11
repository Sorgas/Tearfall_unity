using game.model.component.unit;
using util.lang;

namespace types.unit {
// Lists all possible status effects in game. Initializes 
// Effect instances are reused for all units.  
public class UnitStatusEffects : Singleton<UnitStatusEffects> {
    public static readonly UnitStatusEffect sleepy = new ("sleepy", -2);
    public static readonly UnitStatusEffect tired = new ("tired", -5);
    public static readonly UnitStatusEffect exhausted = new ("exhausted", -10);
    public static readonly UnitStatusEffect peckish = new ("peckish", -2);
    public static readonly UnitStatusEffect hungry = new ("hungry", -5);
    public static readonly UnitStatusEffect starving = new ("starving", -10);

    public UnitStatusEffects() {
        tired.offsets["movespeed"] = -0.1f;
        // multipliers["movespeed"] = 0.9f;
        tired. multipliers["workspeed"] = 0.9f;
        exhausted.multipliers["movespeed"] = 0.5f;
        exhausted.multipliers["workspeed"] = 0.5f;
        hungry.multipliers["frostresist"] = 0.9f;
        hungry.multipliers["movespeed"] = 0.9f;
        hungry.multipliers["carryweight"] = 0.75f;
        starving.multipliers["frostresist"] = 0.5f;
        starving.multipliers["damagemodifier"] = 0.5f;
        starving.multipliers["carryweight"] = 0.3f;
    }
}
}