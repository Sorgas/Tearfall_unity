using game.model.component.task.action.target;
using game.model.container.item;

namespace game.model.component.task.action {
    /**
    * Abstract action, which performs manipulations with items.
    * 
    * @author Alexander on 22.06.2020.
    */
    public abstract class ItemAction : Action {
        protected ItemContainer container;

        protected ItemAction(ActionTarget target) : base(target) {
            container = GameModel.get().itemContainer;
        }
    }
}