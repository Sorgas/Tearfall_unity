using System;
using System.Collections.Generic;
using game.model.util.validation;
using UnityEngine;

namespace types {
    public class ZoneTypes {
        public static ZoneType STOCKPILE = new("stockpile", ZoneTypeEnum.STOCKPILE, new StockpilePositionValidator(), new Color(1f, 0.92f, 0.015f, 0.2f));
        public static ZoneType FARM = new("farm", ZoneTypeEnum.FARM, new FarmPositionValidator(), new Color(172f / 255, 1, 89f / 255, 0.2f));

        public static List<ZoneType> all = new() { STOCKPILE, FARM };

        public static ZoneType get(ZoneTypeEnum type) {
            return type switch {
                ZoneTypeEnum.STOCKPILE => STOCKPILE,
                ZoneTypeEnum.FARM => FARM,
                _ => throw new ArgumentException()
            };
        }
    }

    public class ZoneType {
        public readonly string name;
        public readonly ZoneTypeEnum value;
        public readonly Color tileColor;
        public readonly PositionValidator positionValidator;

        public ZoneType(string name, ZoneTypeEnum value, PositionValidator positionValidator, Color tileColor) {
            this.name = name;
            this.value = value;
            this.tileColor = tileColor;
            this.positionValidator = positionValidator;
        }
    }

    public enum ZoneTypeEnum {
        STOCKPILE,
        FARM
    }

    public class ZoneTaskTypes {
        public const string STORE_ITEM = "storeItem";
        public const string REMOVE_ITEM = "removeItem";
        public const string HOE = "hoe";
        public const string PLANT = "plant";
        public const string HARVEST = "harvest";
        public const string REMOVE_PLANT = "removePlant";

        public static readonly string[] STOCKPILE_TASKS = { STORE_ITEM, REMOVE_ITEM };
        public static readonly string[] FARM_TASKS = { HOE, PLANT, HARVEST, REMOVE_PLANT };
    }
}