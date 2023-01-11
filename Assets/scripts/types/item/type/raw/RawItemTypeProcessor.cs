namespace types.item.type.raw {
    public class RawItemTypeProcessor {
        public string logMessage;

        // public ItemType process(RawItemType rawType, ItemType type) {
        //     logMessage = "";
        //     log("Processing item type " + rawType.name);
        //     addComponentPrototypes(type, rawType);
        //     return type;
        // }

        // // public ItemType processExtendedType(RawItemType rawType, string namePrefix) {
        // //     ItemType baseType = ItemTypeMap.getItemType(rawType.baseItem); // get base type
        // //     return addAspectsFromRawType(new ItemType(baseType, rawType, namePrefix), rawType);
        // // }

        // private void addComponentPrototypes(ItemType type, RawItemType raw) {
        //     foreach (string componentString in raw.components) {
        //         KeyValuePair<string, string[]> pair = parseAspectString(componentString);
        //         switch (pair.Key) {
        //             case "wear": {
        //                     type.prototype.Replace(new ItemWearComponent { slot = pair.Value[0], layer = pair.Value[1] });
        //                     break;
        //                 }
        //             case "food": {
        //                     type.prototype.Replace(new ItemFoodComponent { nutrition = float.Parse(pair.Value[0]), foodQuality = int.Parse(pair.Value[1]) });
        //                     break;
        //                 }
        //             default: {
        //                     log("   Item type aspect with name " + pair.Key + " not found");
        //                     break;
        //                 }
        //         }
        //     }
        // }

        // private KeyValuePair<string, string[]> parseAspectString(string aspectString) {
        //     string[] aspectParts = aspectString.Replace(")", "").Split('(');
        //     return new KeyValuePair<string, string[]>(aspectParts[0], aspectParts.Length > 1 ? aspectParts[1].Split(',') : null);
        // }

        // private void log(string message) {
        //     logMessage += "      [RawItemTypeProcessor]: " + message + "\n";
        // }
    }
}