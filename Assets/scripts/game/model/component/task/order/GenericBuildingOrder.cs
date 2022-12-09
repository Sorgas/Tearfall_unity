using UnityEngine;

namespace game.model.component.task.order {
    
    // order for construction and building. defines number of items of 1 material and item type
    // TODO add multiple ingredients, merge with crafting order
    public class GenericBuildingOrder {
        public readonly string itemType;
        public readonly int material;
        public readonly int amount;
        public readonly Vector3Int position;

        public GenericBuildingOrder(string itemType, int material, int amount, Vector3Int position) {
            this.itemType = itemType;
            this.material = material;
            this.amount = amount;
            this.position = position;
        }
    }
}