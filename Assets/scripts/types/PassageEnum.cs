﻿using System.Collections.Generic;

namespace enums {
    public static class PassageEnum {
        public static Passage IMPASSABLE = new Passage(0);
        public static Passage PASSABLE = new Passage(1);

        private static Dictionary<int, Passage> map = new Dictionary<int, Passage>();

        static PassageEnum() {
            map.Add(IMPASSABLE.VALUE, IMPASSABLE);
            map.Add(PASSABLE.VALUE, PASSABLE);
        }

        public static Passage get(int id) {
            return map[id];
        }
    }

    public class Passage {
        public readonly int VALUE;

        public Passage(int value) {
            VALUE = value;
        }
    }
}