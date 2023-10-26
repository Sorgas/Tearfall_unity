using System;
using System.Collections.Generic;
using UnityEngine;
using static System.Globalization.NumberStyles;

namespace types.material {
public class Material_ {
    public int id;
    public string name;
    public HashSet<string> tags;
    public float density;
    public int workAmountModifier = 0; // in percents
    public int value;
    public string tileset;
    public Color color;
    public bool isVariant;

    public Material_(RawMaterial raw) {
        id = raw.id;
        name = raw.name;
        tags = new HashSet<string>(raw.tags);
        density = raw.density;
        workAmountModifier = raw.workAmountModifier;
        value = raw.value;
        tileset = raw.tileset;
        color = parseColor(raw.color);
    }

    public Material_(Material_ source) {
        id = source.id;
        name = source.name;
        tags = new HashSet<string>(source.tags);
        density = source.density;
        value = source.value;
        tileset = source.tileset;
        color = source.color;
    }

    public Color parseColor(string color) {
        float r = Int32.Parse(color.Substring(0, 2), HexNumber);
        float g = Int32.Parse(color.Substring(2, 2), HexNumber);
        float b = Int32.Parse(color.Substring(4, 2), HexNumber);
        float a = Int32.Parse(color.Substring(6, 2), HexNumber);
        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
}

[Serializable]
public class RawMaterial {
    public int id;
    public string name;
    public string[] tags;
    public float density;
    public int workAmountModifier = 0; // in percents
    public int value;
    public string tileset;
    public string color;
}
}