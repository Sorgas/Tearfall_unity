using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using Leopotam.Ecs;
using types.item.recipe;
using util.lang;
using util.lang.extension;

// order for crafting items in workbenches. Order is 'completed' when performed targetQuantity number of times.
namespace game.model.component.task.order {
public class CraftingOrder {
    public string name;
    public Recipe recipe;
    public CraftingOrderStatus status;
    public List<IngredientOrder> ingredients = new();
    public bool repeated;
    public int targetQuantity;
    public int performedQuantity;
    public bool updated; // to updating order line when order updated from model.

    public CraftingOrder(Recipe recipe) {
        name = recipe.title;
        this.recipe = recipe;
        status = CraftingOrderStatus.WAITING;
        targetQuantity = 1;
        performedQuantity = 0;
    }

    public CraftingOrder(CraftingOrder source) {
        name = source.name;
        recipe = source.recipe;
        status = source.status;
        ingredients = source.ingredients.Select(ingredient => new IngredientOrder(ingredient)).ToList();
        repeated = source.repeated;
        targetQuantity = source.targetQuantity;
        performedQuantity = source.performedQuantity;
        updated = source.updated;
    }

    public List<EcsEntity> allIngredientItems() => ingredients.SelectMany(ingredient => ingredient.items).ToList();

    // stores selected item types and materials for crafting
    public class IngredientOrder {
        public readonly List<Ingredient> ingredients = new(); // 'variants'
        // configured from ui, maps allowed item types to corresponding allowed materials
        // all selected items should have same type and material
        public readonly MultiValueDictionary<string, int> selected = new();
        public readonly Dictionary<string, int> quantities = new();
        public readonly List<EcsEntity> items = new(); // selected before performing, cleared after performing

        public IngredientOrder() { }

        public IngredientOrder(IngredientOrder source) {
            ingredients = source.ingredients;
        }

        public bool hasEnoughItems() => items.Count > 0 && quantities[items[0].take<ItemComponent>().type] == items.Count;
    }

    public enum CraftingOrderStatus {
        PERFORMING, // A, order has task
        WAITING, // W, order has no task
        PAUSED, // P, order paused by player
        // PAUSED_PROBLEM // PP, order paused by failed task (similar to PAUSED)
    }
}
}