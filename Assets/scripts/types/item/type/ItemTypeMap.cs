using System.Collections.Generic;
using enums.item.type.raw;
using Newtonsoft.Json;
using UnityEngine;
using util.lang;

namespace enums.item.type {
    public class ItemTypeMap : Singleton<ItemTypeMap> {
        private Dictionary<string, ItemType> types = new();
        private string logMessage;

        public ItemTypeMap() {
            loadItemTypes();
        }

        public static ItemType getItemType(string name) {
            return get().types[name];
        }

        public static bool contains(string title) {
            return get().types.ContainsKey(title);
        }

        private void loadItemTypes() {
            log("Loading item types");
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/items");
            RawItemTypeProcessor processor = new RawItemTypeProcessor();
            foreach (var file in files) {
                loadFromFile(file, processor);
            }
            Debug.Log(logMessage);
        }

        private void loadFromFile(TextAsset file, RawItemTypeProcessor processor) {
            log("   Loading from " + file.name);
            List<RawItemType> raws = JsonConvert.DeserializeObject<List<RawItemType>>(file.text);
            for (var i = 0; i < raws.Count; i++) {
                ItemType type = processor.process(raws[i]);
                logMessage += processor.logMessage;
                type.atlasName = file.name;
                types.Add(type.name, type);
            }
            log("   " + raws.Count + " loaded from " + file.name);
        }

        private void log(string message) {
            logMessage += message + "\n";
        }
    }
}