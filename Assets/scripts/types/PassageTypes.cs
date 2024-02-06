using System.Collections.Generic;

namespace types {
// describes passage types for map tiles. Different creatures can move through different passage types.
public static class PassageTypes {
    public static Passage IMPASSABLE = new(0, "impassable");
    public static Passage PASSABLE = new(1, "passable");
    public static Passage DOOR = new(2, "door");
    public static Passage FLY = new(3, "fly");
    public static Passage IMPASSABLE_BUILDING = new(4, "impassable_building"); // does not block room area calculation

    private static Dictionary<int, Passage> map = new();
    private static Dictionary<string, Passage> nameMap = new();

    static PassageTypes() {
        map.Add(IMPASSABLE.VALUE, IMPASSABLE);
        map.Add(PASSABLE.VALUE, PASSABLE);
        map.Add(DOOR.VALUE, DOOR);
        map.Add(FLY.VALUE, FLY);
        map.Add(IMPASSABLE_BUILDING.VALUE, IMPASSABLE_BUILDING);
        nameMap.Add(IMPASSABLE.name, IMPASSABLE);
        nameMap.Add(PASSABLE.name, PASSABLE);
        nameMap.Add(DOOR.name, DOOR);
        nameMap.Add(FLY.name, FLY);
        nameMap.Add(IMPASSABLE_BUILDING.name, IMPASSABLE_BUILDING);
    }

    public static Passage get(int id) {
        return map[id];
    }

    public static Passage get(string name) {
        return nameMap[name];
    }
}

public class Passage {
    public readonly int VALUE;
    public readonly string name;

    public Passage(int value, string name) {
        VALUE = value;
        this.name = name;
    }
}
}