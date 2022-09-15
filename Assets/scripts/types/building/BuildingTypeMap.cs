using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.input;
using util.lang;

namespace types.building {
    public class BuildingTypeMap : Singleton<BuildingTypeMap> {
        private Dictionary<string, BuildingType> map = new();

        public BuildingTypeMap() {
            loadFiles();
        }

        public static BuildingType get(string name) {
            return get().map[name];
        }

        public Dictionary<string, BuildingType>.ValueCollection all() {
            return map.Values;
        }

        private void loadFiles() {
            Debug.Log("loading construction types");
            map.Clear();
            var files = Resources.LoadAll<TextAsset>("data/buildings");
            foreach (TextAsset textAsset in files) {
                loadFromFile(textAsset);
            }
        }

        private void loadFromFile(TextAsset file) {
            int count = 0;
            BuildingType[] types = JsonArrayReader.readArray<BuildingType>(file.text);
            if (types == null) return;
            foreach (BuildingType type in types) {
                type.variants = type.materials.Select(materialString => new BuildingVariant(materialString)).ToArray();
                if(type.category == null) type.category = file.name;
                map.Add(type.name, type);
                count++;
            }
            Debug.Log("loaded " + count + " from " + file.name);
        }
    }
}