using game.model.util.validation;
using static enums.BlockTypeEnum;
using static enums.DesignationTypeEnum;

namespace enums {
    // types of designations, referenced from MouseToolEnum
    public static class DesignationTypes {
        public static DesignationType D_NONE = new DesignationType(DTE_NONE, null);                                                          // for removing simple designations
        public static DesignationType D_DIG = new DesignationType(DTE_DIG, new DiggingValidator(FLOOR), "miner", "dig");                        // removes walls and ramps. leaves floor
        public static DesignationType D_STAIRS = new DesignationType(DTE_STAIRS, new DiggingValidator(STAIRS), "miner", "stairs");             // cuts stairs from wall.
        public static DesignationType D_DOWNSTAIRS = new DesignationType(DTE_DOWNSTAIRS, new DiggingValidator(DOWNSTAIRS), "miner", "downstairs"); // cuts combined stairs from wall. assigned automatically.
        public static DesignationType D_RAMP = new DesignationType(DTE_RAMP, new DiggingValidator(RAMP), "miner", "ramp");                   // digs ramp and upper cell.
        public static DesignationType D_CHANNEL = new DesignationType(DTE_CHANNEL, new DiggingChannelValidator(), "miner", "channel");          // digs cell and ramp on lower level
        // public static DesignationType D_CHOP = new DesignationType(7, "chopping trees", PlaceValidatorEnum.TREE_EXISTS.VALIDATOR, "lumberjack");     // chop trees in th area
        // public static DesignationType D_CUT = new DesignationType(8, "cutting plants", "herbalist");                                          // cut plants
        // public static DesignationType D_HARVEST = new DesignationType(9, "harvesting plants", "herbalist");                                   // harvest plants
        // public static DesignationType D_BUILD = new DesignationType(10, "building", "builder");                                               // build construction or building
        // public static DesignationType D_HOE = new DesignationType(11, "hoeing", PlaceValidatorEnum.SOIL_FLOOR.VALIDATOR, "farmer");
        // public static DesignationType D_CUT_FARM = new DesignationType(12, "cutting plants", "farmer");                                          // cut unwanted plants from farm
        // public static DesignationType D_PLANT = new DesignationType(13, "planting", PlaceValidatorEnum.FARM.VALIDATOR, "farmer");
    }

    public class DesignationType {
        public readonly DesignationTypeEnum TYPE;
        public readonly PositionValidator VALIDATOR;
        public readonly string JOB;
        public readonly string SPRITE_NAME;

        public DesignationType(DesignationTypeEnum type, PositionValidator validator, string job, string spriteName) {
            TYPE = type;
            VALIDATOR = validator;
            JOB = job;
            SPRITE_NAME = spriteName;
        }

        public DesignationType(DesignationTypeEnum type, string job) : this(type, null, job, null) { }
    }

    public enum DesignationTypeEnum {
        DTE_NONE,
        DTE_DIG,
        DTE_STAIRS,
        DTE_DOWNSTAIRS,
        DTE_RAMP,
        DTE_CHANNEL,
    }
}