using System.Collections.Generic;
using System.Linq;
using generation.item;
using MoreLinq;
using types.item.recipe;
using UnityEngine;
using util.input;
using util.lang;

namespace types.building {
// Loads construction types from json file.
public class ConstructionTypeMap : Singleton<ConstructionTypeMap> {
    private Dictionary<string, ConstructionType> map = new();

    public ConstructionTypeMap() {
        loadFiles();
    }

    private void loadFiles() {
        map.Clear();
        int count = 0;
        IngredientProcessor processor = new IngredientProcessor();
        CraftingOrderGenerator generator = new CraftingOrderGenerator();
        TextAsset file = Resources.Load<TextAsset>("data/constructions");
        JsonArrayReader.readArray<RawConstructionType>(file.text)
            ?.Select(raw => process(raw, processor, generator))
            .ForEach(type => {
                map.Add(type.name, type);
                count++;
            });
        Debug.Log("[ConstructionTypeMap]: loaded " + count + " from " + file.name);
    }

    private ConstructionType process(RawConstructionType raw, IngredientProcessor processor, CraftingOrderGenerator generator) {
        ConstructionType type = new();
        type.name = raw.name;
        raw.ingredients.Select(processor.parseIngredient).ForEach(ingredient => type.ingredients.add(ingredient.key, ingredient));
        type.blockType = BlockTypes.get(raw.blockTypeName);
        type.dummyOrder = generator.generateConstructionOrder(type);
        return type;
    }

    public static ConstructionType get(string name) {
        return get().map[name];
    }

    public static bool has(string name) => get().map.ContainsKey(name);
}
}