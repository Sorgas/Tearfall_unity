using System.Collections.Generic;
using UnityEngine;

namespace types.item {
public class DamageTypes {
    public static readonly DamageType slashing = new ("slashing", "slashing", Color.white);
    public static readonly DamageType blunt = new ("blunt", "blunt", Color.white);
    public static readonly DamageType piercing = new ("piercing", "piercing", Color.white);

    public static readonly Dictionary<string, DamageType> map = new();

    static DamageTypes() {
        map.Add(slashing.name, slashing);
        map.Add(blunt.name, blunt);
        map.Add(piercing.name, piercing);
    }

    public static DamageType get(string name) {
        return map[name];
    }
}

public class DamageType {
    public readonly string name;
    public readonly string iconName;
    public readonly Color color;

    public DamageType(string name, string iconName, Color color) {
        this.name = name;
        this.iconName = iconName;
        this.color = color;
    }
}
}