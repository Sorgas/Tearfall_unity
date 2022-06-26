using System;

namespace types.building {
    [Serializable]
    public class BuildingType {
        public string name;
        public string tileset;
        public int[] size;
        public int[] positionN;
        public int[] positionS;
        public int[] positionE;
        public int[] positionW;
    }
}