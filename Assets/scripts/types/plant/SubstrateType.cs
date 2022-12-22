using System;
using UnityEngine;

public class SubstrateType {
    public int id;
    public string name;
    public string blockTag; // can only spawn on block of material with tag
    public string tileset;
    public int tilesetSize;
    public string rawColor;
    public Color color;
    
    public SubstrateType() { }
    
    public Color parseColor(string color) {
        color.Substring(0,2);
        float r = Int32.Parse(color.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
        float g = Int32.Parse(color.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
        float b = Int32.Parse(color.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
        float a = Int32.Parse(color.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
        return new Color(r, g, b, a);
    }
}