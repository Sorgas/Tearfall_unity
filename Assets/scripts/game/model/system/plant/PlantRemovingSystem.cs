using game.model.component.plant;
using generation.item;
using Leopotam.Ecs;
using types.plant;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.plant {
    // removes plant entity and visual sprite GO
    public class PlantRemovingSystem : LocalModelUnscalableEcsSystem {
        // public EcsFilter<PlantRemoveComponent> filter;
        private ItemGenerator generator = new();

        public override void Run() {
            // foreach (int i in filter) {
            //     EcsEntity plant = filter.GetEntity(i);
            //     if (plant.Has<PlantVisualComponent>()) {
            //     }
            //         
            // }
        }

        // TODO move to ChopTreeAction
        private void leavePlantProduct(EcsEntity plant) {
            PlantComponent component = plant.take<PlantComponent>();
            PlantType type = component.type;
            if (type.isTree) { // leave logs per tree block
                leaveBlockProduct(component.block, plant.pos());
            }
        }

        private void leaveBlockProduct(PlantBlock block, Vector3Int position) {
            if (block.blockType == PlantBlockTypeEnum.TRUNK.code) {
                EcsEntity product = generator.generateItem("log", "wood", model.createEntity());
                model.itemContainer.onMap.putItemToMap(product, position);
            }
        }
    }
}