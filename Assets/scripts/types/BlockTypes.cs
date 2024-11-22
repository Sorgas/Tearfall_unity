﻿using System.Collections.Generic;
using static types.PassageTypes;

namespace types {
    public static class BlockTypes {
        public static BlockType SPACE = new(0, FLY, 16, true, 0, "space", null);
        public static BlockType WALL = new(1, IMPASSABLE, 0, false, 3, "wall", "WALL"); // not passable
        public static BlockType FLOOR = new(2, PASSABLE, 12, true, 1, "floor", "WALLF"); // passable, liquids don't fall
        public static BlockType STAIRS = new(3, PASSABLE, 8, false, 2, "stairs", "ST"); // DF-like stairs
        public static BlockType DOWNSTAIRS = new(4, PASSABLE, 14, true, 1, "downstairs", "STF");
        public static BlockType RAMP = new(5, PASSABLE, 6, false, 2, "ramp", ""); // passable, liquids don't fall
        private static BlockType[] all = { SPACE, WALL, FLOOR, STAIRS, DOWNSTAIRS, RAMP};

        private static Dictionary<int, BlockType> map = new();
        private static Dictionary<string, BlockType> nameMap = new();

        static BlockTypes() {
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
        public readonly bool flat; // tile has wall part
        public readonly int PRODUCT;
        public readonly string NAME;
        public readonly string PREFIX;

        public BlockType(int id, Passage passage, int openness, bool flat, int product, string name, string prefix) {
            CODE = id;
            PASSAGE = passage;
            OPENNESS = openness;
            this.flat = flat;
            PRODUCT = product;
            NAME = name;
            PREFIX = prefix;
        }
    }
}