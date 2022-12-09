using UnityEngine;

namespace Assets.scripts.types.item.recipe {
    public class RecipeProcessor {
        private IngredientProcessor ingredientProcessor = new IngredientProcessor();

        public Recipe processRawRecipe(RawRecipe rawRecipe) {
            Debug.Log("Processing recipe " + rawRecipe.name);
            Recipe recipe = new Recipe(rawRecipe);
            foreach(string ingredientString in rawRecipe.ingredients) {
                if(ingredientProcessor.validateIngredient(ingredientString)) {
                    Ingredient ingredient = ingredientProcessor.parseIngredient(ingredientString);
                    recipe.ingredients.Add(ingredient.key, ingredient);
                }
            }
            //TODO add combine recipes
            // combine recipes should specify all required item parts
            // if (recipe.ingredients.keySet().contains("main") || validateCombineRecipe(recipe)) return recipe;
            // return null;
            return recipe;
        }

        // private boolean validateCombineRecipe(Recipe recipe) {
        //     ItemType type = ItemTypeMap.instance().getItemType(recipe.newType);
        //     if (type == null)
        //         return Logger.LOADING.logWarn("Recipe " + recipe.name + " has invalid item type " + recipe.newType, false);
        //     if (!recipe.ingredients.keySet().containsAll(type.requiredParts))
        //         return Logger.LOADING.logWarn("Recipe " + recipe.name + " specifies not all required parts of type " + type.name, false);
        //     return true;
        // }
    }
}