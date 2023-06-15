using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.container.item;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action {
    /**
    * Abstract action, which performs manipulations with items.
    * 
    * @author Alexander on 22.06.2020.
    */
    public abstract class EquipmentAction : Action {
        protected ItemContainer container => task.take<TaskActionsComponent>().model.itemContainer;
        protected ref UnitEquipmentComponent equipment => ref performer.takeRef<UnitEquipmentComponent>();

        protected EquipmentAction(ActionTarget target) : base(target) {
            name = "equipment action";
        }

        protected bool validate() {
            if (!performer.Has<UnitEquipmentComponent>()) {
                Debug.LogWarning("unit " + performer + " has no UnitEquipmentComponent.");
                return false;
            }
            return true;
        }
    }
}