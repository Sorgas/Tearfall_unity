using System.Collections.Generic;
using UnityEngine;

namespace types {
    public class ZoneTypes {
        public static ZoneType STOCKPILE = new ZoneType("stockpile", ZoneTypeEnum.STOCKPILE, new Color(1f, 0.92f, 0.015f, 0.2f));
        public static ZoneType FARM = new ZoneType("farm", ZoneTypeEnum.FARM, new Color(0, 1, 0 , 0.2f));

        public static List<ZoneType> all = new() {STOCKPILE, FARM};
    }

    public class ZoneType {
        public readonly string name;
        public readonly ZoneTypeEnum value;
        public readonly Color tileColor;

        public ZoneType(string name, ZoneTypeEnum value, Color tileColor) {
            this.name = name;
            this.value = value;
            this.tileColor = tileColor;
        }
    }
    
    public enum ZoneTypeEnum {
        STOCKPILE,
        FARM
    }
}