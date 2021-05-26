using System;

namespace Tearfall_unity.Assets.scripts.enums.material {
    [Serializable]
    public class RawMaterial {
        public string name;
        public string[] tags;
        public float density;
        // public Dictionary<string, List<string>> reactions; // other aspects
        public int value;
        public int atlasY;
        public string color;
        // public float workAmountModifier = 1;
        // public string[] aspects; // "aspect(params)"
    }
}