using System.Collections.Generic;
using UnityEngine;

namespace types.item {
public static class ItemTags {
    public static readonly ItemTag NULL = new("null", false);
    // generation
    public static readonly ItemTag STONE_IGNEOUS = new("stone_igneous", false); // used for stone layers generation
    public static readonly ItemTag STONE_METAMORFIC = new("stone_metamorfic", false);
    public static readonly ItemTag STONE_SEDIMENTARY = new("stone_sedimentary", false);
    // materials of item
    public static readonly ItemTag SOIL = new("soil", false);
    public static readonly ItemTag STONE = new("stone"); // gabbro(material) rock(type) // stones have no origin
    public static readonly ItemTag METAL = new("metal"); // brass(material) bar(type)
    public static readonly ItemTag WOOD = new("wood"); // birch(material) log(type)
    public static readonly ItemTag CLOTH = new("cloth");
    public static readonly ItemTag MEAT = new("meat"); // fox(origin) meat(material) piece(type)
    public static readonly ItemTag ORE = new("ore"); // magnetite(material) rock(type)
    public static readonly ItemTag ORGANIC = new("organic"); // 
    public static readonly ItemTag FUEL = new("fuel");
    // food/corpses
    public static readonly ItemTag COOKABLE = new("cookable"); // can be boiled or roasted
    public static readonly ItemTag BREWABLE = new("brewable"); // item can be prepared into drink
    public static readonly ItemTag DRINKABLE = new("drinkable"); // TODO replace with aspect
    public static readonly ItemTag RAW = new("raw"); // raw cow meat piece
    public static readonly ItemTag SPOILED = new("spoiled"); // spoiled raw cow meat peace
    public static readonly ItemTag CORPSE = new("corpse");
    public static readonly ItemTag SAPIENT = new("sapient");
    public static readonly ItemTag PREPARED = new("prepared"); // cow meat stew

    public static readonly ItemTag SEED_PRODUCE = new("seed_produce");
    public static readonly ItemTag WATER = new("water");
    public static readonly ItemTag MATERIAL = new("material"); // item is raw material for building and crafting

    public static readonly List<ItemTag> all = new(new[] {
        STONE_IGNEOUS, STONE_METAMORFIC, STONE_SEDIMENTARY, SOIL, STONE, METAL, WOOD, CLOTH, MEAT,
        ORE, ORGANIC, FUEL, COOKABLE, BREWABLE, DRINKABLE, RAW, SPOILED, CORPSE, SAPIENT, PREPARED,
        SEED_PRODUCE, WATER, MATERIAL
    });

    public static Dictionary<string, ItemTag> tags = initTags();

    public static ItemTag findTag(string name) {
        if (tags.ContainsKey(name)) return tags[name];
        Debug.LogError("tag " + name + " not found.");
        return NULL;
    }

    public static Dictionary<string, ItemTag> initTags() {
        Dictionary<string, ItemTag> result = new();
        foreach (var tag in all) {
            result.Add(tag.name, tag);
        }
        return result;
    }
}

public class ItemTag {
    public string name;
    public string displayName;
    public bool displayable;

    public ItemTag(string name, string displayName) {
        this.name = name;
        this.displayName = displayName;
        displayable = true;
    }

    // TODO add first letter capitalization
    public ItemTag(string name) : this(name, name.ToUpper()) { }

    public ItemTag(string name, bool displayable) : this(name) {
        this.displayable = displayable;
    }
}
}