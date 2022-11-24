using System;
using System.Collections.Generic;

namespace types.building {
    [Serializable]
    public class BuildingType {
        public const string SLEEP_FURNITURE = "sleepFurniture";

        public string name;
        public string tileset;
        public int[] size; // for N orientation
        public int[] positionN;
        public int[] positionS;
        public int[] positionE;
        public int[] positionW;
        public int[] access; // TODO add rawbuilding type class, use Vector2Int here
        public string passage;
        public string category;

        public string[] materials; // ways to build this building (raw)
        public BuildingVariant[] variants; // ways to build this building
        
        public string[] rawComponents;
        public List<string> components = new(); // components that should be added to building

        public bool isSingleTile() => size[0] == 1 && size[1] == 1;

        public BuildingVariant selectVariant(string itemType) {
            for (var i = 0; i < variants.Length; i++) {
                if (variants[i].itemType.Equals(itemType)) {
                    return variants[i];
                }
            }
            return null;
        }
    }
}