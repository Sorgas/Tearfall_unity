using game.model.component.item;
using Leopotam.Ecs;
using util.item;

public class FoodItemSelector : ItemSelector {

    public override bool checkItem(EcsEntity item) {
        if(item.Has<ItemFoodComponent>()) {
            return true;
        }
        return false;
    }
}