using System;
using System.Collections.Generic;
using types.unit.race;

namespace types.unit {
    [Serializable]
    public class RawCreatureType {
        public string name;
        public string title;
        public string description;
        public string bodyTemplate;
        public List<string> desiredSlots = new List<string>();
        public List<string> aspects = new List<string>();
        public int[] atlasXY;
        public string color;
        public CombinedAppearance combinedAppearance;
        public Dictionary<string, float> statMap = new Dictionary<string, float>();
    }
}