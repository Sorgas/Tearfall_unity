using game.model.component.plant;
using generation.plant;
using Leopotam.Ecs;
using types.plant;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.plant {
    // Handles plants which are considered harvested. Restarts growth or destroys plant
    public class PlantHarvestSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<PlantHarvestedComponent> filter;
        private readonly PlantGenerator generator = new();
        private bool debug = true;
        
        public override void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                PlantType plantType = entity.take<PlantComponent>().type;
                entity.Del<PlantHarvestedComponent>();
                if (plantType.destroyOnHarvest) {
                    model.plantContainer.removePlant(entity); // cause tile update
                    log("plant removed from container");
                } else {
                    entity.Replace(generator.generateProductGrowthComponent(plantType, 0));
                    entity.Get<PlantVisualUpdateComponent>().add(PlantUpdateType.HARVEST_TIMEOUT);
                    log("plant product growth restarted");
                }
            }
        }

        private void log(string message) {
            if(debug) Debug.Log("[PlantHarvestSystem] " + message);
        }
    }
}