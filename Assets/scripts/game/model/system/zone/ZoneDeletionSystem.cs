using game.model.component;
using Leopotam.Ecs;

namespace game.model.system.zone {
    public class ZoneDeletionSystem : IEcsRunSystem {
        public EcsFilter<ZoneDeletedComponent>.Exclude<ZoneUpdatedComponent> filter;
        
        public void Run() {
            foreach (int i in filter) {
                filter.GetEntity(i).Destroy();
            }
        }
    }
}