using game.model.component.task.order;
using types.item.recipe;
using static game.model.component.task.order.CraftingOrder;

namespace generation.item {
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
}