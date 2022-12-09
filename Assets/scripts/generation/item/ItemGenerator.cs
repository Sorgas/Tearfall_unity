using System;
using System.Collections.Generic;
using System.Linq;
using enums.item;
using enums.item.type;
using enums.plant;
using game.model.component;
using game.model.component.item;
using game.model.component.plant;
using Leopotam.Ecs;
using types.material;
using UnityEngine;

namespace generation.item {
    public class ItemGenerator {

        public EcsEntity generateItem(ItemData data, Vector3Int position, EcsEntity entity) =>
            generateItem(data.type, data.material, position, entity);

        public EcsEntity generateItem(string typeName, string materialName, Vector3Int position, EcsEntity itemEntity) =>
            generateItem(typeName, materialName, itemEntity)
                .Replace(new PositionComponent { position = position });

        public EcsEntity generateItem(string typeName, int material, EcsEntity entity) =>
            generateItem(typeName, MaterialMap.get().material(material).name, entity);

        // generates item without position
        public EcsEntity generateItem(string typeName, string materialName, EcsEntity entity) {
            ItemType type = ItemTypeMap.getItemType(typeName);
            if (type == null) Debug.LogError("Type " + typeName + " not found.");
            Material_ material = MaterialMap.get().material(materialName);
            List<ItemTagEnum> tags = material.tags.Select(tag => (ItemTagEnum)Enum.Parse(typeof(ItemTagEnum), tag, true)).ToList();
            entity.Replace(new ItemComponent {
                material = material.id,
                type = typeName,
                materialString = material.name,
                volume = 1,
                weight = material.density * 1,
                tags = new(tags)
            });
            if (type.tool != null) {
                entity.Replace(new ItemToolComponent { action = type.tool.action });
            }
            entity.Replace(new NameComponent { name = material.name + " " + type.title });
            addComponentsFromType(type, ref entity);
            return entity;
        }

        public EcsEntity generatePlantProduct(PlantBlock block, EcsEntity entity) {
            if (block.blockType == PlantBlockTypeEnum.TRUNK.code) {
                return generateItem("log", "wood", entity);
            }
            return EcsEntity.Null;
        }

        private void addComponentsFromType(ItemType type, ref EcsEntity item) {
            if (type.components.ContainsKey("wear")) {
                string[] args = type.components["wear"];
                item.Replace(new ItemWearComponent { slot = args[0], layer = args[1] });
                Debug.Log("wear component added to item");
            }
            if(type.components.ContainsKey("food")) {
                string[] args = type.components["food"];
                item.Replace(new ItemFoodComponent { nutrition = float.Parse(args[0])
                // , foodQuality = int.Parse(args[1]) 
                });
            }
        }
    }
}