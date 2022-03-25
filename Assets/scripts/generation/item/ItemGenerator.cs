using enums.item.type;
using enums.material;
using game.model;
using game.model.component;
using game.model.component.item;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.item {
    public class ItemGenerator {
        
        public EcsEntity generateItem(ItemData data, Vector3Int position, EcsEntity entity) =>
            generateItem(data.type, data.material, position, entity);

        public EcsEntity generateItem(string typeName, string materialName, Vector3Int position, EcsEntity itemEntity) =>
            generateItem(typeName, materialName, itemEntity)
                .Replace(new PositionComponent { position = position });

        // generates item without position
        public EcsEntity generateItem(string typeName, string materialName, EcsEntity entity) {
            ItemType type = ItemTypeMap.getItemType(typeName);
            if (type == null) Debug.LogError("Type " + typeName + " not found.");
            Material_ material = MaterialMap.get().material(materialName);
            entity.Replace(new ItemComponent {
                material = material.id, type = typeName, materialString = material.name, volume = 1,
                weight = material.density * 1
            });
            if (type.tool != null) {
                entity.Replace(new ItemToolComponent {action = type.tool.action});
            }
            entity.Replace(new NameComponent());
            return entity;
        }
    }
}