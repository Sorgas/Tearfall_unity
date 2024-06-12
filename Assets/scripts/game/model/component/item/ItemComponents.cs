using System.Collections.Generic;
using Leopotam.Ecs;
using types.unit.need;
using UnityEngine;
using UnityEngine.Rendering;

namespace game.model.component.item {
// main component for all items
public struct ItemComponent {
    public string type; // type name from ItemTypeMap
    public int material;
    public List<string> tags;

    public float weight;
    public int volume; // used for containers
    public int value;
    
    public string materialString; // for faster naming
    public bool isBuildingMaterial;
}

// items with this component can be equipped to units
public struct ItemWearComponent {
    public string bodypart; // covers listed bodyparts
    public string slot; // equpped to slot
    public string layer; // equpped to slot at layer
    public int insulation;
}

public struct ConditionComponent {
    public List<string> acceptedDamages;
}

// item can spoil and rot over time
public struct RotComponent {
    public int speed;
    public int value; // 0-100
}

// item can rust over time
public struct RustComponent {
    public int speed;
    public int value; // 0-100
}

// item can be burned in fire over time
public struct BurnComponent {
    public int speed;
    public int value; // 0-100
}

// item can be broken over time
public struct BrakeComponent {
    public int speed; // incoming damage is multiplied to this
    public int value; // 0-100
}

// item is cannot be put on the ground
public struct DestroyOnGroundComponent { }

// item can be split into two smaller items keeping total size.
public struct SplittableComponent { }

// two items of same type can be merged into one.
public struct MergeableComponent { }

public struct ItemToolComponent {
    public string action;
}

// item gives temporary effect when consumed
public struct PotionEffectComponent {
    public string effect; // todo
}

// item gives effect when worn
public struct EnchantmentEffectComponent {
    public string effect; // todo
}

// item (or building) is container as bag or bottle and con contain items
public struct ContainerComponent {
    public string type; // liquid, solid or grain
    public int capacity;
}

// item is forbidden to any use by player
public struct ForbiddenComponent { }

// item can be used as weapon in combat
public struct CombatComponent {
    public string damageType;
    public int damage;
    public int hitReload;
    public int blockEfficiency;
}

// item is cursed and will put negative effect when used
public struct CursedComponent {
    bool revealed; // curses are initially hidden
    string effect; // todo
}

// item is owned by some creature. creatures not hostile to owner won't use it
public struct OwnedComponent {
    public string owner;
}

// item is held or worn by creature. it will not take part in non-hostile operations
public struct ItemHeldComponent {
    public EcsEntity holder;
}

// item is contained in container (chest)
public struct ItemContainedComponent {
    public EcsEntity container;
}

// item is held or worn by creature. it will not take part in non-hostile operations
// todo move to monobeh handler of item go.
public struct ItemVisualComponent {
    public GameObject go;
    public SpriteRenderer spriteRenderer;
    public GameObject iconGo;
    public SpriteRenderer iconRenderer;
    public SortingGroup sortingGroup;

    public Sprite sprite;
}

public struct ItemFoodComponent {
    public float nutrition; // [0, 1] - restores unit's hunger level
    public int foodQuality; // can give mood buffs
    public float spoiling;
}

public struct ItemSeedComponent {
    public string plant;
}
}