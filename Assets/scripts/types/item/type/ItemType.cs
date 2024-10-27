using System;
using System.Collections.Generic;
using UnityEngine;

namespace types.item.type {
public class ItemType {
    public string name; // id
    public string baseItem;
    // strings
    public string title; // displayable name
    public string description; // displayable description
    public int value; // base value of item
    
    public HashSet<string> tags = new();
    // public List<string> parts = new(); // if set, player will see parts when selecting materials for crafting. // TODO post MVP
    // components
    public Dictionary<string, string[]> components = new(); // component name to array of component arguments
    public string toolAction;
    public WeaponItemType weapon; // is set if this item could be used as weapon
    public ShieldItemType shield;
    // storage
    public string stockpileCategory;
    public List<string> stockpileMaterialTags = new();
    public int stackSize = 1; // how many items allowed on ground cell
    // render
    public string atlasName;
    public int[] atlasXY;
    public string color;

    public ItemType(RawItemType raw) {
        name = raw.name;
        title = raw.title ?? raw.name;
        baseItem = raw.baseItem;
        description = raw.description;
        value = raw.value;
        toolAction = raw.toolAction;
        if (raw.weapon != null) {
            weapon = new WeaponItemType();
            weapon.damage = raw.weapon.damage;
            weapon.accuracy = raw.weapon.accuracy;
            weapon.reload = raw.weapon.reload;
            weapon.skill = raw.weapon.skill;
            weapon.attribute = raw.weapon.attribute;
            weapon.damageType = raw.weapon.damageType;
        }
        if (raw.shield != null) {
            shield = new ShieldItemType();
            shield.blockChance = raw.shield.blockChance;
            shield.reload = raw.shield.reload;
        }
        atlasName = raw.atlasName;
        atlasXY = raw.atlasXY;
        // if (raw.parts == null || raw.parts.Length == 0) {
        //     parts.Add("main"); // single part item
        // } else {
        //     parts.AddRange(raw.parts);
        // }
        foreach (string rawTag in raw.tags) {
            tags.Add(rawTag);
        }
        foreach (string rawComponent in raw.components) {
            parseAndAddComponentDefinition(rawComponent);
        }
        extractStockpileValues(raw);
    }

    private void parseAndAddComponentDefinition(string componentString) {
        string[] array = componentString.Replace(")", "").Split('(');
        if (array.Length < 1) {
            Debug.LogError(componentString + " is invalid in item type " + name);
            return;
        }
        components.Add(array[0], array.Length > 1 ? array[1].Split(',') : null);
    }

    private void extractStockpileValues(RawItemType raw) {
        stockpileCategory = raw.stockpileCategory == null ? "special" : raw.stockpileCategory;
        if (raw.stockpileMaterialTags == null) {
            stockpileMaterialTags.Add("stone"); // should never be used
        } else {
            stockpileMaterialTags.AddRange(raw.stockpileMaterialTags.Split(","));
        }
    }
}

public class WeaponItemType {
    // weapons
    public int damage; // if 0, item is not a weapon
    public float accuracy;
    public float reload; // attack reload turns
    public string skill; // combat skill to use
    public string attribute;
    public string damageType;
    // public string ammo; // ammo item name
}

public class ShieldItemType {
    public float blockChance;
    public float reload;
}
}