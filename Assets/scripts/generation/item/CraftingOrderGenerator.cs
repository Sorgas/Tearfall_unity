using System.Collections.Generic;
using game.model.component.task.order;
using types;
using types.building;
using types.item.recipe;
using UnityEngine;
using util.lang;

namespace generation.item {
public class CraftingOrderGenerator {
    public CraftingOrder generate(string recipeName) => generate(RecipeMap.get().get(recipeName));

    // TODO use material preferences from workbench to fill materials in order
    // creates default crafting order by recipe. Order can be configured from workbench ui
    public CraftingOrder generate(Recipe recipe) {
        CraftingOrder order = new(recipe);
        fillIngredientOrders(recipe.ingredients, order);
        return order;
    }

    public ConstructionOrder generateConstructionOrder(ConstructionType type) => generateConstructionOrder(type, Vector3Int.zero);

    public ConstructionOrder generateConstructionOrder(ConstructionType type, Vector3Int position) {
        ConstructionOrder order = new(type.blockType, position);
        fillIngredientOrders(type.ingredients, order);
        return order;
    }

    public BuildingOrder generateBuildingOrder(BuildingType type) => generateBuildingOrder(type, Vector3Int.zero, Orientations.N);

    public BuildingOrder generateBuildingOrder(BuildingType type, Vector3Int position, Orientations orientation) {
        BuildingOrder order = new(Vector3Int.zero, type, orientation);
        fillIngredientOrders(type.ingredients, order);
        return order;
    }

    // ingredients for same key are combined by single ingredient order, to represent 'variants'.
    private void fillIngredientOrders(MultiValueDictionary<string, Ingredient> ingredients, AbstractItemConsumingOrder order) {
        foreach (string key in ingredients.Keys) {
            IngredientOrder ingredientOrder = new(key);
            
            foreach (Ingredient ingredient in ingredients[key]) { // combine multiple ingredient variants in one ingredient order
                ingredientOrder.ingredients.Add(ingredient);
                foreach (var type in ingredient.itemTypes) {
                    ingredientOrder.quantities.Add(type, ingredient.quantity);
                    ingredientOrder.selected.Add(type, new List<int>(ingredient.materials)); // all combinations are selected by default
                }
            }
            order.ingredients.Add(key, ingredientOrder);
        }
    }
}
}