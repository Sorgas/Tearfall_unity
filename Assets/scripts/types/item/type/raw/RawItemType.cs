using System;

namespace types.item.type.raw {

    [Serializable]
    public class RawItemType {
        public string name; // id
        public string baseItem; // items can extends other items
        public string title; // displayable name
        public string description; // displayable description
        public string[] toolActions; // some actions require tools or get bonus from having tool equipped
        public string[] parts; // defines parts of item. first one is main
        public string[] tags; // tags will be copied to items

        public string[] components; // string representation of components: NAME/[ARGUMENT[/ARGUMENT]]
        public string stockpileCategory; // mandatory for all items
        public string stockpileMaterialTags;
        
        // render
        public int[] atlasXY;
        public string color = "0xffffffff";

        public RawItemType() {
            parts = Array.Empty<string>();
            tags = Array.Empty<string>();
            components = Array.Empty<string>();
            atlasXY = Array.Empty<int>();
        }
    }
}