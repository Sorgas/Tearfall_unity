using game.model.util.validation;

namespace enums {
    // types of designations, referenced from MouseToolEnum
    public class DesignationTypeEnum {
        public static DesignationType D_NONE = new DesignationType(-1, "none", null);                                                          // for removing simple designations
        public static DesignationType D_DIG = new DesignationType(2, "digging", new DiggingValidator(BlockTypeEnum.FLOOR), "miner", "dig");                        // removes walls and ramps. leaves floor
        public static DesignationType D_STAIRS = new DesignationType(3, "cutting stairs", new DiggingValidator(BlockTypeEnum.STAIRS), "miner", "stairs");             // cuts stairs from wall.
        public static DesignationType D_DOWNSTAIRS = new DesignationType(4, "cutting downstairs", new DiggingValidator(BlockTypeEnum.DOWNSTAIRS), "miner", "downstairs"); // cuts combined stairs from wall. assigned automatically.
        public static DesignationType D_RAMP = new DesignationType(5, "cutting ramp", new DiggingValidator(BlockTypeEnum.RAMP), "miner", "ramp");                   // digs ramp and upper cell.
        public static DesignationType D_CHANNEL = new DesignationType(6, "digging channel", new DiggingChannelValidator(), "miner", "channel");          // digs cell and ramp on lower level
        // public static DesignationType D_CHOP = new DesignationType(7, "chopping trees", PlaceValidatorEnum.TREE_EXISTS.VALIDATOR, "lumberjack");     // chop trees in th area
        public static DesignationType D_CUT = new DesignationType(8, "cutting plants", "herbalist");                                          // cut plants
        public static DesignationType D_HARVEST = new DesignationType(9, "harvesting plants", "herbalist");                                   // harvest plants
        public static DesignationType D_BUILD = new DesignationType(10, "building", "builder");                                               // build construction or building
        // public static DesignationType D_HOE = new DesignationType(11, "hoeing", PlaceValidatorEnum.SOIL_FLOOR.VALIDATOR, "farmer");
        public static DesignationType D_CUT_FARM = new DesignationType(12, "cutting plants", "farmer");                                          // cut unwanted plants from farm
        // public static DesignationType D_PLANT = new DesignationType(13, "planting", PlaceValidatorEnum.FARM.VALIDATOR, "farmer");
    }

    public class DesignationType {
        public readonly int SPRITE_X;
        public readonly string NAME;
        public readonly PositionValidator VALIDATOR;
        public readonly string JOB;
        public readonly string spriteName;

        public DesignationType(int spriteX, string name, PositionValidator validator, string job, string spriteName) {
            SPRITE_X = spriteX;
            name = name;
            VALIDATOR = validator;
            JOB = job;
            this.spriteName = spriteName;
        }

        public DesignationType(int spriteX, string name, string job) : this(spriteX, name, null, job, null) { }
    }
}