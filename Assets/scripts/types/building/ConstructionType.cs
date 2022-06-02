using System;

namespace types.building {
    [Serializable]
    public class ConstructionType {
        public string name;
        public string blockTypeName;
        public string[] materials;

        public BlockType blockType;
    }
}