using System.Collections.Generic;
using UnityEngine;
using util.input;
using util.lang;

namespace types.building {
    public class BuildingTypeMap : Singleton<BuildingTypeMap> {
        private Dictionary<string, BuildingType> map = new();

        public BuildingTypeMap() {
            loadFiles();
        }

        private void loadFiles() {
            Debug.Log("loading construction types");
            map.Clear();
            TextAsset file = Resources.Load<TextAsset>("data/buildings/furniture"); // TODO use other files
            int count = 0;
            BuildingType[] types = JsonArrayReader.readArray<BuildingType>(file.text);
            if (types == null) return;
            foreach (BuildingType type in types) {
                // type.variants = type.materials.Select(materialString => new ConstructionVariant(materialString)).ToArray();
                // Debug.Log(type.blockTypeName + " " + type.materials + " " + type.name);
                // type.blockType = BlockTypes.get(type.blockTypeName);
                map.Add(type.name, type);
                count++;
            }
            Debug.Log("loaded " + count + " from " + file.name);
        }

        public static BuildingType get(string name) {
            return get().map[name];
        }
    }
}