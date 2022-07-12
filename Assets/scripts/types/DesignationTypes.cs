using enums.action;
using game.model.util.validation;
using types.unit;
using static enums.action.ActionTargetTypeEnum;
using static types.BlockTypes;

namespace types {
    // types of designations, referenced from MouseToolEnum
    // icons for cursor are taken from icons/designations/[spriteName]
    public static class DesignationTypes {
        public static DesignationType D_NONE = new("none", null);
        public static DesignationType D_CLEAR = new("none", null, "cancel", "cancel"); // for removing simple designations

        public static DesignationType D_DIG = new("dig", new DiggingValidator(FLOOR), NEAR, "miner", "dig", "dig");
        public static DesignationType D_STAIRS = new("stairs", new DiggingValidator(STAIRS), ANY, "miner", "stairs", "stairs");
        public static DesignationType D_DOWNSTAIRS = new("downstairs", new DiggingValidator(DOWNSTAIRS), ANY, "miner", "downstairs", "downstairs");
        public static DesignationType D_RAMP = new("ramp", new DiggingValidator(RAMP), ANY, "miner", "ramp", "ramp");
        public static DesignationType D_CHANNEL = new("channel", new DiggingChannelValidator(), NEAR, "miner", "channel", "channel");

        public static DesignationType D_CHOP = new("chopping trees", PlaceValidatorEnum.TREE_EXISTS, NEAR, Jobs.WOODCUTTER.name, "choptrees", "choptrees"); // chop trees in th area
        public static DesignationType D_CONSTRUCT = new("construction", "builder");
        public static DesignationType D_BUILD = new("building", "builder");
    }

    public class DesignationType {
        public readonly string name;
        public readonly PositionValidator validator;
        public readonly ActionTargetTypeEnum targetType;
        public readonly string job;
        public readonly string spriteName;
        public readonly string iconName;

        public DesignationType(string name, PositionValidator validator, ActionTargetTypeEnum targetType, string job,
            string spriteName, string iconName) {
            this.name = name;
            this.targetType = targetType;
            this.validator = validator;
            this.job = job;
            this.spriteName = spriteName;
            this.iconName = "toolbar/orders/" + iconName;
        }

        public DesignationType(string name, string job) : this(name, null, EXACT, job, null, null) { }
        
        public DesignationType(string name, string job, string spriteName, string iconName) : this(name, null, EXACT, job, null, iconName) { }

        // cast exception if not digging
        public BlockType getDiggingBlockType() {
            return ((DiggingValidator)validator).targetBlockType;
        }
    }
    
    
    // public static DesignationType D_CUT = new DesignationType(8, "cutting plants", "herbalist");                                          // cut plants
    // public static DesignationType D_HARVEST = new DesignationType(9, "harvesting plants", "herbalist");                                   // harvest plants
    // public static DesignationType D_BUILD = new DesignationType(10, "building", "builder");                                               // build construction or building
    // public static DesignationType D_HOE = new DesignationType(11, "hoeing", PlaceValidatorEnum.SOIL_FLOOR.VALIDATOR, "farmer");
    // public static DesignationType D_CUT_FARM = new DesignationType(12, "cutting plants", "farmer");                                          // cut unwanted plants from farm
    // public static DesignationType D_PLANT = new DesignationType(13, "planting", PlaceValidatorEnum.FARM.VALIDATOR, "farmer");

}