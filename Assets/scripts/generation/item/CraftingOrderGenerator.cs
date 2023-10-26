using System.Collections.Generic;
using game.model.component.task.order;
using types.item.recipe;
using static game.model.component.task.order.CraftingOrder;

namespace generation.item {
class CraftingOrderGenerator {

    public CraftingOrder generate(string recipeName) => generate(RecipeMap.get().get(recipeName));

    // TODO use material preferences from workbench to fill materials in order
    // creates default crafting order by recipe. Order can be configured from workbench ui
    public CraftingOrder generate(Recipe recipe) {
        CraftingOrder order = new(recipe);
        foreach (string key in recipe.ingredients.Keys) {
            order.ingredients.Add(createIngredientOrder(recipe, key));
        }
        return order;
    }

    private IngredientOrder createIngredientOrder(Recipe recipe, string key) {
        IngredientOrder order = new();
        foreach (Ingredient ingredient in recipe.ingredients[key]) { // combine multiple ingredient variants in one ingredient order
            order.ingredients.Add(ingredient);
            foreach (var type in ingredient.itemTypes) {
                order.quantities.Add(type, ingredient.quantity);
                order.selected.Add(type, new List<int>(ingredient.materials)); // all combinations are selected by default
            }
        }
        return order;
    }
}
}