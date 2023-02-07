using System;

namespace types.item.type.raw {

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

        public string[] components; // string representation of components: NAME/[ARGUMENT[/ARGUMENT]]
        public string stockpileCategory; // mandatory for all items
        public string stockpileMaterialTags;
        
        // render
        public int[] atlasXY;
        public string color;

        public RawItemType() {
            requiredParts = Array.Empty<string>();
            optionalParts = Array.Empty<string>();
            tags = Array.Empty<string>();
            components = Array.Empty<string>();
            atlasXY = Array.Empty<int>();
        }
    }
}