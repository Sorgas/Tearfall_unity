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
        public string[] materials;
        public BuildingVariant[] variants;

        public bool isSingleTile() {
            return size[0] == 1 && size[1] == 1;
        }
    }
}