using System.Linq;
using game.model.component.building;
using game.model.component.task.order;
using MoreLinq;
using types.item.recipe;
using types.material;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.order.CraftingOrder;

namespace generation.item {
class CraftingOrderGenerator {
    // TODO use material preferences from workbench to fill materials in order
    // creates default crafting order by recipe. Order can be configured from workbench ui
    public CraftingOrder generate(Recipe recipe, WorkbenchComponent workbench) {
        CraftingOrder order = new(recipe);
        foreach (Ingredient ingredient in recipe.ingredients.Values) {
            order.ingredients.Add(createIngredientOrder(ingredient));
        }
        return order;
    }

    private IngredientOrder createIngredientOrder(Ingredient ingredient) {
        IngredientOrder order = new(ingredient);
        order.itemTypes.AddRange(ingredient.itemTypes);
        if (ingredient.tag != null) {
            MaterialMap.get().getByTag(ingredient.tag)
            .Select(material => material.id)
            .ForEach(materialId => order.materials.Add(materialId));
        } else {
            Debug.LogError("No tag for ingredient " + ingredient.key);
        }
        return order;
    }
}
}