using System;
using System.Collections.Generic;
using entity;
using enums.item.type.raw;
using UnityEngine;

namespace enums.item.type {
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
        public Dictionary<Type, Aspect> aspects = new();

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
        }

        public ItemType(ItemType type, RawItemType rawType, string namePrefix) {
            name = namePrefix + type.name;
            title = rawType.title.Length == 0 ? name : rawType.title;
            description = rawType.description ?? type.description;
            tool = rawType.tool ?? type.tool;
            atlasXY = rawType.atlasXY ?? type.atlasXY;
            color = rawType.color ?? type.color;
        }

        public void add(Aspect aspect) { 
            aspects.Add(aspect.GetType(), aspect);
        }
    }
}