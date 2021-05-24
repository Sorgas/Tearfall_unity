using System.Collections.Generic;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.enums.material {
    public class Material {
        public int id;
        public string name;
        // public List<string> tags;
        public float density;
        public int value;
        public int atlasY;
        public Color color;
        public float workAmountModifier; // changes time of building and crafting

        public Material(int id, RawMaterial raw) {
            this.id = id;
            name = raw.name;
            // tags = new List<string>(raw.tags);
            density = raw.density;
            // reactions = raw.reactions;
            value = raw.value;
            atlasY = raw.atlasY;
            color = parseColor(raw.color);
            // workAmountModifier = raw.workAmountModifier;
        }

        public string toString() {
            return "material[" + name + "]";
        }

        public Color parseColor(string color) {
            color.Substring(0,2);
            float r = float.Parse(color.Substring(0,2));
            float g = float.Parse(color.Substring(2,2));
            float b = float.Parse(color.Substring(4,2));
            float a = float.Parse(color.Substring(6,2));
            return new Color(r, g, b, a);
        }
    }
}