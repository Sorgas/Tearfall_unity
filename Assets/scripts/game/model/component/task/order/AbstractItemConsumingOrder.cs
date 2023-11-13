using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;

namespace game.model.component.task.order {
// Superclass for crafting an building orders. Defines set of configurable IngredientOrders.
// Each IngredientOrder describes allowed combinations of item types and materials. 
// Item selected for these ingredient orders can be consumed during crafting or building.
public abstract class AbstractItemConsumingOrder {
    public string name;
    public Dictionary<string, IngredientOrder> ingredients = new();
    public bool updated; // to update view when order updated from model.

    public List<EcsEntity> allIngredientItems() => ingredients.Values.SelectMany(ingredient => ingredient.items).ToList();

    protected AbstractItemConsumingOrder() {}

    protected AbstractItemConsumingOrder(AbstractItemConsumingOrder source) {
        name = source.name;
        ingredients = source.ingredients.ToDictionary(
            entry => entry.Key, 
            entry => new IngredientOrder(entry.Value));
        updated = source.updated;
    }
}
}