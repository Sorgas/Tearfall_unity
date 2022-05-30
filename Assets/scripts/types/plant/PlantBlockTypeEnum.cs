using System.Collections.Generic;

namespace enums.plant {
    class PlantBlockTypeEnum {
        public static readonly PlantBlockType STOMP = new(10, false, "log");
        public static readonly PlantBlockType ROOT = new(11, false, "root");
        public static readonly PlantBlockType TRUNK = new(12, false, "log");
        public static readonly PlantBlockType BRANCH = new(13, true, "branch");
        public static readonly PlantBlockType CROWN = new(14, true, null);
        public static readonly PlantBlockType SINGLE_PASSABLE = new(15, true, null);
        public static readonly PlantBlockType SINGLE_NON_PASSABLE = new(16, false, null);
        private static PlantBlockType[] all = { STOMP, ROOT, TRUNK, CROWN, SINGLE_PASSABLE, SINGLE_NON_PASSABLE };

        private static readonly Dictionary<int, PlantBlockType> map;

        static PlantBlockTypeEnum() {
            map = new Dictionary<int, PlantBlockType>();
            foreach (PlantBlockType type in all) {
                map.Add(type.code, type);
            }
        }

        public static PlantBlockType getType(int code) {
            return map[code];
        }
    }

    public class PlantBlockType {
        public readonly int code;
        public readonly bool passable;
        public readonly string cutProduct;

        public PlantBlockType(int code, bool passable, string product) {
            this.code = code;
            this.passable = passable;
            this.cutProduct = product;
        }
    }
}
