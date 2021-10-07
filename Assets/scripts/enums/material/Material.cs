using System;
using System.Collections.Generic;
using UnityEngine;

namespace enums.material {
    public class Material_ {
        public int id;
        public string name;
        public List<string> tags;
        public float density;
        public int value;
        public string tileset;
        public Color color;

        public Material_(RawMaterial raw) {
            this.id = raw.id;
            name = raw.name;
            tags = new List<string>(raw.tags);
            density = raw.density;
            value = raw.value;
            tileset = raw.tileset;
            color = parseColor(raw.color);
        }

        public string toString() {
            return "material[" + name + "]";
        }

        public Color parseColor(string color) {
            color.Substring(0,2);
            float r = Int32.Parse(color.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
            float g = Int32.Parse(color.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
            float b = Int32.Parse(color.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
            float a = Int32.Parse(color.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
            return new Color(r, g, b, a);
        }
    }
}