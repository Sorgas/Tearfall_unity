using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.container.item {
    public class ItemStateValidator : ItemContainerPart {
        public ItemStateValidator(LocalModel model, ItemContainer container) : base(model, container) { }

        // validate that item is registered on its position
        public void validateForTaking(EcsEntity item) {
            if (!item.hasPos()) {
                Debug.LogError("Item " + item + " has no position.");
            }
            Vector3Int position = item.pos();
            if (!container.onMap.getItems(position).Contains(item)) {
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