using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using enums.item;
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
    public class ItemFindingUtil : LocalMapModelComponent {
        private ItemContainer container;

        public ItemFindingUtil(ItemContainer container, LocalModel model) : base(model) {
            this.container = container;
        }

        public EcsEntity findFreeReachableItemBySelector(ItemSelector selector, Vector3Int pos) {
            LocalMap map = model.localMap;
            // get items and positions
            // TODO add items in containers
            if (container.onMap.all.Count < 0) return EcsEntity.Null;
            return container.onMap.all
                .Where(item => map.passageMap.inSameArea(pos, item.pos()))
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
        public List<EcsEntity> findForCraftingOrderIngredient(IngredientOrder ingredient, CraftingOrder order, Vector3Int performerPosition) {
            List<EcsEntity> otherItems = order.allIngredientItems(); // items selected in other ingredients should not be selected
            int requiredQuantity = order.recipe.ingredients[ingredient.key].quantity;
            foreach (string itemType in ingredient.itemTypes) {
                if (ingredient.materials.Count == 0) { // all items should have tag
                    ItemTagEnum tag = order.recipe.ingredients[ingredient.key].tag;
                    MultiValueDictionary<int, EcsEntity> itemsOfType = model.itemContainer.availableItemsManager.findByType(itemType);
                    foreach (KeyValuePair<int, List<EcsEntity>> entry in itemsOfType) {
                        List<EcsEntity> items = entry.Value
                            .Where(itemEntity => !itemEntity.Has<ItemLockedComponent>())
                            .Where(itemEntity => !otherItems.Contains(itemEntity))
                            .Where(itemEntity => itemEntity.take<ItemComponent>().tags.Contains(tag))
                            .Where(itemEntity => model.localMap.passageMap.inSameArea(itemEntity.pos(), performerPosition))
                            .ToList();
                        if (items.Count >= requiredQuantity) {
                            return selectNNearest(items, requiredQuantity, performerPosition);
                        }
                    }
                } else {
                    foreach (int material in ingredient.materials) {
                        List<EcsEntity> itemsOfTypeAndMaterial = model.itemContainer.availableItemsManager.findByTypeAndMaterial(itemType, material);
                        List<EcsEntity> items = itemsOfTypeAndMaterial
                            .Where(itemEntity => !itemEntity.Has<ItemLockedComponent>())
                            .Where(itemEntity => !otherItems.Contains(itemEntity))
                            .Where(itemEntity => model.localMap.passageMap.inSameArea(itemEntity.pos(), performerPosition))
                            .ToList();
                        if (items.Count >= requiredQuantity) {
                            return selectNNearest(items, requiredQuantity, performerPosition);
                        }
                    }
                }
            }
            return new List<EcsEntity>();
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
                    if(result.Count > quantity) result.RemoveAt(result.Count - 1);
                }
            }
            if(result.Count > quantity) Debug.LogError("wrong number of items taken.");
            return new List<EcsEntity>((IEnumerable<EcsEntity>) result.Keys);
        }

        private float distanceToItem(EcsEntity item, Vector3Int position) => Vector3Int.Distance(item.pos(), position);
    }
}