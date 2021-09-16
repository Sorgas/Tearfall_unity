using Leopotam.Ecs;

namespace Tearfall_unity.Assets.scripts.game.model.system.unit {
    public class ActionPerformingSystem : IEcsRunSystem {
        public EcsFilter<CurrentActionComponent> filter;
        
        public void Run() {
            foreach(int i in filter) {
                CurrentActionComponent component = filter.Get1();
                Action action = component.action;
                action.progressConsumer
            }
        }
    }
}