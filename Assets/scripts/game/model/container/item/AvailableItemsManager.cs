using System.Collections.Generic;
using game.model.component.item;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;
using util.lang.extension;

namespace game.model.container.item {
    // secondary registry for items
    // stores items by types and materials for faster searching
    // updated by calls from onMapItemsManager and ContainedItemsManager
    public class AvailableItemsManager {
        public readonly MultiValueDictionary<string, EcsEntity> byItemType = new();
        public readonly Dictionary<string, MultiValueDictionary<int, EcsEntity>> materials = new(); // itemType -> material -> entities
        private readonly MultiValueDictionary<int, EcsEntity> emptyMap = new();
        
        public void add(EcsEntity entity) {
            ItemComponent item = entity.take<ItemComponent>();
            byItemType.add(item.type, entity);
            if(!materials.ContainsKey(item.type)) materials.Add(item.type, new MultiValueDictionary<int, EcsEntity>());
            materials[item.type].add(item.material, entity);
            Debug.Log(item.type + " added as available");
        }

        public void remove(EcsEntity entity) {
            ItemComponent item = entity.take<ItemComponent>();
            byItemType.remove(item.type, entity);
            materials[item.type].remove(item.material, entity);
        }

        public MultiValueDictionary<int, EcsEntity> findByType(string typeName) {
            if (!materials.ContainsKey(typeName)) return emptyMap;
            return materials[typeName];
        }
    }
}