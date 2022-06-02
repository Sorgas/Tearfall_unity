using System.Collections.Generic;
using UnityEngine;
using util.input;
using util.lang;

namespace types.building {
    public class ConstructionTypeMap : Singleton<ConstructionTypeMap> {
        private Dictionary<string, ConstructionType> map = new();

        public ConstructionTypeMap() {
            loadFiles();
        }

        private void loadFiles() {
            Debug.Log("loading construction types");
            map.Clear();
            TextAsset file = Resources.Load<TextAsset>("data/buildings/constructions");
            int count = 0;
            ConstructionType[] types = JsonArrayReader.readArray<ConstructionType>(file.text);
            if (types == null) return;
            foreach (ConstructionType type in types) {
                Debug.Log(type.blockTypeName + " " + type.materials + " " + type.name);
                type.blockType = BlockTypeEnum.get(type.blockTypeName);
                map.Add(type.name, type);
                count++;
            }
            Debug.Log("loaded " + count + " from " + file.name);
        }

        public static ConstructionType get(string name) {
            return get().map[name];
        }
    }
}