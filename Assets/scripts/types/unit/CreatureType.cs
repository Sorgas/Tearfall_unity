using System;
using System.Collections.Generic;
using types.unit.body;
using types.unit.need;
using types.unit.race;

namespace types.unit {
public class CreatureType {
    public string name;
    public string title;
    public string description;
    public readonly Dictionary<string, BodyPart> bodyParts = new();
    public readonly Dictionary<string, List<string>> slots = new(); // slot name to default limbs
    public readonly Dictionary<string, List<string>> grabSlots = new(); // slot name to default limbs
    public readonly List<string> desiredSlots = new();
    public readonly List<string> femaleDesiredSlots = new();
    public readonly List<Needs> needs = new();
    public readonly List<string> aspects = new();

    public CreatureAppearance appearance;
    // public readonly Dictionary<GameplayStatEnum, float> statMap = new HashMap<>();

    public CreatureType(RawCreatureType raw) {
        name = raw.name;
        title = raw.title;
        description = raw.description;
        aspects.AddRange(raw.aspects);
        appearance = raw.appearance;
    }

    public CreatureType() { }
}

[Serializable]
public class RawCreatureType {
    public string name;
    public string title;
    public string description;
    public string bodyTemplate;
    public List<string> desiredSlots = new();
    public List<string> femaleDesiredSlots = new();
    public List<string> aspects = new();
    public CreatureAppearance appearance;
    public Dictionary<string, float> statMap = new();
}
}