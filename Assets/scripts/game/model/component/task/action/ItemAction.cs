using System.Collections.Generic;
using game.model.component.item;
using game.model.component.task.action.target;
using game.model.container.item;
using Leopotam.Ecs;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.component.task.action {
    /**
    * Abstract action, which performs manipulations with items.
    * 
    * @author Alexander on 22.06.2020.
    */
    public abstract class ItemAction : Action {
        protected ItemContainer container => task.take<TaskActionsComponent>().model.itemContainer;

        protected ItemAction(ActionTarget target) : base(target) { }

        // TODO reference to task?
        protected void setItemsLocked(List<EcsEntity> items, bool value) {
            if (value) {
                items.ForEach(item => item.Replace(new ItemLockedComponent()));
                log("locking " + items.Count + " items");
            } else {
                items.ForEach(item => item.Del<ItemLockedComponent>());
                log("unlocking " + items.Count + " items");
            }
        }
    }
}