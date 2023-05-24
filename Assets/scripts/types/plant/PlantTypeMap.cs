using System.Collections.Generic;
using types.plant.raw;
using UnityEngine;
using util.input;
using util.lang;

namespace types.plant {
    public class PlantTypeMap : Singleton<PlantTypeMap> {
        private Dictionary<string, PlantType> map = new();
        public readonly PlantSpriteMap spriteMap = new();

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
            string logMessage = "loading plants";
            map.Clear();
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/plants");
            foreach (TextAsset file in files) {
                int count = 0;
                PlantType[] types = JsonArrayReader.readArray<PlantType>(file.text);
                if (types == null) continue;
                foreach (PlantType type in types) {
                    type.init();
                    type.atlasName = file.name;
                    if (file.name.Equals("trees")) type.isTree = true;
                    map.Add(type.name, type);
                    count++;
                }
                logMessage += "loaded " + count + " from " + file.name + "\n";
            }
            Debug.Log(logMessage);
        }
    }
}