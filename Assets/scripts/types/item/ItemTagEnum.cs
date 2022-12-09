namespace enums.item {
    public enum ItemTagEnum {
        NULL,

        // generation
        STONE_IGNEOUS, // used for stone layers generation
        STONE_METAMORFIC,
        STONE_SEDIMENTARY,
        
        // materials of item
        SOIL,
        STONE,           // gabbro(material) rock(type) // stones have no origin
        METAL,           // brass(material) bar(type)
        WOOD,            // birch(material) log(type)
        CLOTH,
        MEAT,            // fox(origin) meat(material) piece(type)
        ORE,             // magnetite(material) rock(type)
        ORGANIC,         // 
        FUEL,

        // food/corpses
        COOKABLE,        // can be boiled or roasted
        BREWABLE,        // item can be prepared into drink
        DRINKABLE,       // TODO replace with aspect
        RAW,             // raw cow meat piece,
        SPOILED,         // spoiled raw cow meat peace
        CORPSE,
        SAPIENT,
        PREPARED,        // cow meat stew

        SEED_PRODUCE,
        WATER,
        MATERIAL         // item is raw material for building and crafting
    }
}