using System.Collections.Generic;

namespace enums.item.type.raw {
    public class RawItemType {
        public string name = "unset_item_type"; // id
        public string baseItem; // items can extends other items
        public string title = ""; // displayable name
        public string description; // displayable description
        public ToolItemType tool; // is set if this item could be used as tool
        public List<string> requiredParts = new List<string>(); // defines parts of item. first one is main
        public List<string> optionalParts = new List<string>(); // defines parts of item. first one is main
        public List<string> tags = new List<string>(); // tags will be copied to items

        // first element of lists is aspect name
        public List<string> typeAspects = new List<string>(); // constant aspects. stored in type (value, resource)
        public List<string> aspects = new List<string>(); // other aspects, item aspects filled from this on generation.

        // render
        public int[] atlasXY;
        public string color;
    }
}