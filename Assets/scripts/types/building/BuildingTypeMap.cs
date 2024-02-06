using System.Collections.Generic;
using System.Linq;
using generation.item;
using MoreLinq;
using types.item.recipe;
using UnityEngine;
using util.input;
using util.lang;
using static types.PassageTypes;

namespace types.building {
// loads and stores types of buildings
public class BuildingTypeMap : Singleton<BuildingTypeMap> {
    private const string BUILDINGS_PATH = "data/buildings";
    private const string RECIPE_LISTS_PATH = "data/lists";
    private Dictionary<string, BuildingType> map = new();
    private Dictionary<string, List<string>> recipeListMap = new();
    private IngredientProcessor processor = new();
    private bool debug = false;

    public BuildingTypeMap() {
        loadLists();
        loadFiles();
    }

    public static BuildingType get(string name) => get().map[name];

    public List<string> getRecipes(string name) => recipeListMap.ContainsKey(name) ? recipeListMap[name] : new List<string>();

    public Dictionary<string, BuildingType>.ValueCollection all() => map.Values;

    private void loadLists() {
        log("loading recipe lists");
        TextAsset file = Resources.Load<TextAsset>(RECIPE_LISTS_PATH);
        StringList2[] lists = JsonArrayReader.readArray<StringList2>(file.text);
        foreach (StringList2 list in lists) {
            foreach (string listString in list.lists) {
                // Debug.Log(listString);
                List<string> list2 = new(listString.Split("/"));
                recipeListMap.Add(list2[0], list2.GetRange(1, list2.Count - 1));
            }
        }
    }

    private void loadFiles() {
        log("loading construction types");
        map.Clear();
        CraftingOrderGenerator generator = new();
        foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>(BUILDINGS_PATH)) {
            loadFromFile(textAsset, generator);
        }
    }

    private void loadFromFile(TextAsset file, CraftingOrderGenerator generator) {
        int count = 0;
        RawBuildingType[] raws = JsonArrayReader.readArray<RawBuildingType>(file.text);
        if (raws == null) return;
        foreach (RawBuildingType raw in raws) {
            if (raw.category == null) raw.category = file.name;
            BuildingType type = create(raw, generator);
            map.Add(type.name, type);
            count++;
        }
        log("loaded " + count + " from " + file.name);
    }

    private BuildingType create(RawBuildingType raw, CraftingOrderGenerator generator) {
        BuildingType type = new();
        type.name = raw.name;
        type.title = raw.title;
        type.tileset = raw.tileset;
        if (raw.tilesetSize != 0) type.tilesetSize = raw.tilesetSize;
        if (raw.tileCount != 0) type.tileCount = raw.tileCount;
        type.size = raw.size;
        type.positionN = raw.positionN;
        type.positionS = raw.positionS;
        type.positionE = raw.positionE;
        type.positionW = raw.positionW;
        type.access = raw.access;
        type.job = raw.job;
        if (raw.passage != null) {
            type.passage = raw.passage;
            if (type.passage.Equals(IMPASSABLE.name)) type.passage = IMPASSABLE_BUILDING.name;
        }
        type.category = raw.category;
        if (raw.components != null) {
            type.components = new(raw.components);
        }

        raw.ingredients.Select(processor.parseIngredient).ForEach(ingredient => type.ingredients.add(ingredient.key, ingredient));
        type.init();
        type.dummyOrder = generator.generateBuildingOrder(type);
        return type;
    }

    private void log(string message) {
        if (debug) Debug.Log("[BuildingTypeMap]: " + message);
    }
}

public class StringList2 {
    // public string test;
    public string[] lists;
}
}