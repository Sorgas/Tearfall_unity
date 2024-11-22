using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.component.task.order;
using game.model.localmap;
using generation.item;
using Leopotam.Ecs;
using MoreLinq;
using types.item.recipe;
using UnityEngine;
using util.lang;
using util.lang.extension;

namespace game.model.container.item.finding {
public class CraftingItemFindingUtil : AbstractItemFindingUtil {
    private readonly SimpleCraftingCheckingUtil simpleUtil;
    private bool debug = true;
    
    public CraftingItemFindingUtil(LocalModel model, ItemContainer container) : base(model, container) {
        simpleUtil = new SimpleCraftingCheckingUtil { model = model };
    }

    public List<IngredientOrder> findInvalidIngredientOrders(AbstractItemConsumingOrder order, Vector3Int position) => 
        findInvalidIngredientOrders(order, position, EcsEntity.Null);

    public List<IngredientOrder> findInvalidIngredientOrders(AbstractItemConsumingOrder order, Vector3Int position, EcsEntity lookupContainer) {
        return order.ingredients.Values.Where(ingredient => !ingredientOrderValid(ingredient, position, lookupContainer)).ToList();
    }

    // finds items for all ingredient orders, does not save them to order,
    // items already saved in order not taken into account 
    public bool checkItemsForOrder(AbstractItemConsumingOrder order, Vector3Int position) {
        List<EcsEntity> foundItems = new();
        foreach (var ingredientOrder in order.ingredients.Values) {
            List<EcsEntity> items = findItemsForIngredient(ingredientOrder, position, foundItems);
            if (items == null) return false;
            foundItems.AddRange(items);
        }
        return true;
    }
    
    // ingredient order is valid if
    //      it has correct number of items selected
    //      all items have allowed type and material
    //      all items have same type and material
    //      all items are reachable from reference position
    private bool ingredientOrderValid(IngredientOrder order, Vector3Int position, EcsEntity lookupContainer) {
        if (!order.hasEnoughItems()) return false;
        ItemComponent firstItem = order.items[0].take<ItemComponent>();
        if (!order.selected.contains(firstItem.type, firstItem.material)) return false;
        foreach (var item in order.items) {
            ItemComponent itemComponent = item.take<ItemComponent>();
            if (itemComponent.material != firstItem.material || itemComponent.type != firstItem.type) return false;
        }
        if (lookupContainer != EcsEntity.Null) {
            return order.items.All(item => (item.Has<ItemContainedComponent>() && item.take<ItemContainedComponent>().container.Equals(lookupContainer))
                                           || model.itemContainer.itemAccessibleFromPosition(item, position));   
        }
        return order.items.All(item => model.itemContainer.itemAccessibleFromPosition(item, position));
    }

    public List<EcsEntity> findItemsForIngredient(IngredientOrder ingredientOrder, AbstractItemConsumingOrder order, Vector3Int position) {
        return findItemsForIngredient(ingredientOrder, position, order.allIngredientItems());
    }

    // finds items for ingredient order accessible from position
    private List<EcsEntity> findItemsForIngredient(IngredientOrder ingredientOrder, Vector3Int position, List<EcsEntity> blockedItems) {
        List<ItemGroup> groups = findItemsForIngredientByMaterial(ingredientOrder);
        groups = groups
            .Where(group => group.items.Count > 0)
            .Select(group => {
                string type = group.items[0].take<ItemComponent>().type;
                int quantity = ingredientOrder.quantities[type];
                return filterForCrafting(group, blockedItems, position, quantity);
            })
            .Where(group => group != null)
            .ToList();
        return selectNearestGroup(groups)?.items;
    }

    // finds groups of items suitable for ingredient order which has material list. Does not check item quantity
    private List<ItemGroup> findItemsForIngredientByMaterial(IngredientOrder ingredientOrder) {
        List<ItemGroup> result = new();
        foreach (var itemType in ingredientOrder.selected.Keys) {
            foreach (int material in ingredientOrder.selected[itemType]) {
                IEnumerable<EcsEntity> items = model.itemContainer.availableItemsManager.findByTypeAndMaterial(itemType, material);
                result.Add(new ItemGroup(items));
            }
        }
        return result;
    }

    // items for crafting should be not locked, be in same area with performer, and not already selected for order
    // Passing Vector3Int.back as position disables accessibility check
    private ItemGroup filterForCrafting(ItemGroup group, List<EcsEntity> prohibitedItems, Vector3Int position, int requiredQuantity) {
        if (group.items.Count < requiredQuantity) return null;
        group.items = group.items
            .Where(itemEntity => !itemEntity.Has<LockedComponent>()) // not locked for any task
            .Where(itemEntity => !prohibitedItems.Contains(itemEntity)) // not in block list
            .ToList();
        if (group.items.Count < requiredQuantity) return null;
        if (position != Vector3Int.back) {
            group.items = group.items.ToDictionary(item => item, container.getItemAccessPosition) // filter accessible
                .Where(pair => model.itemContainer.itemAccessibleFromPosition(pair.Key, position))
                .Select(pair => pair.Key).ToList();
        }
        if (group.items.Count < requiredQuantity) return null;
        return selectNNearest(group, requiredQuantity, position);
    }

    // TODO use no-sorting solution
    // removes from item group all items except nearest [quantity] items
    private ItemGroup selectNNearest(ItemGroup group, int quantity, Vector3Int position) {
        List<KeyValuePair<EcsEntity, float>> pairs = group.items
            .Select(item => new KeyValuePair<EcsEntity, float>(item, fastDistanceToItem(item, position)))
            .PartialSortBy(quantity, pair => pair.Value).ToList();
        group.items.Clear();
        MoreEnumerable.ForEach(pairs, pair => {
            group.items.Add(pair.Key);
            group.totalDistance += pair.Value;
        });
        if (group.items.Count != quantity) Debug.LogError("wrong number of items taken.");
        return group;
    }

    private ItemGroup selectNearestGroup(List<ItemGroup> groups) {
        if (groups.Count == 0) return null;
        if (groups.Count == 1) return groups[0];
        return groups.MinBy(group => group.totalDistance);
    }

    private class ItemGroup {
        public List<EcsEntity> items = new();
        public float totalDistance = -1;

        public ItemGroup(IEnumerable<EcsEntity> items) {
            this.items.AddRange(items);
        }
    }

    private class SimpleCraftingCheckingUtil {
        public LocalModel model;

        // simplified check 
        public bool checkItemsForOrder(CraftingOrder order) {
            foreach (var ingredientOrder in order.ingredients.Values) {
                if (!checkItemsForIngredient(ingredientOrder)) return false;
            }
            return true;
        }

        // checks that there are available items for ingredient
        private bool checkItemsForIngredient(IngredientOrder ingredientOrder) => checkItemsForIngredientByMaterial(ingredientOrder);

        private bool checkItemsForIngredientByMaterial(IngredientOrder ingredientOrder) {
            foreach (string itemType in ingredientOrder.selected.Keys) {
                foreach (int material in ingredientOrder.selected[itemType]) {
                    IEnumerable<EcsEntity> items = model.itemContainer.availableItemsManager.findByTypeAndMaterial(itemType, material);
                    if (items.Count() >= ingredientOrder.quantities[itemType]) return true;
                }
            }
            return false;
        }
    }

    private void log(string message) {
        if(debug) Debug.Log($"[CraftingItemFindingUtil]: {message}");
    }
}
}