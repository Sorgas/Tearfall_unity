using System.Collections.Generic;

namespace game.model.component.unit {

// status effect for unit like hunger or potion effect. Can change 
public class UnitStatusEffect {
    public readonly string name;
    public readonly string icon;
    public readonly bool displayed; // some effects are hidden
    public readonly Dictionary<string, float> offsets = new(); // property -> delta
    public readonly Dictionary<string, float> multipliers = new(); // property -> delta
    public readonly Dictionary<string, int> bonuses = new();
    
    public UnitStatusEffect(string name) {
        this.name = name;
        icon = name;
    }

    public UnitStatusEffect(string name, int moodDelta) : this(name) {
        if (moodDelta != 0) bonuses["mood"] = moodDelta;
    }
}
}