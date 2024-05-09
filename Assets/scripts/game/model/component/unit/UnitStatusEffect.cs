using System.Collections.Generic;

namespace game.model.component.unit {

public class UnitStatusEffect {
    public readonly string name;
    public readonly string icon;
    public readonly Dictionary<string, int> additiveChanges = new(); // property -> delta
    public readonly Dictionary<string, float> multiplicativeChanges = new(); // property -> delta

    public UnitStatusEffect(string name) {
        this.name = name;
        icon = name;
    }

    public UnitStatusEffect(string name, int moodDelta) : this(name) {
        if (moodDelta != 0) additiveChanges["mood"] = moodDelta;
    }
}
}