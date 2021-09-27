using Leopotam.Ecs;

namespace Tearfall_unity.Assets.scripts.game.model.system.unit {
    public class ActionPerformingSystem : IEcsRunSystem {
        public EcsFilter<CurrentActionComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                CurrentActionComponent component = filter.Get1(i);
                _Action action = component.action;
                action.perform(unit);
            }
        }
    }
}