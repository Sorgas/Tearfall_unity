using game.model.util.validation;
using static enums.BlockTypeEnum;

namespace enums {
    // types of designations, referenced from MouseToolEnum
    public class DesignationTypeEnum {
        public static DesignationType D_NONE = new DesignationType("none", null);                                                          // for removing simple designations
        public static DesignationType D_DIG = new DesignationType("digging", new DiggingValidator(FLOOR), "miner", "dig");                        // removes walls and ramps. leaves floor
        public static DesignationType D_STAIRS = new DesignationType("cutting stairs", new DiggingValidator(STAIRS), "miner", "stairs");             // cuts stairs from wall.
        public static DesignationType D_DOWNSTAIRS = new DesignationType("cutting downstairs", new DiggingValidator(DOWNSTAIRS), "miner", "downstairs"); // cuts combined stairs from wall. assigned automatically.
        public static DesignationType D_RAMP = new DesignationType("cutting ramp", new DiggingValidator(RAMP), "miner", "ramp");                   // digs ramp and upper cell.
        public static DesignationType D_CHANNEL = new DesignationType("digging channel", new DiggingChannelValidator(), "miner", "channel");          // digs cell and ramp on lower level
        // public static DesignationType D_CHOP = new DesignationType(7, "chopping trees", PlaceValidatorEnum.TREE_EXISTS.VALIDATOR, "lumberjack");     // chop trees in th area
        // public static DesignationType D_CUT = new DesignationType(8, "cutting plants", "herbalist");                                          // cut plants
        // public static DesignationType D_HARVEST = new DesignationType(9, "harvesting plants", "herbalist");                                   // harvest plants
        // public static DesignationType D_BUILD = new DesignationType(10, "building", "builder");                                               // build construction or building
        // public static DesignationType D_HOE = new DesignationType(11, "hoeing", PlaceValidatorEnum.SOIL_FLOOR.VALIDATOR, "farmer");
        // public static DesignationType D_CUT_FARM = new DesignationType(12, "cutting plants", "farmer");                                          // cut unwanted plants from farm
        // public static DesignationType D_PLANT = new DesignationType(13, "planting", PlaceValidatorEnum.FARM.VALIDATOR, "farmer");
    }

    public class DesignationType {
        public readonly string NAME;
        public readonly PositionValidator VALIDATOR;
        public readonly string JOB;
        public readonly string SPRITE_NAME;

        public DesignationType(string name, PositionValidator validator, string job, string spriteName) {
            NAME = name;
            VALIDATOR = validator;
            JOB = job;
            SPRITE_NAME = spriteName;
        }

        public DesignationType(string name, string job) : this(name, null, job, null) { }
    }
}