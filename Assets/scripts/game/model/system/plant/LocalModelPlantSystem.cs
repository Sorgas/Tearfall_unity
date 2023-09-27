namespace game.model.system.plant {
    // all plant growing systems share UPDATE_INTERVAL and TIME_DELTA
    public abstract class LocalModelPlantSystem : LocalModelIntervalEcsSystem {
        //plants are updated every 5 in-game minutes
        private const int UPDATE_INTERVAL = GameTime.ticksPerMinute * 5;
        // part of day equal to UPDATE_INTERVAL (plant growth values are in days)
        protected const float TIME_DELTA = ((float)UPDATE_INTERVAL) / GameTime.ticksPerDay;
        
        protected LocalModelPlantSystem() : base(UPDATE_INTERVAL) { }
    }
}