namespace game.model.util.validation {
    public static class PlaceValidators {
        // public static readonly PositionValidator SOIL_FLOOR = new FreeSoilFloorValidator();
        // public static readonly PositionValidator FARM = pos -> GameMvc.model().get(LocalMap.class).blockType.get(pos) == BlockTypeEnum.FARM.CODE
        public static readonly PositionValidator TREE_EXISTS = new TreeExistsValidator();
        // public static readonly PositionValidator DISTANCE_TO_WATER = new DistanceToWaterValidator();
        // public static readonly PositionValidator PLANT_CUTTING = new PlantCuttingValidator()),
        // public static readonly PositionValidator FREE_FLOOR = new FreeFloorValidator()),
        public static readonly PositionValidator CONSTRUCTION = new ConstructionValidator();
        
    }
}