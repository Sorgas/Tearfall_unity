using game.model.component.task.action.target;
using game.model.container.item;
using util.lang.extension;

namespace game.model.component.task.action {
    /**
    * Abstract action, which performs manipulations with items.
    * 
    * @author Alexander on 22.06.2020.
    */
    public abstract class ItemAction : Action {
        protected ItemContainer container => task.take<TaskActionsComponent>().model.itemContainer;

        protected ItemAction(ActionTarget target) : base(target) {
            name = "item action";
        }
    }
}