using game.model.component.plant;
using Leopotam.Ecs;
using types.plant;
using UnityEngine;
using util.lang.extension;
using static game.model.component.plant.PlantUpdateType;

namespace game.model.system.plant {
    // plants are growing from planting moment to maturity age
    // counts plant growth. Creates update events when stage changes. 
    public class PlantGrowthSystem : LocalModelPlantSystem {
        public EcsFilter<PlantGrowthComponent>.Exclude<PlantRemoveComponent> filter;
        
        protected override void runIntervalLogic(int updates) {
            foreach (int i in filter) {
                ref PlantGrowthComponent growthComponent = ref filter.Get1(i);
                if (growthComponent.growth < growthComponent.maturityAge) {
                    EcsEntity entity = filter.GetEntity(i);
                    PlantType type = entity.take<PlantComponent>().type;
                    growthComponent.growth += updates * TIME_DELTA * getGrowthFactor(entity.pos());
                    if (growthComponent.growth > growthComponent.nextStage) {
                        growthComponent.currentStage++;
                        if (growthComponent.currentStage >= type.growthStages.Length - 1) { // current stage is last
                            entity.Del<PlantGrowthComponent>(); // stop growth
                        } else {
                            growthComponent.nextStage = type.growthStages[growthComponent.currentStage];
                        }
                        entity.Replace(new PlantVisualUpdateComponent { type = STAGE_CHANGE }); // to update visual
                    }
                }
            }
        }

        // TODO check light level on plant tile
        // TODO check current temperature on plant tile
        private float getGrowthFactor(Vector3Int position) {
            return 1f;
        }
    }
}