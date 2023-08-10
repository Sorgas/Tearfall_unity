using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.input;
using util.lang;

namespace types.building {
public class ConstructionTypeMap : Singleton<ConstructionTypeMap> {
    private Dictionary<string, ConstructionType> map = new();

    public ConstructionTypeMap() {
        loadFiles();
    }

    private void loadFiles() {
        map.Clear();
        TextAsset file = Resources.Load<TextAsset>("data/constructions");
        int count = 0;
        ConstructionType[] types = JsonArrayReader.readArray<ConstructionType>(file.text);
        if (types == null) return;
        foreach (ConstructionType type in types) {
            type.variants = type.materials.Select(materialString => new BuildingVariant(materialString)).ToArray();
            // Debug.Log(type.blockTypeName + " " + type.materials.Aggregate((material1, material2) => material1 + " " + material2) + " " + type.name);
            type.blockType = BlockTypes.get(type.blockTypeName);
            map.Add(type.name, type);
            count++;
        }
        Debug.Log("[ConstructionTypeMap]: loaded " + count + " from " + file.name);
    }

    public static ConstructionType get(string name) {
        return get().map[name];
    }
}
}