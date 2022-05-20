using game.model.component;
using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.plant {
    public class PlantRemovingSystem : IEcsRunSystem {
        public EcsFilter<PlantComponent, RemovedComponent> filter;
        
        public void Run() {
            foreach (int i in filter) {
                EcsEntity plant = filter.GetEntity(i);
                if (plant.Has<PlantVisualComponent>()) {
                    Object.Destroy(plant.Get<PlantVisualComponent>().go);
                }
                plant.Destroy();
            }
        }
    }
}