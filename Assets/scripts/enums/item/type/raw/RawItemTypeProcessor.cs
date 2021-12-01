using System;
using System.Collections.Generic;
using entity;
using UnityEngine;

namespace enums.item.type.raw {
    public class RawItemTypeProcessor {

        public ItemType process(RawItemType rawType) {
            return addAspectsFromRawType(new ItemType(rawType), rawType);
        }

        public ItemType processExtendedType(RawItemType rawType, string namePrefix) {
            ItemType baseType = ItemTypeMap.getItemType(rawType.baseItem); // get base type
            return addAspectsFromRawType(new ItemType(baseType, rawType, namePrefix), rawType);
        }

        private ItemType addAspectsFromRawType(ItemType type, RawItemType raw) {
            foreach (var rawTypeTypeAspect in raw.typeAspects) { // create type aspects
                type.add(createAspect(rawTypeTypeAspect, type));
            }
            foreach (var rawAspect in raw.aspects) {
                KeyValuePair<string, string[]> pair = parseAspectString(rawAspect);
                type.itemAspects.Add(pair.Key, new List<string>(pair.Value));
            }
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
                    return new WearAspect(); // TODO values
                }
                default: {
                    Debug.LogWarning("Item type aspect with name " + type.name);
                    return null;
                }
            }
        }

        private KeyValuePair<string, string[]> parseAspectString(string aspectString) {
            string[] aspectParts = aspectString.Replace(")", "").Split('(');
            return new KeyValuePair<string, string[]>(aspectParts[0], aspectParts.Length > 1 ? aspectParts[1].Split(',') : null);
        }
    }
}