using System.Collections.Generic;
using System.Linq;

namespace types.item.type {
// Some item types can derive from another item type. This class merges base types into types.
public class ItemTypeMerger {
    
    public void mergeRawTypes(List<RawItemType> types) {
        Dictionary<string, RawItemType> map = types.ToDictionary(type => type.name, type => type);
        foreach (var typeName in map.Keys) {
            if (map[typeName].baseItem != null) {
                mergeRawType(map[typeName], map[map[typeName].baseItem]);
            }
        }
    }

    // if target type field has default value after loading, value will be reset from base type
    private void mergeRawType(RawItemType target, RawItemType baseType) {
        if (target.title == null) target.title = baseType.title;
        if (target.description == null) target.description = baseType.description;
        if (target.value == -1) target.value = baseType.value;
        if (target.stockpileCategory == null) target.stockpileCategory = baseType.stockpileCategory;
        if (target.stockpileMaterialTags == null) target.stockpileMaterialTags = baseType.stockpileMaterialTags;

        if (target.atlasXY == null) target.atlasXY = baseType.atlasXY;
        if (target.color == null) target.color = baseType.color;

        if (target.toolAction == null) target.toolAction = baseType.toolAction;
        if (baseType.weapon != null) {
            mergeRawWeaponType(target, baseType);
        }
        if (baseType.shield != null) {
            mergeRawShieldType(target, baseType);
        }
    }

    private void mergeRawWeaponType(RawItemType target, RawItemType baseType) {
        RawWeaponItemType targetWeapon = target.weapon ?? new RawWeaponItemType();
        if (targetWeapon.damage < 0) targetWeapon.damage = baseType.weapon.damage;
        if (targetWeapon.accuracy < 0) targetWeapon.accuracy = baseType.weapon.accuracy;
        if (targetWeapon.reload < 0) targetWeapon.reload = baseType.weapon.reload;
        if (targetWeapon.skill == null) targetWeapon.skill = baseType.weapon.skill;
        if (targetWeapon.damageType == null) targetWeapon.damageType = baseType.weapon.damageType;
        target.weapon = targetWeapon;
    }
    
    private void mergeRawShieldType(RawItemType target, RawItemType baseType) {
        RawShieldItemType targetShield = target.shield ?? new RawShieldItemType();
        if (targetShield.blockChance < 0) targetShield.blockChance = baseType.shield.blockChance;
        if (targetShield.reload < 0) targetShield.reload = baseType.shield.reload;
        target.shield = targetShield;
    }

    // merging of complex fields occurs after parsing
    public void mergeItemTypes(List<ItemType> types) {
        Dictionary<string, ItemType> map = types.ToDictionary(type => type.name, type => type);
        foreach (var typeName in map.Keys) {
            if (map[typeName].baseItem != null) {
                mergeItemType(map[typeName], map[map[typeName].baseItem]);
            }
        }
    }

    private void mergeItemType(ItemType target, ItemType source) {
        if (source.tags != null) {
            foreach (var tag in source.tags) {
                target.tags.Add(tag);
            }
        }
        if (source.components != null) {
            foreach (var key in source.components.Keys) {
                if (!target.components.ContainsKey(key)) {
                    target.components.Add(key, source.components[key]);
                }
            }
        }
    }
}
}