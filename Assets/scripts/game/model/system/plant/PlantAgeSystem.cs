using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.plant {
    // plant age is limited
    // counts plant age, kills plant when max age reached
    public class PlantAgeSystem : LocalModelPlantSystem {
        public EcsFilter<PlantAgeComponent> filter;

        protected override void runIntervalLogic(int updates) {
            foreach (int i in filter) {
                ref PlantAgeComponent ageComponent = ref filter.Get1(i);
                ageComponent.age += TIME_DELTA * updates;
                if (ageComponent.age > ageComponent.maxAge) {
                    EcsEntity entity = filter.GetEntity(i);
                    Debug.Log("plant reached max age");
                    model.plantContainer.removePlant(entity, false);
                }
            }
        }
    }
}