using System.Collections.Generic;
using types.item.type.raw;
using UnityEngine;

namespace types.item.type {
    public class ItemType {
        public string name;                                          // id
        public string title;                                         // displayable name
        public string description;                                   // displayable description
        public ToolItemType tool;                                    // is set if this item could be used as tool
        public int[] atlasXY;
        public string color;
        public HashSet<string> tags = new();
        public List<string> parts = new();                           // if set, player will see parts when selecting materials for crafting.
        public string atlasName;
        public Dictionary<string, string[]> components = new(); // component name to array of component arguments
        public string stockpileCategory;
        public List<string> stockpileMaterialTags = new();
        public int stackSize = 1; // how many items allowed on ground cell
        
        public ItemType(RawItemType raw) {
            name = raw.name;
            title = raw.title ?? raw.name;
            description = raw.description;
            if (raw.toolActions != null && raw.toolActions.Length > 0) {
                tool = new ToolItemType();
                tool.action = raw.toolActions[0];
            }
            atlasXY = raw.atlasXY;
            if (raw.parts == null || raw.parts.Length == 0) {
                parts.Add("main"); // single part item
            } else {
                parts.AddRange(raw.parts);
            }
            foreach (string rawTag in raw.tags) {
                tags.Add(rawTag);
            }
            foreach (string rawComponent in raw.components) {
                parseAndAddComponentDefinition(rawComponent);
            }
            extractStockpileValues(raw);
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