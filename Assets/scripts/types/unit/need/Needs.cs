using System.Collections.Generic;

namespace enums.unit.need {
    
    // stores definitions of creature's needs
    public class Needs {
        public static readonly WearNeed wear = new();
        public static readonly RestNeed rest = new();

        public static readonly NeedEnumValue WEAR = new NeedEnumValue(new WearNeed());
        public static readonly NeedEnumValue REST = new NeedEnumValue(new RestNeed());
        // public static FOOD("food", "starvation", new FoodNeed("hunger")),
        // public static WATER("water", "dehydration", new WaterNeed("thirst"))
        // public static WARMTH("warmth", null); // TODO

        public static readonly Dictionary<string, Needs> map = new Dictionary<string, Needs>();

        static Needs() {
            // TODO fill map
        }
    }

    public class NeedEnumValue {
        public readonly Need NEED;
        public readonly string DISEASE;

        public NeedEnumValue(Need need, string disease) {
            NEED = need;
            DISEASE = disease;
        }

        public NeedEnumValue(Need need) {}
    }
}