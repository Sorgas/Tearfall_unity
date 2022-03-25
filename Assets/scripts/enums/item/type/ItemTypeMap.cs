using System.Collections.Generic;
using enums.item.type.raw;
using enums.unit;
using Newtonsoft.Json;
using UnityEngine;
using util.input;
using util.lang;

namespace enums.item.type {
    public class ItemTypeMap : Singleton<ItemTypeMap> {
        private Dictionary<string, ItemType> types = new Dictionary<string, ItemType>();

        public ItemTypeMap() {
            loadItemTypes();
        }

        private void loadItemTypes() {
            Debug.Log("loading item types");
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/items");
            RawItemTypeProcessor processor = new RawItemTypeProcessor();
            foreach (var file in files) {
                List<RawItemType> raws = JsonConvert.DeserializeObject<List<RawItemType>>(file.text);
                // RawItemType[] raws = JsonArrayReader.readArray<RawItemType>(file.text);
                for (var i = 0; i < raws.Count; i++) {
                    ItemType type = processor.process(raws[i]);
                    type.atlasName = file.name;
                    types.Add(type.name, type);
                }
                Debug.Log(files.Length + " loaded from " + file.name);
            }
        }

        public static ItemType getItemType(string name) {
            return get().types[name];
        }

        public static bool contains(string title) {
            return get().types.ContainsKey(title);
        }
    }
}