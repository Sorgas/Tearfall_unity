using game.model.component.plant;
using Leopotam.Ecs;
using types.plant;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.plant {
    // plants grow their products for some time and then become harvestable
    public class PlantProductGrowthSystem : LocalModelPlantSystem {
        public EcsFilter<PlantProductGrowthComponent> filter;

        protected override void runIntervalLogic(int updates) {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ref PlantProductGrowthComponent component = ref filter.Get1(i);
                component.growth += updates * TIME_DELTA * getConditionsFactor(entity.pos());
                if (component.growth > component.growthEnd) {
                    entity.Del<PlantProductGrowthComponent>();
                    PlantType type = entity.take<PlantComponent>().type;
                    if (type.productKeepTime > 0) {
                        entity.Replace(new PlantHarvestKeepComponent { productKeepTime = type.productKeepTime, harvestTime = 0 });
                    }
                    entity.Replace(new PlantHarvestableComponent());
                    entity.Get<PlantVisualUpdateComponent>().add(PlantUpdateType.HARVEST_READY);
                    model.plantContainer.plantUpdated(entity); // to update zones
                }
            }
        }

        // TODO apply conditions
        private float getConditionsFactor(Vector3Int position) {
            return 1f;
        }
    }
}