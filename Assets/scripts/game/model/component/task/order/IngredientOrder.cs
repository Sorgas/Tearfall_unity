using System;
using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using Leopotam.Ecs;
using types.item.recipe;
using util.lang;
using util.lang.extension;

namespace game.model.component.task.order {
// Configurable order for selecting items for one ingredient of crafting recipe or building type.
// Supports selecting item type and material combinations from different Ingredients
public class IngredientOrder : ICloneable {
    public readonly string key;
    public readonly List<Ingredient> ingredients = new(); // 'variants'
    // configured from ui, maps allowed item types to corresponding allowed materials. Only combinations described in ingredients are allowed.
    public readonly MultiValueDictionary<string, int> selected = new();
    public readonly Dictionary<string, int> quantities = new(); // item type -> number of items
    // Selected before performing, cleared after performing. All selected items should have same type and material
    public readonly List<EcsEntity> items = new();

    public IngredientOrder(string key) {
        this.key = key;
    }

    public IngredientOrder(IngredientOrder source) {
        key = source.key;
        ingredients = source.ingredients; // Ingredient objects are effectively immutable. Set of Ingredient does not change 
        selected = source.selected.clone();
        quantities = source.quantities.ToDictionary(entry => entry.Key, entry => entry.Value);
    }

    public bool hasEnoughItems() => items.Count > 0 && quantities[items[0].take<ItemComponent>().type] == items.Count;

    public object Clone() => new IngredientOrder(this);
}
}