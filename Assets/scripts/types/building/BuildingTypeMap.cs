using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.input;
using util.lang;

namespace types.building {
    // loads and stores types of buildings
    public class BuildingTypeMap : Singleton<BuildingTypeMap> {
        private const string BUILDINGS_PATH = "data/buildings";
        private const string RECIPE_LISTS_PATH = "data/lists";
        private Dictionary<string, BuildingType> map = new();
        private Dictionary<string, List<string>> recipeListMap = new();

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
            foreach(StringList2 list in lists) {
                foreach(string listString in list.lists) {
                    // Debug.Log(listString);
                    List<string> list2 = new(listString.Split("/"));
                    recipeListMap.Add(list2[0], list2.GetRange(1, list2.Count - 1));
                }
            }
        }

        private void loadFiles() {
            log("loading construction types");
            map.Clear();
            foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>(BUILDINGS_PATH)) {
                loadFromFile(textAsset);
            }
        }

        private void loadFromFile(TextAsset file) {
            int count = 0;
            BuildingType[] types = JsonArrayReader.readArray<BuildingType>(file.text);
            if (types == null) return;
            foreach (BuildingType type in types) {
                type.init();
                type.variants = type.materials.Select(materialString => new BuildingVariant(materialString)).ToArray();
                if(type.rawComponents != null) {
                    type.components = type.rawComponents.ToList();
                }
                if(type.category == null) type.category = file.name;
                map.Add(type.name, type);
                count++;
            }
            log("loaded " + count + " from " + file.name);
        }

        private void log(string message) {
            Debug.Log("[BuildingTypeMap]: " + message);
        }
    }

    public class StringList2 {
        // public string test;
        public string[] lists;
    }
}