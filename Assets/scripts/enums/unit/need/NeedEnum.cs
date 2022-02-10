using System;
using System.Collections.Generic;
using util.lang;

namespace enums.unit.need {
    public class NeedEnum {

        // public static WEAR("wear", new WearNeed("no_wear")),
        // public static REST("rest", "fatigue", new RestNeed("tiredness")),
        // public static FOOD("food", "starvation", new FoodNeed("hunger")),
        // public static WATER("water", "dehydration", new WaterNeed("thirst"))
//    WARMTH("warmth", null); // TODO


        public static readonly Dictionary<string, NeedEnum> map = new Dictionary<string, NeedEnum>();

        static NeedEnum() {
            // TODO fill map
        }
    }

    public class NeedEnumValue {
        public readonly string NAME;
        public readonly Need NEED;
        public readonly string DISEASE;

        public NeedEnumValue(string name, Need need, string disease) {
            NAME = name;
            NEED = need;
            DISEASE = disease;
        }

        public NeedEnumValue(String name, Need need) : this(name, need, null) { }
    }

}