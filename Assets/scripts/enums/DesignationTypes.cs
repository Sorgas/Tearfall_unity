using enums.action;
using game.model.util.validation;
using static enums.action.ActionTargetTypeEnum;
using static enums.BlockTypeEnum;
using static enums.DesignationTypeEnum;

namespace enums {
    // types of designations, referenced from MouseToolEnum
    // icons for cursor are taken from icons/designations/[spriteName]
    public static class DesignationTypes {
        public static DesignationType D_NONE = new(DTE_NONE, "none", null); // for removing simple designations
        public static DesignationType D_DIG = new(DTE_DIG, "dig", new DiggingValidator(FLOOR), NEAR, "miner", "dig");
        public static DesignationType D_STAIRS = new(DTE_STAIRS, "dig stairs", new DiggingValidator(STAIRS), ANY, "miner", "stairs");
        public static DesignationType D_DOWNSTAIRS = new(DTE_DOWNSTAIRS, "dig downstairs", new DiggingValidator(DOWNSTAIRS), ANY, "miner", "downstairs");
        public static DesignationType D_RAMP = new(DTE_RAMP, "dig ramp", new DiggingValidator(RAMP), ANY, "miner", "ramp");
        public static DesignationType D_CHANNEL = new(DTE_CHANNEL, "dig channel", new DiggingChannelValidator(), NEAR, "miner", "channel");
        public static DesignationType D_CHOP = new DesignationType(DTE_CHOP, "chopping trees", PlaceValidatorEnum.TREE_EXISTS, NEAR, "lumberjack", "choptrees");     // chop trees in th area
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
        public readonly ActionTargetTypeEnum targetType;
        public readonly string JOB;
        public readonly string SPRITE_NAME;
        public readonly string name;

        public DesignationType(DesignationTypeEnum type, string name, PositionValidator validator, ActionTargetTypeEnum targetType, string job, string spriteName) {
            TYPE = type;
            this.name = name;
            this.targetType = targetType;
            VALIDATOR = validator;
            JOB = job;
            SPRITE_NAME = spriteName;
        }

        public DesignationType(DesignationTypeEnum type, string name, string job) : this(type, name, null, EXACT, job, null) { }

        // cast exception if not digging
        public BlockType getDiggingBlockType() {
            return ((DiggingValidator)VALIDATOR).targetBlockType;
        }
    }

    public enum DesignationTypeEnum {
        DTE_NONE, // clears designations
        DTE_DIG, // removes walls and ramps. leaves floor or downstairs
        DTE_STAIRS, // cuts stairs from wall or ramp
        DTE_DOWNSTAIRS, // cuts stairs from wall or downstairs from floor
        DTE_RAMP, // digs ramp and upper cell.
        DTE_CHANNEL, // digs cell and ramp on lower level
        DTE_CHOP
    }
}