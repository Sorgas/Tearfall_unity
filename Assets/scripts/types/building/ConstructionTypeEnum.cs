namespace types.building {
    public class ConstructionTypeEnum {
        public static readonly ConstructionType WALL = new(BlockTypeEnum.WALL, 4);
        public static readonly ConstructionType FLOOR = new(BlockTypeEnum.FLOOR, 4);
        public static readonly ConstructionType RAMP = new(BlockTypeEnum.RAMP, 4);
        public static readonly ConstructionType STAIRS = new(BlockTypeEnum.STAIRS, 4);
        public static readonly ConstructionType DOWNSTAIRS = new(BlockTypeEnum.DOWNSTAIRS, 4);
    }

    public class ConstructionType {
        public BlockType blockType;
        public int resourceAmount;
        public string iconName;

        public ConstructionType(BlockType blockType, int resourceAmount) {
            this.blockType = blockType;
            this.resourceAmount = resourceAmount;
        }
    }
}