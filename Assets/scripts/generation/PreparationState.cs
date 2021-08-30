using System.Collections.Generic;

namespace Tearfall_unity.Assets.scripts.generation
{
    public class PreparationState {
        public List<SettlerData> settlers = new List<SettlerData>();
        public List<ItemData> items = new List<ItemData>();
    }

    // Descriptor for settler. Used to generate unit when game starts.
    public class SettlerData {
        public string name;
        public int age;
        // todo 
    }

    public class ItemData {
        public string type;
        public string material;
        public int quantity;
    }
}