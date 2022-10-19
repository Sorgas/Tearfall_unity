using System.Collections.Generic;
using System.Linq;
using Assets.scripts.types.item.recipe;
using Leopotam.Ecs;

// order for crafting items in workbenches. Order is 'completed' when performed targetQuantity number of times.
public class CraftingOrder {
    public string name;
    public Recipe recipe;
    public CraftingOrderStatus status;
    public List<IngredientOrder> ingredients = new();
    public bool repeated;
    public bool paused;
    public int targetQuantity;
    public int performedQuantity;

    public CraftingOrder(Recipe recipe) {
        this.name = recipe.title;
        this.recipe = recipe;
        this.status = CraftingOrderStatus.WAITING;
        targetQuantity = 1;
        performedQuantity = 0;
    }

    public List<EcsEntity> allIngredientItems() {
        return ingredients.SelectMany(ingredient => ingredient.items).ToList();
    }

    public class IngredientOrder {
        public string key; // ingredient key from recipe
        public List<string> itemTypes = new(); // configured from ui
        public List<int> materials = new(); // configured from ui. if empty, tag is used
        public List<EcsEntity> items = new();
    }

    public enum CraftingOrderStatus {
        PERFORMING, // A
        WAITING, // W
        PAUSED, // P
        PAUSED_PROBLEM
    }
}