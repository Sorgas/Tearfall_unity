using System.Collections.Generic;

namespace types.item.recipe {
// Describes selection of ingredient items for crafting.
// Can specify sets of item types and materials. Any combination of item type and material allowed. All items should be of same type and material
// TODO add item tags to filter items?
public class Ingredient {
    public readonly string key; // item/building part name, 'consumed', 'main'
    public readonly List<string> itemTypes; // acceptable item types, empty for any
    public readonly List<int> materials; // acceptable materials, empty for any
    public readonly int quantity; // number of items
    // TODO add amount component to material items and use target amount in ingredient definition.

    public Ingredient(string key, List<string> itemTypes, List<int> materials, int quantity) {
        this.key = key;
        this.itemTypes = itemTypes;
        this.materials = materials;
        this.quantity = quantity;
    }

    public string toString() {
        return $"Ingredient:{key}, [{string.Join(",", itemTypes)}], [{string.Join(",", materials)}], {quantity}";
    }
}
}