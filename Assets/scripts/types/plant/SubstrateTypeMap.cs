using System.Collections.Generic;
using UnityEngine;
using util.input;
using util.lang;

namespace types.plant {
    public class SubstrateTypeMap : Singleton<SubstrateTypeMap> {
        private Dictionary<int, SubstrateType> map = new();

        public SubstrateTypeMap() {
            loadAll();
        }

        private void loadAll() {
            string logMessage = "loading substrates";
            map.Clear();
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/substrates");
            foreach (TextAsset file in files) {
                int count = 0;
                SubstrateType[] types = JsonArrayReader.readArray<SubstrateType>(file.text);
                if (types == null) continue;
                foreach (SubstrateType type in types) {
                    type.color = type.parseColor(type.rawColor);
                    map.Add(type.id, type);
                    count++;
                }
                logMessage += "loaded " + count + " from " + file.name + "\n";
            }
            Debug.Log(logMessage);
        }

        public SubstrateType get(int id) => map[id];

        public IEnumerable<SubstrateType> all() => map.Values;
    }
}