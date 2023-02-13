using System;
using System.Collections.Generic;
using System.Linq;
using types.item.type.raw;
using UnityEngine;

namespace types.item.type {
    public class ItemType {
        public string name;                                          // id
        public string title;                                         // displayable name
        public string description;                                   // displayable description
        public ToolItemType tool;                                    // is set if this item could be used as tool TODO replace with type aspec
        public int[] atlasXY;
        public string color;
        public HashSet<ItemTagEnum> tags = new();
        public List<string> requiredParts = new();
        public List<string> optionalParts = new();
        public string atlasName;
        public Dictionary<string, string[]> components = new(); // component name to array of component arguments
        public string stockpileCategory;
        public List<string> stockpileMaterialTags = new();
        
        public ItemType(RawItemType raw) {
            name = raw.name;
            title = raw.title ?? raw.name;
            description = raw.description;
            tool = raw.tool;
            atlasXY = raw.atlasXY;
            if (raw.requiredParts == null || raw.requiredParts.Length == 0) {
                requiredParts.Add(name); // single part item
            } else {
                requiredParts.AddRange(raw.requiredParts);
            }
            Type itemTagEnumType = typeof(ItemTagEnum);
            foreach (var tag in raw.tags) {
                string tag2 = tag.ToUpper();
                if (Enum.IsDefined(itemTagEnumType, tag2)) {
                    tags.Add((ItemTagEnum)Enum.Parse(itemTagEnumType, tag2));
                } else {
                    Debug.LogError("tag " + tag2 + " not found");
                }
            }
            foreach (string rawComponent in raw.components) {
                parseAndAddComponentDefinition(rawComponent);
            }
            extractStockpileValues(raw);
        }

        public ItemType(ItemType type, RawItemType rawType, string namePrefix) {
            name = namePrefix + type.name;
            title = rawType.title.Length == 0 ? name : rawType.title;
            description = rawType.description ?? type.description;
            tool = rawType.tool ?? type.tool;
            atlasXY = rawType.atlasXY ?? type.atlasXY;
            color = rawType.color ?? type.color;
        }

        private void parseAndAddComponentDefinition(string componentString) {
            string[] array = componentString.Replace(")", "").Split('(');
            if (array.Length < 1) {
                Debug.LogError(componentString + " is invalid in item type " + name);
                return;
            }
            components.Add(array[0], array.Length > 1 ? array[1].Split(',') : null);
        }

        private void extractStockpileValues(RawItemType raw) {
            stockpileCategory = raw.stockpileCategory == null ? "special" : raw.stockpileCategory;
            if (raw.stockpileMaterialTags == null) {
                stockpileMaterialTags.Add("stone"); // should never be used
            } else {
                stockpileMaterialTags.AddRange(raw.stockpileMaterialTags.Split(","));
            }
        }
    }
}