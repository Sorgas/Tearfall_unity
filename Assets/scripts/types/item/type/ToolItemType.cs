using System.Collections.Generic;

namespace types.item.type {
// part of ItemType for tools and weapons
public class ToolItemType {
    // tools
    public string action; // some jobs, (mining, lumbering) require tools with specific action.
    // weapons
    public float damage; // if 0, item is not a weapon
    public float accuracy;
    public float reload; // attack reload turns
    public string skill; // combat skill to use
    public string damageType;
    // public string ammo; // ammo item name
}
}