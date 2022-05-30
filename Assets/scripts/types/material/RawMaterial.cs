using System;

namespace enums.material {
    [Serializable]
    public class RawMaterial {        
        public int id;
        public string name;
        public string[] tags;
        public float density;
        public int value;
        public string tileset;
        public string color;
    }
}