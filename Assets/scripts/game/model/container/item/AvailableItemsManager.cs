using System.Collections.Generic;
using System.Linq;
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

        public List<EcsEntity> findNearest(string type, int material, int number, Vector3Int position) {
            if (!materials.ContainsKey(type) || !materials[type].ContainsKey(material)) return new List<EcsEntity>();
            List<EcsEntity> allItems = new(materials[type].get(material));
            MultiValueDictionary<float, EcsEntity> distances = new();
            foreach (EcsEntity ecsEntity in allItems) {
                Vector3Int itemPosition = ecsEntity.pos();
                // TODO handle contained items
                distances.add(Vector3Int.Distance(position, itemPosition), ecsEntity);
            }
            return distances.Keys
                .OrderBy(value => value).ToList()
                .SelectMany(value => distances[value]).ToList()
                .GetRange(0, number);
        }
    }
}