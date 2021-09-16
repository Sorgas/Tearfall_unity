using System.Collections.Generic;
using static Assets.scripts.enums.PassageEnum;

namespace Assets.scripts.enums {
    public static class BlockTypeEnum {
        public static BlockType SPACE = new BlockType(0, IMPASSABLE, 16, true, 0, "space", null);
        public static BlockType WALL = new BlockType(1, IMPASSABLE, 0, false, 3, "wall", "WALL"); // not passable
        public static BlockType FLOOR = new BlockType(2, PASSABLE, 12, true, 1, "floor", "WALLF"); // passable, liquids don't fall
        public static BlockType STAIRS = new BlockType(3, PASSABLE, 8, false, 2, "stairs", "ST"); // DF-like stairs
        public static BlockType DOWNSTAIRS = new BlockType(4, PASSABLE, 14, true, 1, "downstairs", "STF");
        public static BlockType RAMP = new BlockType(5, PASSABLE, 6, false, 2, "ramp", ""); // passable, liquids don't fall
        public static BlockType FARM = new BlockType(6, PASSABLE, 12, true, 1, "farm plot", "");
        private static BlockType[] all = { SPACE, WALL, FLOOR, STAIRS, DOWNSTAIRS, RAMP, FARM };

        private static Dictionary<int, BlockType> map = new Dictionary<int, BlockType>();
        private static Dictionary<string, BlockType> nameMap = new Dictionary<string, BlockType>();

        static BlockTypeEnum() {
            foreach (BlockType type in all) {
                map.Add(type.CODE, type);
                nameMap.Add(type.NAME, type);
            }
        }

        public static BlockType get(int code) {
            return map[code];
        }

        public static BlockType get(string name) {
            return nameMap[name];
        }
    }

    public class BlockType {
        public readonly int CODE;
        public readonly Passage PASSAGE;
        public readonly int OPENNESS;
        public readonly bool FLAT;
        public readonly int PRODUCT;
        public readonly string NAME;
        public readonly string PREFIX;

        public BlockType(int id, Passage passage, int openness, bool flat, int product, string name, string prefix) {
            CODE = id;
            PASSAGE = passage;
            OPENNESS = openness;
            FLAT = flat;
            PRODUCT = product;
            NAME = name;
            PREFIX = prefix;
        }
    }
}