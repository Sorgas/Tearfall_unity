using Leopotam.Ecs;

namespace Tearfall_unity.Assets.scripts.game.view.system.unit {
    public class UnitVisualMovementSystem : IEcsRunSystem {
        public EcsFilter<VisualMovementComponent, MovementComponent> filter;

        // update scene position by model movement progress
        public void Run() {
            foreach (int i in filter) {
                MovementComponent movement = filter.Get2(i);
                VisualMovementComponent visual = filter.Get1(i);

                
            }
        }
    }
}