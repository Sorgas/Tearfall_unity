using enums.plant;
using game.model.component.plant;
using generation.item;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.plant {
    public class PlantRemovingSystem : IEcsRunSystem {
        public EcsFilter<PlantRemoveComponent> filter;
        private ItemGenerator generator = new();
        
        public void Run() {
            foreach (int i in filter) {
                EcsEntity plant = filter.GetEntity(i);
                if (plant.Has<PlantVisualComponent>()) {
                    Object.Destroy(plant.Get<PlantVisualComponent>().go);
                }
                if (filter.Get1(i).leaveProduct) {
                    leavePlantProduct(plant);
                }
                plant.Destroy();
            }
        }

        // TODO handle multi-block trees
        private void leavePlantProduct(EcsEntity plant) {
            PlantComponent component = plant.take<PlantComponent>();
            PlantType type = component.type;
            if (type.isTree) { // leave logs per tree block
                leaveBlockProduct(component.block, plant.pos());
            }
        }

        private void leaveBlockProduct(PlantBlock block, Vector3Int position) {
            if (block.blockType == PlantBlockTypeEnum.TRUNK.code) {
                EcsEntity product = generator.generateItem("log", "wood");
                GameModel.get().itemContainer.onMapItems.putItemToMap(product, position);
            }
        }
    }
}