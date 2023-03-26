using game.model.localmap;
using Leopotam.Ecs;

// EcsSystem that have link to LocalMapModel for operating on entities
namespace game.model.system {
    public abstract class LocalModelEcsSystem : IEcsRunSystem {
        protected LocalModel model;

        public LocalModelEcsSystem(LocalModel model) {
            this.model = model;
        }

        public abstract void Run();
    }
}