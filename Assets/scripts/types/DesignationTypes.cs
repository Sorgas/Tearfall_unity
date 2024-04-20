using game.model.util.validation;
using types.action;
using types.unit;
using static types.action.ActionTargetTypeEnum;
using static types.BlockTypes;

namespace types {
    // types of designations, referenced from MouseToolEnum
    // icons for cursor are taken from icons/designations/[spriteName]
    // TODO add non-null validators to all 
    public static class DesignationTypes {
        public static DesignationType D_CLEAR = new("none", null, "cancel", "cancel"); // for removing simple designations

        public static DesignationType D_DIG = new("dig", new DiggingValidator(FLOOR), NEAR, "miner", "dig", "dig");
        public static DesignationType D_STAIRS = new("stairs", new DiggingValidator(STAIRS), ANY, "miner", "stairs", "stairs");
        public static DesignationType D_DOWNSTAIRS = new("downstairs", new DiggingValidator(DOWNSTAIRS), ANY, "miner", "downstairs", "downstairs");
        public static DesignationType D_RAMP = new("ramp", new DiggingValidator(RAMP), ANY, "miner", "ramp", "ramp");
        public static DesignationType D_CHANNEL = new("channel", new DiggingChannelValidator(), NEAR, "miner", "channel", "channel");

        public static DesignationType D_CHOP = new("chopping trees", PlaceValidators.TREE_EXISTS, NEAR, Jobs.WOODCUTTER.name, "choptrees", "choptrees"); // chop trees in th area
        public static DesignationType D_CONSTRUCT = new("construction", "builder");
        public static DesignationType D_BUILD = new("building", "builder");
        public static DesignationType D_HARVEST_PLANT = new("harvesting plant", PlaceValidators.PLANT_HARVESTING, NEAR, Jobs.FORAGER.name, "harvest", "harvest");
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
}