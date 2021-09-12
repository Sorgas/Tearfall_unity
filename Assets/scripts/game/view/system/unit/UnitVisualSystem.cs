using Leopotam.Ecs;

namespace Tearfall_unity.Assets.scripts.game.view.system.unit {
    public class UnitVisualSystem : IEcsRunSystem {
        public EcsFilter<UnitVisualComponent, VisualMovementComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                UnitVisualComponent visual = filter.Get1(i);
                VisualMovementComponent movement = filter.Get2(i);
                if(visual.spriteRenderer = null) {
                    // TODO create unit sprite
                }

            }
        }
    }
}