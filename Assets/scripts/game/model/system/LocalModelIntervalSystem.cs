using game.model.localmap;

namespace game.model.system {
    public abstract class LocalModelIntervalSystem : EcsRunIntervalSystem {
        protected LocalModel model;

        protected LocalModelIntervalSystem(LocalModel model, int interval) : base(interval) {
            this.model = model;
        }
    }
}