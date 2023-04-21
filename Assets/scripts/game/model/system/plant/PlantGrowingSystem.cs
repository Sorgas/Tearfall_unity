using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;
using static game.model.component.plant.PlantUpdateType;

namespace game.model.system.plant {
    // adds time delta to plant age.
    // adds plant growth based on plant maturity age and current conditions
    public class PlantGrowingSystem : LocalModelIntervalEcsSystem {
        public EcsFilter<PlantComponent>.Exclude<PlantRemoveComponent> filter;
        private const int UPDATE_INTERVAL = GameTime.ticksPerMinute * 5;
        // plant age is in days
        private const float PLANT_AGE_DELTA = ((float)UPDATE_INTERVAL) / GameTime.ticksPerDay;

        public PlantGrowingSystem() : base(UPDATE_INTERVAL) { }

        protected override void runIntervalLogic(int updates) {
            foreach (int i in filter) {
                ref PlantComponent plant = ref filter.Get1(i);
                plant.age += PLANT_AGE_DELTA * updates;
                if (plant.growth < 1) {
                    // TODO check light level on plant tile
                    // TODO check current temperature on plant tile
                    plant.growth += PLANT_AGE_DELTA * updates / plant.type.maturityAge;
                    Debug.Log(plant.growth);
                    if (plant.growth > plant.type.growthStages[plant.currentStage]) {
                        Debug.Log("stage " + plant.currentStage);
                        plant.currentStage++;
                        filter.GetEntity(i).Replace(new PlantVisualUpdateComponent { type = GROW });
                        filter.GetEntity(i).Replace(new PlantUpdateComponent { type = GROW });
                    }
                    if (plant.growth > 1) plant.growth = 1f;
                }
            }
        }
    }
}