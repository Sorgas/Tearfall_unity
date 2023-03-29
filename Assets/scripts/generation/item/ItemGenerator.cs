using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.component.plant;
using Leopotam.Ecs;
using types.item;
using types.item.type;
using types.material;
using types.plant;
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
            List<string> tags = material.tags.ToList();
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
                Debug.Log(args[0]);
                item.Replace(new ItemFoodComponent { nutrition = float.Parse(args[0], CultureInfo.InvariantCulture.NumberFormat)
                // , foodQuality = int.Parse(args[1]) 
                });
            }
        }
    }
}