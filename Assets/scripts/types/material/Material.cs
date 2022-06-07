using System;
using System.Collections.Generic;
using enums.material;
using UnityEngine;

namespace types.material {
    public class Material_ {
        public int id;
        public string name;
        public List<string> tags;
        public float density;
        public int value;
        public string tileset;
        public Color color;

        public Material_(RawMaterial raw) {
            id = raw.id;
            name = raw.name;
            tags = new List<string>(raw.tags);
            density = raw.density;
            value = raw.value;
            tileset = raw.tileset;
            color = parseColor(raw.color);
        }

        public Material_(Material_ source) {
            id = source.id;
            name = source.name;
            tags = new List<string>(source.tags);
            density = source.density;
            value = source.value;
            tileset = source.tileset;
            color = source.color;
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