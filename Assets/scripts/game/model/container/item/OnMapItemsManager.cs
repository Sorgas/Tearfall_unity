using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;
using util.lang.extension;

namespace game.model.container.item {
// stores reference of positions to items in it. Puts and takes items from map, updating its entity's position component 
// items on map have position component, others have not.
public class OnMapItemsManager : ItemContainerPart {
    public readonly MultiValueDictionary<Vector3Int, EcsEntity> itemsOnMap = new(); // position -> items

    public OnMapItemsManager(LocalModel model, ItemContainer container) : base(model, container) { }

    public List<EcsEntity> getItems(Vector3Int position) {
        return itemsOnMap.ContainsKey(position) ? itemsOnMap[position] : new List<EcsEntity>();
    }

    // adds item without position to specified position on map (used in gameplay)
    public void putItemToMap(EcsEntity item, Vector3Int position) {
        validateForPlacing(item);
        item.Replace(new PositionComponent { position = position });
        container.availableItemsManager.add(item); // make item available
        itemsOnMap.add(item.pos(), item);
        addPositionForUpdate(item.pos());
    }

    // removes item from map
    public void takeItemFromMap(EcsEntity item) {
        validateForTaking(item);
        Vector3Int position = item.pos();
        item.Del<PositionComponent>();
        container.availableItemsManager.remove(item); // make item unavailable
        itemsOnMap.remove(position, item);
        addPositionForUpdate(position);
    }

    // validate that item is registered on its position
    private void validateForTaking(EcsEntity item) {
        if (!item.hasPos()) {
            Debug.LogError("Item " + item + " has no position.");
        }
        Vector3Int position = item.pos();
        if (!container.onMap.getItems(position).Contains(item)) {
            Debug.LogError("Item " + item + " is not registered by its position.");
        }
    }

    // validates that item has no position 
    private void validateForPlacing(EcsEntity item) {
        if (item.hasPos()) {
            Debug.LogError("Item " + item + " already has position.");
        }
    }
}
}