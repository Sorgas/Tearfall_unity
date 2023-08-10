using System.Collections.Generic;
using UnityEngine;
using util.input;
using util.lang;

namespace types.plant {
    public class SubstrateTypeMap : Singleton<SubstrateTypeMap> {
        private Dictionary<int, SubstrateType> map = new();
        private string logMessage;
        private bool debug = false;
        
        public SubstrateTypeMap() {
            loadAll();
        }

        private void loadAll() {
            log("[SubstrateTypeMap]: loading substrates");
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
                log("    loaded " + count + " from " + file.name);
            }
            if(debug) Debug.Log(logMessage);
        }

        public SubstrateType get(int id) => map[id];

        public IEnumerable<SubstrateType> all() => map.Values;

        private void log(string message) {
            if(debug) logMessage += message + "\n";
        }
    }
}