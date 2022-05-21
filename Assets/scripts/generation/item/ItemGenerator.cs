using entity;
using enums;
using enums.item.type;
using enums.material;
using enums.plant;
using game.model;
using game.model.component;
using game.model.component.item;
using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.item {
    public class ItemGenerator {

        public EcsEntity generateItem(ItemData data, Vector3Int position, EcsEntity entity) =>
            generateItem(data.type, data.material, position, entity);

        public EcsEntity generateItem(string typeName, string materialName) =>
            generateItem(typeName, materialName, GameModel.get().createEntity());
        
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
                entity.Replace(new ItemToolComponent { action = type.tool.action });
            }
            if (type.aspects.ContainsKey(typeof(WearAspect))) {
                WearAspect aspect = (WearAspect)type.aspects[typeof(WearAspect)];
                entity.Replace(new ItemWearComponent { slot = aspect.slot, layer = aspect.layer });
            }
            entity.Replace(new NameComponent());
            return entity;
        }

        public EcsEntity generatePlantProduct(PlantBlock block) {
            if (block.blockType == PlantBlockTypeEnum.TRUNK.code) {
                return generateItem("log", "wood");
            }
            return EcsEntity.Null;
        }
    }
}