using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.component.task.action.equipment {

    /**
     * Base class for actions that manipulate unit equipment.
     * Equipment tasks are composed of several actions 'get from source' -> 'put to destination'
     * Examples: get from container, get from ground, 
     * 
     * @author Alexander on 22.06.2020.
     */
    public abstract class EquipmentAction : ItemAction {
        // protected CreatureEquipmentSystem system;
        public EcsEntity item;

        protected EquipmentAction(ActionTarget target, EcsEntity item) : base(target, null) {
            // system = GameMvc.model().get(UnitContainer.class).equipmentSystem;
            this.item = item;
        }

        protected ref UnitEquipmentComponent equipment() {
            if (!performer().Has<UnitEquipmentComponent>()) {
                Debug.LogWarning("unit " + performer() + " has no UnitEquipmentComponent1.");
            }
            return ref (performer().Get<UnitEquipmentComponent>());
        }

        protected bool validate() {
            if (!performer().Has<UnitEquipmentComponent>()) {
                Debug.LogWarning("unit " + performer() + " has no UnitEquipmentComponent2."); 
                return false;
            }
            
            return true;
        }
    }
}