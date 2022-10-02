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
            Debug.Log("loading recipe lists");
            TextAsset file = Resources.Load<TextAsset>("data/lists");
            StringList2[] lists = JsonArrayReader.readArray<StringList2>(file.text);
            Debug.Log(lists.Count());
            foreach(StringList2 list in lists) {
                foreach(string listString in list.lists) {
                    Debug.Log(listString);
                    List<string> list2 = new(listString.Split("/"));
                    recipeListMap.Add(list2[0], list2.GetRange(1, list2.Count - 1));
                }
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

    public class StringList2 {
        // public string test;
        public string[] lists;
    }
}