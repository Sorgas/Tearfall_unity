using game.model.component.item;
using Leopotam.Ecs;

namespace util.item {
    public class FoodItemSelector : ItemSelector {

        public override bool checkItem(EcsEntity item) {
            if(item.Has<ItemFoodComponent>()) {
                return true;
            }
            return false;
        }
    }
}