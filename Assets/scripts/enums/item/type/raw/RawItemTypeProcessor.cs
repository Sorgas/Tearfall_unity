using System.Collections.Generic;
using System.Linq;
using entity;
using util.extension;

namespace enums.item.type.raw {
    public class RawItemTypeProcessor {
        public string logMessage;
        
        public ItemType process(RawItemType rawType) {
            logMessage = "";
            log("Processing item type " + rawType.name);
            ItemType type = addAspectsFromRawType(new ItemType(rawType), rawType);
            return type;
        }

        public ItemType processExtendedType(RawItemType rawType, string namePrefix) {
            ItemType baseType = ItemTypeMap.getItemType(rawType.baseItem); // get base type
            return addAspectsFromRawType(new ItemType(baseType, rawType, namePrefix), rawType);
        }

        private ItemType addAspectsFromRawType(ItemType type, RawItemType raw) {
            raw.aspects
                .Select(aspect => createAspect(aspect, type))
                .Where(aspect => aspect != null)
                .ForEach(aspect => {
                    log("   Aspect " + aspect.GetType().Name + " added");
                    type.add(aspect);
                });
            return type;
        }

        private Aspect createAspect(string aspectString, ItemType type) {
            KeyValuePair<string, string[]> pair = parseAspectString(aspectString);
            switch (pair.Key) {
                case "value": {
                    return new ValueAspect(float.Parse(pair.Value[0]));
                }
                case "fuel": {
                    return new FuelAspect();
                }
                case "wear": {
                    return new WearAspect(pair.Value[0], pair.Value[1]);
                }
                default: {
                    log("   Item type aspect with name " + pair.Key + " not found");
                    return null;
                }
            }
        }

        private KeyValuePair<string, string[]> parseAspectString(string aspectString) {
            string[] aspectParts = aspectString.Replace(")", "").Split('(');
            return new KeyValuePair<string, string[]>(aspectParts[0], aspectParts.Length > 1 ? aspectParts[1].Split(',') : null);
        }
        
        private void log(string message) {
            logMessage += "      [RawItemTypeProcessor]: " + message + "\n";
        }
    }
}