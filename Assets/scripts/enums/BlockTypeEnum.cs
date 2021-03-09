using System.Collections;
using System.Collections.Generic;
using Assets.scripts.enums;
using UnityEngine;

namespace Assets.scripts.enums {
    public static class BlockTypeEnum {
        public static BlockType SPACE = new BlockType(0, PassageEnum.IMPASSABLE, 16, true, 0, "space");
        public static BlockType WALL = new BlockType(1, PassageEnum.IMPASSABLE, 0, false, 3, "wall"); // not passable
        public static BlockType FLOOR = new BlockType(2, PassageEnum.PASSABLE, 12, true, 1, "floor"); // passable, liquids don't fall
        public static BlockType STAIRS = new BlockType(3, PassageEnum.PASSABLE, 8, false, 2, "stairs"); // DF-like stairs
        public static BlockType DOWNSTAIRS = new BlockType(4, PassageEnum.PASSABLE, 14, true, 1, "downstairs");
        public static BlockType RAMP = new BlockType(5, PassageEnum.PASSABLE, 6, false, 2, "ramp"); // passable, liquids don't fall
        public static BlockType FARM = new BlockType(17, PassageEnum.PASSABLE, 12, true, 1, "farm plot"); // 
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

        public BlockType(int id, Passage passage, int openness, bool flat, int product, string name) {
            this.CODE = id;
            this.PASSAGE = passage;
            this.OPENNESS = openness;
            this.FLAT = flat;
            this.PRODUCT = product;
            this.NAME = name;
        }
    }
}