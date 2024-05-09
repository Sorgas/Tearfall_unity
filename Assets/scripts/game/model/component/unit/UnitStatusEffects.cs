using util.lang;

namespace game.model.component.unit {
// lists all possible status effects in game
public class UnitStatusEffects : Singleton<UnitStatusEffects> {
    public static readonly UnitStatusEffect sleepy = new ("sleepy", -2);
    public static readonly UnitStatusEffect tired = new ("tired", -5);
    public static readonly UnitStatusEffect exhausted = new ("exhausted", -10);
    public static readonly UnitStatusEffect peckish = new ("peckish", -2);
    public static readonly UnitStatusEffect hungry = new ("hungry", -5);
    public static readonly UnitStatusEffect starving = new ("starving", -10);

    public UnitStatusEffects() {
        tired.multiplicativeChanges["movespeed"] = 0.9f;
        tired.multiplicativeChanges["workspeed"] = 0.9f;
        exhausted.multiplicativeChanges["movespeed"] = 0.5f;
        exhausted.multiplicativeChanges["workspeed"] = 0.5f;
        hungry.multiplicativeChanges["frostresist"] = 0.9f;
        hungry.multiplicativeChanges["movespeed"] = 0.9f;
        hungry.multiplicativeChanges["carryweight"] = 0.75f;
        starving.multiplicativeChanges["frostresist"] = 0.5f;
        starving.multiplicativeChanges["damagemodifier"] = 0.5f;
        starving.multiplicativeChanges["carryweight"] = 0.3f;
    }
}
}