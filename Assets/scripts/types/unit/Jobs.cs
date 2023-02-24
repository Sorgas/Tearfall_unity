namespace types.unit {
    public static class Jobs {
        public static readonly Job NONE = new("none", "none");
        public static readonly Job MINER = new("miner", "miner");
        public static readonly Job WOODCUTTER = new("woodcutter", "woodcutter");
        public static readonly Job BUILDER = new("builder", "builder");
        public static readonly Job CARPENTER = new("carpenter", "carpenter");
        public static readonly Job FARMER = new("farmer", "farmer");
        public static readonly Job COOK = new("cook", "cook");
        public static readonly Job SMITH = new("smith", "smith");
        public static readonly Job TAILOR = new("tailor", "tailor");
        public static readonly Job HAULER = new("hauler", "hauler");
        
        public static readonly Job[] jobs = { MINER, WOODCUTTER, BUILDER, CARPENTER, FARMER, COOK, SMITH, TAILOR };
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