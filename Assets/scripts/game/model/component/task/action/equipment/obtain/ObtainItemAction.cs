using enums.action;
using game.model.component.item;
using game.model.component.task.action.target;
using Leopotam.Ecs;

namespace game.model.component.task.action.equipment.obtain {
    public class ObtainItemAction : EquipmentAction {

        public ObtainItemAction(EcsEntity item) : base(new SelfActionTarget(), item) {
            name = "obtain item action";
            startCondition = () => {
                if (equipment().hauledItem == item) return ActionConditionStatusEnum.OK;
                
                // TODO take item from own equipment
                // if(item.Has<ItemHeldComponent>() && item.take<ItemHeldComponent>().holder == performer) return addPreAction() TODO 
                if(item.Has<ItemContainedComponent>()) return addPreAction(new GetItemFromContainerAction(item)); 
                if(item.Has<PositionComponent>()) return addPreAction(new GetItemFromGroundAction(item)); // pickup from ground
                return ActionConditionStatusEnum.FAIL; // item is not registered in container or equipped on another unit
            };

            onStart = () => maxProgress = 0;
        }
    }
}