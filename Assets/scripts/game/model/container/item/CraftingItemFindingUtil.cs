using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.component.task.order;
using game.model.localmap;
using Leopotam.Ecs;
using MoreLinq;
using UnityEngine;
using util.lang;
using util.lang.extension;

namespace game.model.container.item {
public class CraftingItemFindingUtil : ItemContainerPart {
    // TODO rewrite stockpile method to use item selectors. selector should be stored in stockpile component and updated from stockpile config menu.
    public CraftingItemFindingUtil(LocalModel model, ItemContainer container) : base(model, container) { }

    // finds items for crafting
    public IList<EcsEntity> findForIngredientOrder(CraftingOrder.IngredientOrder ingredientOrder, CraftingOrder order, Vector3Int position) {
        // log("searching items for ingredient " + ingredient.key + " of order " + order.name);
        List<EcsEntity> otherItems = order.allIngredientItems(); // items selected in other ingredients should not be selected
        List<List<EcsEntity>> itemLists = ingredientOrder.materials.Count == 0 // get groups of suitable items
            ? findItemsForIngredientWithTag(ingredientOrder)
            : findItemsForIngredientByMaterial(ingredientOrder);
        List<ItemGroup> groups = itemLists
            .Select(list => filterForCrafting(list, otherItems, position, ingredientOrder.ingredient.quantity))
            .Where(group => group != null)
            .ToList();
        return selectNearestGroup(groups)?.items;
    }

    // finds groups of items suitable for ingredient order which has tag
    private List<List<EcsEntity>> findItemsForIngredientWithTag(CraftingOrder.IngredientOrder ingredientOrder) {
        string tag = ingredientOrder.ingredient.tag;
        List<List<EcsEntity>> result = new();
        foreach (string itemType in ingredientOrder.itemTypes) {
            MultiValueDictionary<int, EcsEntity> itemsOfType = model.itemContainer.availableItemsManager.findByType(itemType); // material -> items
            foreach (KeyValuePair<int, List<EcsEntity>> entry in itemsOfType) { // for each material
                result.Add(entry.Value.Where(item => item.take<ItemComponent>().tags.Contains(tag)).ToList()); // filter by tag
            }
        }
        return result;
    }

    // finds groups of items suitable for ingredient order which has material list
    private List<List<EcsEntity>> findItemsForIngredientByMaterial(CraftingOrder.IngredientOrder ingredientOrder) {
        List<List<EcsEntity>> result = new();
        foreach (string itemType in ingredientOrder.itemTypes) {
            foreach (int material in ingredientOrder.materials) {
                result.Add(model.itemContainer.availableItemsManager.findByTypeAndMaterial(itemType, material));
            }
        }
        return result;
    }

    // items for crafting should be not locked, be in same area with performer, and not already selected for order
    private ItemGroup filterForCrafting(IList<EcsEntity> items, List<EcsEntity> prohibitedItems, Vector3Int position, int requiredQuantity) {
        if (items.Count < requiredQuantity) return null;
        items = items
            .Where(itemEntity => !itemEntity.Has<LockedComponent>()) // not locked for any task
            .Where(itemEntity => !prohibitedItems.Contains(itemEntity)) // not in block list
            .ToList();
        if (items.Count < requiredQuantity) return null;
            // map to position
        Dictionary<EcsEntity, Vector3Int> itemsToPosition = items.ToDictionary(item => item, container.getItemAccessPosition);
        List<EcsEntity> localItems = itemsToPosition // filter accessible
            .Where(pair => model.itemContainer.itemAccessibleFromPosition(pair.Key, position))
            .Select(pair => pair.Key).ToList();
        if (localItems.Count < requiredQuantity) return null;
        return selectNNearest(localItems, requiredQuantity, position);
    }

    private ItemGroup selectNNearest(List<EcsEntity> items, int quantity, Vector3Int position) {
        // TODO use no-sorting solution
        ItemGroup group = new ItemGroup();
        List<KeyValuePair<EcsEntity, float>> pairs = items
            .Select(item => new KeyValuePair<EcsEntity, float>(item, fastDistance(item, position)))
            .PartialSortBy(quantity, pair => pair.Value).ToList();
        MoreEnumerable.ForEach(pairs, pair => {
            group.items.Add(pair.Key);
            group.totalDistance += pair.Value;
        });
        if (group.items.Count > quantity) Debug.LogError("wrong number of items taken.");
        return group;
    }

    private ItemGroup selectNearestGroup(List<ItemGroup> groups) {
        log("selecting nearest from " + groups.Count + " groups");
        if (groups.Count == 0) return null;
        if (groups.Count == 1) return groups[0];
        return groups.MinBy(group => group.totalDistance);
    }

    private EcsEntity selectNearest(List<EcsEntity> items, Vector3Int position) {
        float minDistance = -1;
        EcsEntity result = EcsEntity.Null;
        foreach (EcsEntity item in items) {
            float distance = fastDistance(item, position);
            if (distance == 0) return item;
            if (distance < minDistance || minDistance < 0) {
                result = item;
                minDistance = distance;
            }
        }
        return result;
    }

    private float fastDistance(EcsEntity item, Vector3Int position) {
        return (container.getItemAccessPosition(item) - position).sqrMagnitude;
    }
    
    private void log(string message) => Debug.Log("[ItemFindingUtil]: " + message);

    private class ItemGroup {
        public List<EcsEntity> items = new();
        public float totalDistance = 0;
    }
}
}