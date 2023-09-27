using System.Collections.Generic;
using types.item.type;
using UnityEngine;
using util.input;
using util.lang;

namespace types.plant {
// loads plant definitions from resources/data/plants/* files
public class PlantTypeMap : Singleton<PlantTypeMap> {
    private Dictionary<string, PlantType> map = new();
    public readonly PlantSpriteMap spriteMap = new();
    private bool debug = false;
    private string logMessage = "";

    public PlantTypeMap() {
        loadAll();
        foreach (PlantType plantType in map.Values) {
            spriteMap.createSprites(plantType);
        }
    }

    public PlantType get(string name) {
        return (name != null && map.ContainsKey(name)) ? map[name] : null;
    }

    public ICollection<PlantType> all() => new List<PlantType>(map.Values);

    private void loadAll() {
        log("loading plants");
        map.Clear();
        TextAsset[] files = Resources.LoadAll<TextAsset>("data/plants");
        foreach (TextAsset file in files) {
            int count = 0;
            PlantType[] types = JsonArrayReader.readArray<PlantType>(file.text);
            if (types == null) continue;
            foreach (PlantType type in types) {
                if (!checkPlantProducts(type, file)) continue; // do not create invalid type
                initType(type, file);
                map.Add(type.name, type);
                count++;
            }
            log("loaded " + count + " from " + file.name);
        }
        if (debug) Debug.Log(logMessage);
    }

    private bool checkPlantProducts(RawPlantType type, TextAsset file) {
        if (type.product != null) {
            if (type.product.productItemType == null) {
                Debug.LogWarning($"Plant type {type.name} in file {file.name} has product without item type specified.");
                return false;
            }
            if (ItemTypeMap.getItemType(type.product.productItemType) == null) {
                Debug.LogWarning($"Plant type {type.name} in file {file.name} contains invalid product item type.");
                return false;
            }
        }
        return true;
    }

    private void initType(PlantType type, TextAsset file) {
        type.init();
        if (type.atlasName == "") type.atlasName = file.name;
        if (file.name.Equals("trees")) type.isTree = true;
    }

    private void log(string message) {
        if (debug) logMessage += $"{message} \n";
    }
}
}