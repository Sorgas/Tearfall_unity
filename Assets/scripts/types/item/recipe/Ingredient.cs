using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using types.material;

namespace types.item.recipe {
    // describes selection of ingredient items for crafting.
    // Any combination of item types and materials allowed. all items should be of same type and material
    public class Ingredient {
        public string key;                              // item/building part name, 'consumed', 'main'
        public List<string> itemTypes;                  // acceptable item types
        public string tag;                              // acceptable item tag
        // public List<string> materials;               // acceptable materials TODO allow defining specific materials
        public int quantity;                            // number of items

        public List<int> allowedMaterials = new();   // -1 for any
        
        public Ingredient(string key, List<string> itemTypes, string tag, int quantity) {
            this.key = key;
            this.itemTypes = new(itemTypes);
            this.tag = tag;
            this.quantity = quantity;
            allowedMaterials.AddRange(MaterialMap.get().getByTag(tag).Select(material => material.id));
        }

        public bool checkItem(ItemComponent item) {
            return item.tags.Contains(tag) && itemTypes.Contains(item.type);
        }
    }
}