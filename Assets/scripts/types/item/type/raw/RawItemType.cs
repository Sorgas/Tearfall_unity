using System;
using UnityEngine;

namespace enums.item.type.raw {

    [Serializable]
    public class RawItemType {
        public string name; // id
        public string baseItem; // items can extends other items
        public string title; // displayable name
        public string description; // displayable description
        public ToolItemType tool; // is set if this item could be used as tool
        public string[] requiredParts; // defines parts of item. first one is main
        public string[] optionalParts; // defines parts of item. first one is main
        public string[] tags; // tags will be copied to items

        // first element of lists is aspect name
        public string[] aspects; // other aspects, item aspects filled from this on generation.

        // render
        public int[] atlasXY;
        public string color;

        public RawItemType() {
            requiredParts = Array.Empty<string>();
            optionalParts = Array.Empty<string>();
            tags = Array.Empty<string>();
            aspects = Array.Empty<string>();
            atlasXY = Array.Empty<int>();
        }
    }
}