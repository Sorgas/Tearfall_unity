using game.model.component.task.order;
using game.model.container;
using game.model.system.plant;
using Leopotam.Ecs;
using types;
using types.material;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action {
    public class ConstructionAction : GenericBuildingAction {
        private readonly BlockType blockType;
        
        public ConstructionAction(EcsEntity designation, ConstructionOrder order) : base(designation, order) {
            name = "construction action";
            blockType = order.blockType;
            onFinish = () => {
                int material = MaterialMap.get().id(designation.take<DesignationConstructionComponent>().materialVariant);
                model.localMap.blockType.set(order.position, blockType, material); // create block
                PlantContainer container = model.plantContainer;
                container.removePlant(container.getPlant(order.position), true); // remove plant
                // GameMvc.model().get(SubstrateContainer.class).remove(order.position); // remove substrates
                consumeItems();
                Debug.Log(blockType.NAME + " built at " + order.position);
            };
        }
    }
}