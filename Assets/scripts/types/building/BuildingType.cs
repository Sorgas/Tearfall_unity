using System;

namespace types.building {
    [Serializable]
    public class BuildingType {
        public string name;
        public string tileset;
        public int[] size; // for N orientation
        public int[] positionN;
        public int[] positionS;
        public int[] positionE;
        public int[] positionW;
        public int[] access;
        public string passage;
        public string[] materials;
        public BuildingVariant[] variants;
        public string category;
        public bool isSingleTile() {
            return size[0] == 1 && size[1] == 1;
        }

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