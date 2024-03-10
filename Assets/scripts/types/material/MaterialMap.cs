using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.input;
using util.lang;

namespace types.material {
// reads and stores material definitions from jsons.
// id's x00 - material category, 0xx - material. material variants have +x000
public class MaterialMap : Singleton<MaterialMap> {
    private Dictionary<string, Material_> map = new();
    private Dictionary<int, Material_> idMap = new();
    private bool debug = true;
    private MaterialVariantGenerator variantGenerator;

    public MaterialMap() {
        loadFiles();
        variantGenerator = new MaterialVariantGenerator(this);
        variantGenerator.createVariants();
    }

    private void loadFiles() {
        log("loading materials");
        map.Clear();
        TextAsset[] files = Resources.LoadAll<TextAsset>("data/materials");
        foreach (TextAsset file in files) {
            int count = 0;
            RawMaterial[] materials = JsonArrayReader.readArray<RawMaterial>(file.text);
            if (materials == null) continue;
            foreach (RawMaterial raw in materials) {
                Material_ material = new(raw);
                saveMaterial(material);
                count++;
            }
            log("loaded " + count + " from " + file.name);
        }
    }

    public Material_ material(int id) => idMap[id];

    public Material_ material(string name) => map[name];

    public int id(string name) => material(name).id;

    public List<Material_> getByTag(string tag) {
        return map.Values
            .Select(material => material)
            .Where(material => !material.isVariant)
            .Where(material => material.tags.Contains(tag))
            .ToList();
    }

    public List<Material_> getByTagsAny(List<string> tags) {
        return map.Values
            .Select(material => material)
            .Where(material => material.tags.Any(tag => tags.Contains(tag)))
            .ToList();
    }

    public void saveMaterial(Material_ material) {
        map.Add(material.name, material);
        idMap.Add(material.id, material);
    }

    public bool variationRequired(string itemType) => variantGenerator.descriptors.ContainsKey(itemType);

    public Material_ getVariantFor(int materialId, string itemType) {
        // TODO validate
        return material(variateValue(material(materialId).name, itemType));
    }

    // applies wording rule to value for variation (material names and tilesets)
    public static string variateValue(string value, string itemTypeName) => MaterialVariantGenerator.variateValue(value, itemTypeName);

    public List<Material_> all => map.Values.ToList();

    public bool hasMaterial(string materialName) => map.ContainsKey(materialName);

    private void log(string message) {
        if (debug) Debug.Log("[MaterialMap]" + message);
    }
}
}