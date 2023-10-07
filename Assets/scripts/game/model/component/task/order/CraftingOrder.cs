using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using types.item.recipe;

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
        // from ingredient
        public readonly Ingredient ingredient;
        public readonly List<string> itemTypes = new(); // configured from ui, all items should be of same type from this list
        public readonly HashSet<int> materials = new(); // configured from ui. all items should be of same material from this list. -1 for any

        public readonly List<EcsEntity> items = new(); // selected before performing, cleared after performing

        public IngredientOrder(Ingredient ingredient) {
            this.ingredient = ingredient;
        }

        public IngredientOrder(IngredientOrder source) {
            ingredient = source.ingredient;
            itemTypes = new List<string>(source.itemTypes);
            materials = new HashSet<int>(source.materials);
        }
    }

    public enum CraftingOrderStatus {
        PERFORMING, // A, order has task
        WAITING, // W, order has no task
        PAUSED, // P, order paused by player
        // PAUSED_PROBLEM // PP, order paused by failed task (similar to PAUSED)
    }
}
}