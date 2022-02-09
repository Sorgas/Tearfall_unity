using System.Collections.Generic;
using enums.unit.need;
using UnityEditor;

namespace enums.unit {
    public class CreatureType {
        public string name;
        public string title;
        public string description;
        public readonly Dictionary<string, BodyPart> bodyParts = new Dictionary<string, BodyPart>();
        public readonly Dictionary<string, List<string>> slots = new Dictionary<string, List<string>>(); // slot name to default limbs
        public readonly List<string> desiredSlots = new List<string>();
        public readonly List<NeedEnum> needs = new List<NeedEnum>();
        public readonly List<string> aspects = new List<string>();
        public int[] atlasXY;
        public string color;
        // public CombinedAppearance combinedAppearance;
        // public readonly Dictionary<GameplayStatEnum, float> statMap = new HashMap<>();

        public CreatureType(RawCreatureType rawType) {
            name = rawType.name;
            title = rawType.title;
            description = rawType.description;
            atlasXY = rawType.atlasXY;
            color = rawType.color;
            aspects.AddRange(rawType.aspects);
            // combinedAppearance = rawType.combinedAppearance;
        }

        public CreatureType() {}
    }
}