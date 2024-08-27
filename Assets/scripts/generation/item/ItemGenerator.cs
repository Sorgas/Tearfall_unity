using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.component.plant;
using Leopotam.Ecs;
using types.item.type;
using types.material;
using types.plant;
using UnityEngine;
using util.lang.extension;

namespace generation.item {
// should be the only place item entities are populated
public class ItemGenerator {
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
        entity.take<ItemComponent>().tags.AddRange(type.tags);
        entity.Replace(new NameComponent { name = material.name + " " + type.title });
        addComponentsFromType(type, ref entity, material.id);
        return entity;
    }

    public EcsEntity generatePlantProduct(PlantBlock block, EcsEntity entity) {
        if (block.blockType == PlantBlockTypeEnum.TRUNK.code) {
            return generateItem("log", MaterialMap.get().material(block.material).name, entity);
        }
        return EcsEntity.Null;
    }

    public EcsEntity generatePlantProduct(EcsEntity plant, EcsEntity entity) {
        PlantType type = plant.take<PlantComponent>().type;
        return generateItem(type.product.productItemType, type.product.productMaterial, entity);
    }

    // adds optional item components which not all items should have
    private void addComponentsFromType(ItemType type, ref EcsEntity item, int material) {
        if (type.components.ContainsKey("wear")) {
            string[] args = type.components["wear"];
            item.Replace(new ItemWearComponent { slot = args[0], layer = args[1] });
        }
        if (type.components.ContainsKey("food")) {
            string[] args = type.components["food"];
            item.Replace(new ItemFoodComponent {
                nutrition = float.Parse(args[0], CultureInfo.InvariantCulture.NumberFormat),
                foodQuality = int.Parse(args[1]),
                spoiling = 0
            });
        }
        if (type.components.ContainsKey("grip")) {
            string[] args = type.components["grip"];
            item.Replace(new ItemGripComponent { type = args[0] });
        }
        if (type.weapon != null) {
            item.Replace(new ItemWeaponComponent {
                damage = type.weapon.damage,
                accuracy = type.weapon.accuracy,
                reload = type.weapon.reload,
                skill = type.weapon.skill,
                damageType = type.weapon.damageType,
            });
        }
        if (type.toolAction != null) {
            item.Replace(new ItemToolComponent { action = type.toolAction });
        }
    }
}
}