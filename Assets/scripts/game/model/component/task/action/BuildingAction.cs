using game.model.component.task.order;
using game.model.system.plant;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.component.task.action {
    public class BuildingAction : GenericBuildingAction {

        public BuildingAction(EcsEntity designation, BuildingOrder order) : base(designation, order) {
            name = "building action";
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