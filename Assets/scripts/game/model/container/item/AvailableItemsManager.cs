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
    // does not consider locked items
    public class AvailableItemsManager {
        private readonly MultiValueDictionary<string, EcsEntity> byType = new(); // itemType -> entities
        private readonly Dictionary<string, MultiValueDictionary<int, EcsEntity>> byTypeAndMaterial = new(); // itemType -> material -> entities
        private readonly MultiValueDictionary<int, EcsEntity> emptyMap = new();
        private readonly List<EcsEntity> emptyList = new();

        public void add(EcsEntity entity) {
            ItemComponent item = entity.take<ItemComponent>();
            byType.add(item.type, entity);
            if (!byTypeAndMaterial.ContainsKey(item.type)) byTypeAndMaterial.Add(item.type, new MultiValueDictionary<int, EcsEntity>());
            byTypeAndMaterial[item.type].add(item.material, entity);
            Debug.Log(item.type + " added as available");
        }

        public void remove(EcsEntity entity) {
            ItemComponent item = entity.take<ItemComponent>();
            byType.remove(item.type, entity);
            byTypeAndMaterial[item.type].remove(item.material, entity);
        }

        public MultiValueDictionary<int, EcsEntity> findByType(string type) {
            if (!byTypeAndMaterial.ContainsKey(type)) return emptyMap.clone();
            return byTypeAndMaterial[type].clone();
        }

        public List<EcsEntity> findByTypeAndMaterial(string type, int material) {
            if (!byTypeAndMaterial.ContainsKey(type) || !byTypeAndMaterial[type].ContainsKey(material)) return new List<EcsEntity>();
            return new List<EcsEntity>(byTypeAndMaterial[type][material]);
        }

        public List<EcsEntity> findNearest(string type, int material, int number, Vector3Int position) {
            if (!byTypeAndMaterial.ContainsKey(type) || !byTypeAndMaterial[type].ContainsKey(material)) return new List<EcsEntity>();
            List<EcsEntity> allItems = new(byTypeAndMaterial[type].get(material));
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