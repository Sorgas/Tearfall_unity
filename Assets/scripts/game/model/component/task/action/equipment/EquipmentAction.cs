using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.equipment {

    /**
     * Base class for actions that manipulate unit equipment.
     * Equipment tasks are composed of several actions 'get from source' -> 'put to destination'
     * Examples: get from container, get from ground, 
     * 
     * @author Alexander on 22.06.2020.
     */
    public abstract class EquipmentAction : ItemAction {
        public EcsEntity item;

        protected EquipmentAction(ActionTarget target, EcsEntity item) : base(target) {
            name = "equipment action";
            this.item = item;
        }

        protected ref UnitEquipmentComponent equipment() {
            return ref performer.takeRef<UnitEquipmentComponent>();
        }

        protected bool validate() {
            if (!itemCanBeLocked(item)) return false;
            if (!performer.Has<UnitEquipmentComponent>()) {
                Debug.LogWarning("unit " + performer + " has no UnitEquipmentComponent2.");
                return false;
            }
            return true;
        }
    }
}