using game.model.component.task.action.target;
using game.model.component.unit;
using game.model.container.item;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.component.task.action {
    /**
    * Abstract action, which performs manipulations with items.
    * 
    * @author Alexander on 22.06.2020.
    */
    public abstract class EquipmentAction : Action {
        protected ItemContainer container => task.take<TaskActionsComponent>().model.itemContainer;

        protected EquipmentAction(ActionTarget target) : base(target) {
            name = "item action";
        }
        
        protected ref UnitEquipmentComponent equipment() {
            return ref (performerRef.takeRef<UnitEquipmentComponent>());
        }
    }
}