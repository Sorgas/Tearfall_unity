using System.Collections.Generic;

namespace types {
// describes passage types for map tiles. Different creatures can move through different passage types.
public static class PassageTypes {
    public static Passage IMPASSABLE = new Passage(0, "impassable");
    public static Passage PASSABLE = new Passage(1, "passable");
    public static Passage DOOR = new Passage(2, "door");
    public static Passage FLY = new Passage(3, "fly");

    private static Dictionary<int, Passage> map = new();
    private static Dictionary<string, Passage> nameMap = new();

    static PassageTypes() {
        map.Add(IMPASSABLE.VALUE, IMPASSABLE);
        map.Add(PASSABLE.VALUE, PASSABLE);
        map.Add(DOOR.VALUE, DOOR);
        map.Add(FLY.VALUE, FLY);
        nameMap.Add(IMPASSABLE.name, IMPASSABLE);
        nameMap.Add(PASSABLE.name, PASSABLE);
        nameMap.Add(DOOR.name, DOOR);
        nameMap.Add(FLY.name, FLY);
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