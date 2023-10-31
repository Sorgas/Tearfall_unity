using UnityEngine;

namespace types.item.recipe {
// Converts RawRecipe into Recipe
public class RecipeProcessor {
    private IngredientProcessor ingredientProcessor = new IngredientProcessor();

    public Recipe processRawRecipe(RawRecipe rawRecipe) {
        // Debug.Log("Processing recipe " + rawRecipe.name);
        Recipe recipe = new Recipe(rawRecipe);
        foreach (string ingredientString in rawRecipe.ingredients) {
            if (ingredientProcessor.validateIngredient(ingredientString)) {
                Ingredient ingredient = ingredientProcessor.parseIngredient(ingredientString);
                recipe.ingredients.add(ingredient.key, ingredient);
            }
        }
        return recipe;
    }
}
}