using enums.action;
using game.model.component.task.action.target;
using Leopotam.Ecs;

namespace game.model.component.task.action.equipment.obtain {
    public class ObtainItemAction : EquipmentAction {

        public ObtainItemAction(EcsEntity item) : base(new SelfActionTarget(), item) {
            startCondition = () => {
                if (equipment().hauledItem == item) return ActionConditionStatusEnum.OK;
                // TODO contained item should have component ItemContainedComponent
                // if (itemContainer.isItemInContainer(item))
                //     return addPreAction(new GetItemFromContainerAction(item, itemContainer.contained.get(item).entity)); // take from container

                // if (itemContainer.isItemOnMap(item)) 
                return addPreAction(new GetItemFromGroundAction(item)); // pickup from ground

                // TODO equipped item should have component ItemEquippedComponent
                // if (itemContainer.isItemEquipped(item) && itemContainer.equipped.get(item).entity == task.performer)
                //     return addPreAction(new GetItemFromInventory(item)); // item in performers inventory
                // return FAIL; // item is not registered in container or equipped on another unit
            };

            onStart = () => maxProgress = 0;
        }
    }
}