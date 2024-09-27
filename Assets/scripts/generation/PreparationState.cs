using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace generation {
// filled from UI when player selects settlers and items
public class PreparationState : Singleton<PreparationState> {
    public List<UnitData> units = new();
    public List<ItemData> items = new();
    public Vector2Int location;
    public int size = 100;
}

// Descriptor for settler. Used to generate unit when game starts.
public class UnitData {
    public string faction;
    public string name;
    public string race;
    public int age;
    public string type;
    public bool male;
    public int headVariant;
    public int bodyVariant;
    public StatsData statsData;
    // todo 
}

public struct ItemData {
    public string type;
    public string material;
    public int quantity;
}

public class StatsData {
    public int strength;
    public int agility;
    public int endurance;
    public int intelligence;
    public int spirit;
    public int charisma;
}
}