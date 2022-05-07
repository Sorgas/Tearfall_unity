using System.Collections.Generic;
using System.Linq;
using enums.material;
using enums.plant.raw;
using UnityEngine;
using util.input;
using util.lang;

namespace enums.plant {
    public class PlantTypeMap : Singleton<PlantTypeMap> {
        private Dictionary<string, PlantType> map = new();

        public PlantTypeMap() {
            loadFiles();
        }

        private void loadFiles() {
            Debug.Log("loading plants");
            map.Clear();
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/plants");
            foreach (TextAsset file in files) {
                int count = 0;
                RawPlantType[] plants = JsonArrayReader.readArray<RawPlantType>(file.text);
                if (plants == null) continue;
                foreach (RawPlantType raw in plants) {
                    PlantType plant = new PlantType(raw);
                    map.Add(plant.name, plant);
                    count++;
                }
                Debug.Log("loaded " + count + " from " + file.name);
            }
        }

        public PlantType get(string name) {
            return map[name];
        } 
    }
}