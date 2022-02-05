using game.model.component.item;
using Leopotam.Ecs;

namespace util.item {
    public class ToolWithActionItemSelector : ItemSelector {
        private string actionName;

        public ToolWithActionItemSelector(string actionName) {
            this.actionName = actionName;
        }

        public override bool checkItem(EcsEntity item) {
            return item.Has<ItemToolComponent>() && item.Get<ItemToolComponent>().operation == actionName;
        }
    }
}