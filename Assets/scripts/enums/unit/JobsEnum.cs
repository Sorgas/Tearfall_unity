using System.Collections.Generic;

namespace enums.unit {
    public class JobsEnum {
        public static Job MINER = new Job("miner", "miner");
        public static Job WOODCUTTER = new Job("woodcutter", "woodcutter");
        public static Job CARPENTER = new Job("carpenter", "carpenter");
        public static Job FARMER = new Job("farmer", "farmer");
        public static Job COOK = new Job("cook", "cook");
        public static Job SMITH = new Job("smith", "smith");
        public static Job TAILOR = new Job("tailor", "tailor");

        public static Job[] jobs = { MINER, WOODCUTTER, CARPENTER, FARMER, COOK, SMITH, TAILOR };
    }

    public class Job {
        public readonly string name;
        public readonly string iconName;

        public Job(string name, string iconName) {
            this.name = name;
            this.iconName = iconName;
        }
    }
}