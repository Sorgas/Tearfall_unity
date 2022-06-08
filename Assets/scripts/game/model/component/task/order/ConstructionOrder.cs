using types;
using UnityEngine;

namespace game.model.component.task.order {
    public class ConstructionOrder : GenericBuildingOrder {
        public readonly BlockType blockType;

        public ConstructionOrder(BlockType blockType, string itemType, int material, int amount, Vector3Int position) :
            base(itemType, material, amount, position) {
            this.blockType = blockType;
        }
    }
}