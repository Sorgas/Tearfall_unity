using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.enums.plant {
    class PlantBlockTypeEnum {
        public static readonly PlantBlockType STOMP = new PlantBlockType(10, false, "log");
        public static readonly PlantBlockType ROOT = new PlantBlockType(11, false, "root");
        public static readonly PlantBlockType TRUNK = new PlantBlockType(12, false, "log");
        public static readonly PlantBlockType BRANCH = new PlantBlockType(13, true, "branch");
        public static readonly PlantBlockType CROWN = new PlantBlockType(14, true, null);
        public static readonly PlantBlockType SINGLE_PASSABLE = new PlantBlockType(15, true, null);
        public static readonly PlantBlockType SINGLE_NON_PASSABLE = new PlantBlockType(16, false, null);
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
