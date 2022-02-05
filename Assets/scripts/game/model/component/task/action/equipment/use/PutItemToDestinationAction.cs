using game.model.component.task.action.equipment.obtain;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using UnityEditor;
using static enums.action.ActionConditionStatusEnum;

namespace game.model.component.task.action.equipment.use {
    /**
    * Abstract action for putting items to different places like ground, containers, or equipment.
    * Only defines, that item should be in performer's buffer.
    *
    * @author Alexander on 12.04.2020
    */
    public class PutItemToDestinationAction : EquipmentAction {

        protected PutItemToDestinationAction(ActionTarget target, EcsEntity item) : base(target, item) {
            startCondition = () => {
                if (!validate()) return FAIL;
                if (equipment().hauledItem == item) return OK; // performer has item
                return addPreAction(new ObtainItemAction(item));
            };

            onStart = () => maxProgress = 20;
        }
    }
}