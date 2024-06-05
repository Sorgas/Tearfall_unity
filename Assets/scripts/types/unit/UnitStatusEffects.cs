using System.Collections.Generic;
using game.model.component.unit;
using MoreLinq;

namespace types.unit {
// Lists all possible status effects in game. Initializes 
// Effect instances are reused for all units.  
public class UnitStatusEffects {
    public static readonly UnitStatusEffect SLEEPY = new("sleepy", -2);
    public static readonly UnitStatusEffect TIRED = new("tired", -5);
    public static readonly UnitStatusEffect EXHAUSTED = new("exhausted", -10);
    
    public static readonly UnitStatusEffect PECKISH = new("peckish", -2);
    public static readonly UnitStatusEffect HUNGRY = new("hungry", -5);
    public static readonly UnitStatusEffect RAVENOUSLY_HUNGRY = new("ravenouslyHungry", -10);
    
    public static readonly UnitStatusEffect MODERATE_STARVATION = new("moderateStarvation", -10);
    public static readonly UnitStatusEffect STARVATION = new("starvation", -15);
    public static readonly UnitStatusEffect EXTREME_STARVATION = new("extremeStarvation", -20);
    public static readonly Dictionary<string, UnitStatusEffect> effects = new();
    private static readonly UnitStatusEffect[] all = {
        SLEEPY, TIRED, EXHAUSTED, PECKISH, HUNGRY, RAVENOUSLY_HUNGRY, MODERATE_STARVATION, STARVATION, EXTREME_STARVATION
    };

    static UnitStatusEffects() {
        all.ForEach(effect => effects.Add(effect.name, effect));
        TIRED.offsets["movespeed"] = -0.1f;
        // multipliers["movespeed"] = 0.9f;
        TIRED.multipliers["workspeed"] = 0.9f;
        EXHAUSTED.multipliers["movespeed"] = 0.5f;
        EXHAUSTED.multipliers["workspeed"] = 0.5f;
        HUNGRY.multipliers["frostresist"] = 0.9f;
        HUNGRY.multipliers["movespeed"] = 0.9f;
        HUNGRY.multipliers["carryweight"] = 0.75f;
        RAVENOUSLY_HUNGRY.multipliers["frostresist"] = 0.5f;
        RAVENOUSLY_HUNGRY.multipliers["damagemodifier"] = 0.5f;
        RAVENOUSLY_HUNGRY.multipliers["carryweight"] = 0.3f;

        MODERATE_STARVATION.bonuses["strength"] = -1;
        MODERATE_STARVATION.bonuses["agility"] = -1;
        MODERATE_STARVATION.bonuses["endurance"] = -1;
        STARVATION.bonuses["strength"] = -2;
        STARVATION.bonuses["agility"] = -2;
        STARVATION.bonuses["endurance"] = -2;
        EXTREME_STARVATION.bonuses["strength"] = -4;
        EXTREME_STARVATION.bonuses["agility"] = -4;
        EXTREME_STARVATION.bonuses["endurance"] = -4;
    }
}
}