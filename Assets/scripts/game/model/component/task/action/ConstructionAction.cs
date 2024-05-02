using game.model.component.item;
using game.model.component.task.order;
using game.model.container;
using Leopotam.Ecs;
using types;
using types.material;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action {
// action for building constructions (tiles of some material)
public class ConstructionAction : GenericBuildingAction {
    private readonly BlockType blockType;

    public ConstructionAction(EcsEntity designation, ConstructionOrder order) : base(designation, order) {
        name = "construction action";
        blockType = order.blockType;
        bounds = new(order.position, order.position);

        onFinish = () => {
            EcsEntity item = order.ingredients["main"].items[0]; // all selected items should be same
            ItemComponent itemComponent = item.take<ItemComponent>();
            int material = itemComponent.material;
            if (MaterialMap.get().variationRequired(itemComponent.type)) {
                material = MaterialMap.get().getVariantFor(itemComponent.material, itemComponent.type).id;
            }
            model.localMap.blockType.set(order.position, blockType, material); // create block
            PlantContainer plantContainer = model.plantContainer;
            plantContainer.removePlant(plantContainer.getPlant(order.position), true); // remove plant
            model.localMap.substrateMap.remove(order.position); // remove substrates
            consumeItems();
            giveExp(order.allIngredientItems().Count * 3);
            Debug.Log(blockType.NAME + " built at " + order.position);
        };
    }
}
}