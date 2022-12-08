using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using enums.item;
using game.model.component;
using game.model.component.item;
using game.model.localmap;
using Leopotam.Ecs;
using types.building;
using UnityEngine;
using util.item;
using util.lang;
using util.lang.extension;
using static CraftingOrder;

namespace game.model.container.item {
    public class ItemFindingUtil : ItemContainerPart {
        public ItemFindingUtil(LocalModel model, ItemContainer container) : base(model, container) { }

        // returns item matching itemSelector and available from position
        public EcsEntity findFreeReachableItemBySelector(ItemSelector selector, Vector3Int pos) {
            LocalMap map = model.localMap;
            // get items and positions
            // TODO add items in containers
            return container.availableItemsManager.getAll()
                .Where(item => map.passageMap.inSameArea(pos, item.pos()))
                .Where(item => !item.Has<LockedComponent>())
                .Where(selector.checkItem)
                .DefaultIfEmpty(EcsEntity.Null)
                .Aggregate((cur, item) => cur == EcsEntity.Null || (distanceToItem(item, pos) < distanceToItem(cur, pos))
                    ? item
                    : cur); // select nearest
        }

        // returns variant -> material -> list of items 
        public Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> findForBuildingVariants(BuildingVariant[] variants) {
            Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> resultMap = new();
            foreach (BuildingVariant variant in variants) {
                MultiValueDictionary<int, EcsEntity> map = container.availableItemsManager.findByType(variant.itemType);
                map.removeByPredicate(entities => entities.Count < variant.amount);
                resultMap.Add(variant, map);
            }
            return resultMap;
        }

        // checks if there are at least one option to build construction
        public bool enoughForBuilding(BuildingVariant[] variants) {
            foreach (BuildingVariant variant in variants) {
                if (container.availableItemsManager.findByType(variant.itemType).Values
                    .Any(items => items.Count > variant.amount)) return true;
            }
            return false;
        }

        // finds items for crafting
        public List<EcsEntity> findForIngredientOrder(IngredientOrder ingredient, CraftingOrder order, Vector3Int position) {
            log("searching items for ingredient " + ingredient.key + " of order " + order.name);
            List<EcsEntity> otherItems = order.allIngredientItems(); // items selected in other ingredients should not be selected
            int requiredQuantity = order.recipe.ingredients[ingredient.key].quantity;
            if (ingredient.materials.Count == 0) { // all items should have tag
                return findHavingTag(ingredient, order, position, otherItems);
            } else {
                return findForCraftingByMaterial(ingredient, order, position, otherItems);
            }
        }

        private List<EcsEntity> findHavingTag(IngredientOrder ingredient, CraftingOrder order, Vector3Int position, List<EcsEntity> otherItems) {
            ItemTagEnum tag = order.recipe.ingredients[ingredient.key].tag;
            int requiredQuantity = order.recipe.ingredients[ingredient.key].quantity;
            foreach (string itemType in ingredient.itemTypes) {
                MultiValueDictionary<int, EcsEntity> itemsOfType = model.itemContainer.availableItemsManager.findByType(itemType);
                foreach (KeyValuePair<int, List<EcsEntity>> entry in itemsOfType) { // for each material
                    log("searching " + itemType + " material: " + entry.Key);
                    List<EcsEntity> items = filterForCrafting(entry.Value, otherItems, position, requiredQuantity, tag);
                    if (items.Count > 0) return items;
                }
            }
            return new List<EcsEntity>();
        }

        private List<EcsEntity> findForCraftingByMaterial(IngredientOrder ingredient, CraftingOrder order, Vector3Int position, List<EcsEntity> otherItems) {
            int requiredQuantity = order.recipe.ingredients[ingredient.key].quantity;
            foreach (string itemType in ingredient.itemTypes) {
                foreach (int material in ingredient.materials) {
                    log("searching " + itemType + " material: " + material);
                    List<EcsEntity> items = filterForCrafting(model.itemContainer.availableItemsManager.findByTypeAndMaterial(itemType, material),
                            otherItems, position, requiredQuantity, ItemTagEnum.NULL);
                    if (items.Count > 0) return items;
                }
            }
            return new List<EcsEntity>();
        }

        public EcsEntity findFoodItem(Vector3Int position) {
            List<EcsEntity> list = container.availableItemsManager.getAll()
                .Where(item => item.Has<ItemFoodComponent>())
                .ToList();
            if (list.Count == 0) return EcsEntity.Null;
            return selectNearest(list, position);
        }

        // items for crafting should be not locked, in same area with performer, and not already selected for order
        private List<EcsEntity> filterForCrafting(List<EcsEntity> source, List<EcsEntity> otherItems, Vector3Int position, int requiredQuantity, ItemTagEnum tag) {
            IEnumerable<EcsEntity> stream = source
                .Where(itemEntity => !itemEntity.Has<LockedComponent>())
                .Where(itemEntity => !otherItems.Contains(itemEntity));
            if (tag != ItemTagEnum.NULL) {
                stream = stream.Where(itemEntity => itemEntity.take<ItemComponent>().tags.Contains(tag));
            }
            List<EcsEntity> items = stream.Where(itemEntity => model.localMap.passageMap.inSameArea(itemEntity.pos(), position)).ToList();
            return items.Count >= requiredQuantity
                ? selectNNearest(items, requiredQuantity, position)
                : new List<EcsEntity>();
        }

        private List<EcsEntity> selectNNearest(List<EcsEntity> items, int quantity, Vector3Int position) {
            OrderedDictionary result = new();
            foreach (EcsEntity item in items) {
                float distance = distanceToItem(item, position);
                if (result.Count < quantity || distance < ((float)result[quantity - 1])) {
                    int index = 0;
                    for (int i = 0; i < result.Count; i++) {
                        if (((float)result[i]) > distance) {
                            index = i;
                            break;
                        }
                    }
                    result.Insert(index, item, distance);
                    if (result.Count > quantity) result.RemoveAt(result.Count - 1);
                }
            }
            if (result.Count > quantity) Debug.LogError("wrong number of items taken.");
            return (from EcsEntity mItem in result.Keys select mItem).ToList();
        }

        private EcsEntity selectNearest(List<EcsEntity> items, Vector3Int position) {
            float minDistance = -1;
            EcsEntity result = EcsEntity.Null;
            foreach (EcsEntity item in items) {
                float distance = distanceToItem(item, position);
                if (distance < minDistance || minDistance == -1) {
                    result = item;
                    minDistance = distance;
                }
            }
            return result;
        }

        private float distanceToItem(EcsEntity item, Vector3Int position) => Vector3Int.Distance(item.pos(), position);

        private void log(string message) => Debug.Log("[ItemFindingUtil]: " + message);
    }
}