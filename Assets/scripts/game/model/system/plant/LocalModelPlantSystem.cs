namespace game.model.system.plant {
    // all plant growing systems share UPDATE_INTERVAL and TIME_DELTA
    public abstract class LocalModelPlantSystem : LocalModelIntervalEcsSystem {
        protected const int UPDATE_INTERVAL = GameTime.ticksPerMinute * 5; // every 5 in-game minutes
        // part of day equal to UPDATE_INTERVAL (plant growth values are is in days)
        public const float TIME_DELTA = ((float)UPDATE_INTERVAL) / GameTime.ticksPerDay;
        
        protected LocalModelPlantSystem() : base(UPDATE_INTERVAL) { }
    }
}