using System;

namespace types.item.type {
[Serializable]
public class RawItemType {
    public string name; // id
    public string baseItem; // items can extends other items
    public string title; // displayable name
    public string description; // displayable description
    public int value = 1; // default value for material items

    // public string[] parts; // defines parts of item. first one is main
    public string[] tags; // tags will be copied to items

    public string[] components; // string representation of components: NAME/[ARGUMENT[/ARGUMENT]]
    public string stockpileCategory; // mandatory for all items
    public string stockpileMaterialTags;

    // render
    public int[] atlasXY;
    public string atlasName; // set by item type map
    public string color = "0xffffffff";

    // tools
    public string toolAction; // some actions require tools or get bonus from having tool equipped
    public RawWeaponItemType weapon; // present if item can be used in combat

    public RawItemType() {
        // parts = Array.Empty<string>();
        tags = Array.Empty<string>();
        components = Array.Empty<string>();
        atlasXY = Array.Empty<int>();
    }
}

public class RawWeaponItemType {
    // weapons
    public float damage; // if 0, item is not a weapon
    public float accuracy;
    public float reload; // attack reload turns
    public string skill; // combat skill to use
    public string damageType;
    // public string ammo; // ammo item name
}
}