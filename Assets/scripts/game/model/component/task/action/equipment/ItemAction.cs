using game.model.component.task.action.target;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.component.task.action.equipment {
    /**
     * Base class for actions that manipulate a specific item.
     * Equipment tasks are composed of several actions 'get from source' -> 'put to destination'
     * Examples: get from container, get from ground, 
     * 
     * @author Alexander on 22.06.2020.
     */
    public abstract class ItemAction : EquipmentAction {
        protected EcsEntity item;

        protected ItemAction(ActionTarget target, EcsEntity item) : base(target) {
            name = "item action";
            this.item = item;
        }

        protected bool validate() {
            if (!base.validate()) return false;
            if (!itemCanBeLocked(item)) {
                Debug.LogWarning("item " + item + " locked to another task.");
                return false;
            }
            return true;
        }
    }
}