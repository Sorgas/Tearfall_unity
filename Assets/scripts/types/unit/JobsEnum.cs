using System.Collections.Generic;

namespace enums.unit {
    public class JobsEnum {
        public static Job MINER = new("miner", "miner");
        public static Job WOODCUTTER = new("woodcutter", "woodcutter");
        public static Job CARPENTER = new("carpenter", "carpenter");
        public static Job FARMER = new("farmer", "farmer");
        public static Job COOK = new("cook", "cook");
        public static Job SMITH = new("smith", "smith");
        public static Job TAILOR = new("tailor", "tailor");

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