namespace mainMenu.WorldGen {
    public class WorldGenConfig {
        public int size;
        public int seed;

        //for ocean filler
        public float seaLevel = 0.5f;

        //for river worldgen
        public int riverDensity = 1000;
        public float largeRiverStartLevel = 0.7f;

        //for temperature worldgen
        public float polarLineWidth = 0.04f;
        public float equatorLineWidth = 0.03f;
        public float maxTemperature = 35;
        public float minTemperature = -15;
        public float seasonalDeviation = 5;
        public float elevationInfluence = 4f;

        //rainfall x3 for sm/y
        public int minRainfall = 5; // deserts and glaciers
        public int maxRainfall = 75; // tropical forests

        public WorldGenConfig(int seed, int size) {
            this.seed = seed;
            this.size = size;
        }
    }
}