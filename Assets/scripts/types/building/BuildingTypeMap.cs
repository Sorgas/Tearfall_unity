using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.input;
using util.lang;

namespace types.building {
    public class BuildingTypeMap : Singleton<BuildingTypeMap> {
        private Dictionary<string, BuildingType> map = new();
        private Dictionary<string, List<string>> recipeListMap = new();

        public BuildingTypeMap() {
            loadLists();
            loadFiles();
        }

        public static BuildingType get(string name) {
            return get().map[name];
        }

        public Dictionary<string, BuildingType>.ValueCollection all() {
            return map.Values;
        }

        private void loadLists() {
            TextAsset file = Resources.Load<TextAsset>("data/recipes/lists.json");
            List<string>[] lists = JsonArrayReader.readArray<List<string>>(file.text);
            foreach(List<string> list in lists) {
                recipeListMap.Add(list[0], list.GetRange(1, list.Count - 1));
            }
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