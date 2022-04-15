using System;
using System.Collections.Generic;
using util.lang;

namespace enums.unit.need {
    public class NeedEnum {
        public static readonly NeedEnumValue WEAR = new NeedEnumValue(NeedTypeEnum.WEAR, new WearNeed());
        // public static REST("rest", "fatigue", new RestNeed("tiredness")),
        // public static FOOD("food", "starvation", new FoodNeed("hunger")),
        // public static WATER("water", "dehydration", new WaterNeed("thirst"))
        // public static WARMTH("warmth", null); // TODO
        
        public static readonly Dictionary<string, NeedEnum> map = new Dictionary<string, NeedEnum>();

        static NeedEnum() {
            // TODO fill map
        }
    }

    public class NeedEnumValue {
        public readonly NeedTypeEnum TYPE;
        public readonly Need NEED;
        public readonly string DISEASE;

        public NeedEnumValue(NeedTypeEnum type, Need need, string disease) {
            TYPE = type;
            NEED = need;
            DISEASE = disease;
        }

        public NeedEnumValue(NeedTypeEnum type, Need need) : this(type, need, null) { }
    }

}