using System.Collections.Generic;

namespace types.item {
// crafted items have quality, which affects their stats
/**
 * TODO
 * damageMod
 * insulationMod
 * beautyMod
 * color
 */
public static class ItemQualities {
    public static ItemQuality AWFUL = new("awful", 0.2f, 0.9f); // grey
    public static ItemQuality BAD = new("bad", 0.5f, 0.95f); // grey
    public static ItemQuality NORMAL = new("normal", 1f, 1); // white
    public static ItemQuality FINE = new("fine", 1.3f, 1.03f); // green 
    public static ItemQuality EXCELLENT = new("excellent", 2f, 1.06f); // blue
    public static ItemQuality MASTERWORK = new("masterwork", 20f, 1.09f); // violet
    public static ItemQuality LEGENDARY = new("legendary", 0.2f, 1.12f); // orange

    private static readonly Dictionary<string, ItemQuality> map = new();

    static ItemQualities() {
        map.Add(AWFUL.name, AWFUL);
        map.Add(BAD.name, BAD);
        map.Add(NORMAL.name, NORMAL);
        map.Add(FINE.name, FINE);
        map.Add(EXCELLENT.name, EXCELLENT);
        map.Add(MASTERWORK.name, MASTERWORK);
        map.Add(LEGENDARY.name, LEGENDARY);
    }

    public static ItemQuality get(string name) {
        return map[name];
    }
}

public class ItemQuality {
    public string name;
    public float valueMod;
    public float sleepSpeedMod;

    public ItemQuality(string name, float valueMod, float sleepSpeedMod) {
        this.name = name;
        this.valueMod = valueMod;
        this.sleepSpeedMod = sleepSpeedMod;
    }
}
}