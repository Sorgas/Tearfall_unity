using System.Collections.Generic;
using game.model.component.item;

namespace types.item.recipe {
    public class Ingredient {
        public string key;                     // item/building part name, 'consumed', 'main'
        public List<string> itemTypes;         // acceptable item types
        public string tag;                // acceptable item tags
        public int quantity;                   // number of items

        public Ingredient(string key, List<string> itemTypes, string tag, int quantity) {
            this.key = key;
            this.itemTypes = new(itemTypes);
            this.tag = tag;
            this.quantity = quantity;
        }

        public bool checkItem(ItemComponent item) {
            return item.tags.Contains(tag) && itemTypes.Contains(item.type);
        }
    }
}