using game.model.component.item;
using game.model.component.task.order;
using game.model.component.unit;
using game.model.container;
using Leopotam.Ecs;
using types.item;
using types.material;
using types.unit.skill;
using UnityEngine;
using util.geometry.bounds;
using util.lang.extension;

namespace game.model.component.task.action {
// action for building buildings: (furniture, workbenches, etc.)
public class BuildingAction : GenericBuildingAction {
    private readonly BuildingOrder buildingOrder;
    
    public BuildingAction(EcsEntity designation, BuildingOrder order) : base(designation, order) {
        name = $"{order.type.title} building action";
        buildingOrder = order;
        bounds = IntBounds3.byStartAndSize(order.position, buildingOrder.type.getSize3ByOrientation(buildingOrder.orientation));

        onFinish = () => {
            model.buildingContainer.createBuilding(order);
            PlantContainer container = model.plantContainer;
            container.removePlant(container.getPlant(order.position), true); // remove plant
            // GameMvc.model().get(SubstrateContainer.class).remove(order.position); // remove substrates
            consumeItems();
            Debug.Log(order.type.name + " built at " + order.position);
        };
    }
}
}