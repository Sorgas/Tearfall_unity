using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.container.item {
    public class ItemStateValidator : ItemContainerPart {
        public ItemStateValidator(ItemContainer container) : base(container) { }
        
        // validate that item is registered on its position
        public void validateForTaking(EcsEntity item) {
            if (!item.hasPos()) {
                Debug.LogError("Item " + item + " has no position.");
            }
            Vector3Int position = item.pos();
            if (!container.onMapItems.itemsOnMap.ContainsKey(position)) {
                Debug.LogError("Tile on " + item + " position is empty.");
            }
            if (!container.onMapItems.itemsOnMap[position].Contains(item)) {
                Debug.LogError("Item " + item + " is not registered by its position.");
            }
        }

        // validates that item has no position 
        public void validateForPlacing(EcsEntity item) {
            if (item.hasPos()) {
                Debug.LogError("Item " + item + " already has position.");
            }
        }

    }
}