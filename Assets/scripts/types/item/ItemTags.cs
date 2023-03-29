using System.Collections.Generic;

namespace types.item {
    public static class ItemTags {
        public const string NULL = "null";
        // generation
        public const string STONE_IGNEOUS = "stone_igneous"; // used for stone layers generation
        public const string STONE_METAMORFIC = "stone_metamorfic";
        public const string STONE_SEDIMENTARY = "stone_sedimentary";
        // materials of item
        public const string SOIL = "soil";
        public const string STONE = "stone"; // gabbro(material) rock(type) // stones have no origin
        public const string METAL = "metal"; // brass(material) bar(type)
        public const string WOOD = "wood"; // birch(material) log(type)
        public const string CLOTH = "cloth";
        public const string MEAT = "meat"; // fox(origin) meat(material) piece(type)
        public const string ORE = "ore"; // magnetite(material) rock(type)
        public const string ORGANIC = "organic"; // 
        public const string FUEL = "fuel";
        // food/corpses
        public const string COOKABLE = "cookable"; // can be boiled or roasted
        public const string BREWABLE = "brewable"; // item can be prepared into drink
        public const string DRINKABLE = "drinkable"; // TODO replace with aspect
        public const string RAW = "raw"; // raw cow meat piece
        public const string SPOILED = "spoiled"; // spoiled raw cow meat peace
        public const string CORPSE = "corpse";
        public const string SAPIENT = "sapient";
        public const string PREPARED = "prepared"; // cow meat stew

        public const string SEED_PRODUCE = "seed_produce";
        public const string WATER = "water";
        public const string MATERIAL = "material"; // item is raw material for building and crafting

        public static readonly List<string> all = new(new[] {
            STONE_IGNEOUS, STONE_METAMORFIC, STONE_SEDIMENTARY, SOIL, STONE, METAL, WOOD, CLOTH, MEAT,
            ORE, ORGANIC, FUEL, COOKABLE, BREWABLE, DRINKABLE, RAW, SPOILED, CORPSE, SAPIENT, PREPARED,
            SEED_PRODUCE, WATER, MATERIAL
        });
    }
}