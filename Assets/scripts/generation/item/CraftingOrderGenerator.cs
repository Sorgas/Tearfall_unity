
using Assets.scripts.types.item.recipe;
using static CraftingOrder;

class CraftingOrderGenerator {

    public CraftingOrder generate(Recipe recipe) {
        CraftingOrder order = new(recipe);
        foreach (Ingredient ingredient in recipe.ingredients.Values) {
            order.ingredients.Add(createIngredientOrder(ingredient));   
        }
        return order;
    }

    private IngredientOrder createIngredientOrder(Ingredient ingredient) {
        IngredientOrder order = new(ingredient);
        return order;
    }
}