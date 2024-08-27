using game.model.component.building;
using game.model.component.item;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util;

namespace game.model.component.task.action.equipment {
    /**
     * Base class for actions that manipulate a specific item.
     * Equipment tasks are composed of several actions 'get from source' -> 'put to destination'
     * Examples: get from container, get from ground -> put to container, equip onto performer.
     * 
     * @author Alexander on 22.06.2020.
     */
    public abstract class ItemAction : EquipmentAction {
        protected EcsEntity item;

        protected ItemAction(ActionTarget target, EcsEntity item) : base(target) {
            name = "item action";
            this.item = item;
        }

        // validates that item not used by other tasks
        public override bool validate() {
            if (!base.validate()) return false;
            if (!entityCanBeLocked(item)) {
                Debug.LogWarning("item " + item + " locked to another task.");
                return false;
            }
            return true;
        }
        
        protected static ActionTarget resolveItemContainerTarget(EcsEntity entity) {
            if (entity.Has<BuildingComponent>()) return new BuildingActionTarget(entity, ActionTargetTypeEnum.NEAR);
            if (entity.Has<ItemComponent>()) return new ItemActionTarget(entity);
            if (entity.Has<DesignationComponent>()) return new DesignationActionTarget(entity);
            throw new GameException("unsupported item container type");
        }
    }
}