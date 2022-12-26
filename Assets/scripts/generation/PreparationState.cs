using System.Collections.Generic;
using util.lang;

namespace generation
{
    // filled from UI when player selects settlers and items
    public class PreparationState : Singleton<PreparationState>{
        public List<SettlerData> settlers = new();
        public List<ItemData> items = new();
    }

    // Descriptor for settler. Used to generate unit when game starts.
    public class SettlerData {
        public string name;
        public int age;
        public string type;
        // todo 
    }

    public struct ItemData {
        public string type;
        public string material;
        public int quantity;
    }
}