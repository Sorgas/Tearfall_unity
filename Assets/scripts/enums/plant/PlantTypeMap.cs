using System.Collections.Generic;
using enums.plant.raw;
using UnityEngine;
using util.input;
using util.lang;

namespace enums.plant {
    public class PlantTypeMap : Singleton<PlantTypeMap> {
        private Dictionary<string, PlantType> map = new();

        public PlantTypeMap() {
            loadAll();
        }

        public PlantType get(string name) {
            return map[name];
        }

        private void loadAll() {
            string logMessage = "loading plants";
            map.Clear();
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/plants");
            foreach (TextAsset file in files) {
                int count = 0;
                RawPlantType[] raws = JsonArrayReader.readArray<RawPlantType>(file.text);
                if (raws == null) continue;
                foreach (RawPlantType raw in raws) {
                    PlantType type = new(raw);
                    map.Add(type.name, type);
                    type.atlasName = file.name;
                    count++;
                }
                logMessage += "loaded " + count + " from " + file.name + "\n";
            }
            Debug.Log(logMessage);
        }
    }
}