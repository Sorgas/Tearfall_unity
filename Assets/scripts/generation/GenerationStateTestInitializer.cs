using generation.unit;
using types.building;
using types.item.recipe;
using types.item.type;
using types.material;
using types.plant;
using types.unit;
using UnityEngine;

namespace generation {
// fills generation state for test level. Settlement position is not selected for test levels, so world is generated immediately.
public class GenerationStateTestInitializer {
    // inits regular map in tiny world
    public void initState() {
        initAndGenerateTestWorld();
        createSettlers(1);
        createTestItem();
        createBuildings();
    }

    // inits flat map with weapon items
    public void initCombatState() {
        initAndGenerateTestWorld();
        createSettlers(1);
        createWeapons();
        createCombatBuildings();
        GenerationState.get().localMapGenerator.localGenConfig.localBiome = "flat";
    }

    // test world is 10x10 tiles
    private void initAndGenerateTestWorld() {
        GenerationState state = GenerationState.get();
        preLoadTypeMaps();
        state.worldGenConfig.size = 10;
        state.worldGenConfig.seed = 1;
        state.generateWorld(); // sets world map to game model
        state.preparationState.location = new Vector2Int(5, 5);
        state.preparationState.size = 100;
    }

    // creates test settler as it was selected on preparation screen
    private void createSettlers(int number) {
        SettlerDataGenerator generator = new();
        for (int i = 0; i < number; i++) {
            GenerationState.get().preparationState.settlers.Add(generator.generate());
        }
    }

    // creates test item as it was selected on preparation screen
    private void createTestItem() {
        GenerationState.get().preparationState.items.Add(new ItemData { material = "iron", type = "pickaxe", quantity = 1 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "iron", type = "axe", quantity = 1 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "iron", type = "hoe", quantity = 1 });

        GenerationState.get().preparationState.items.Add(new ItemData { material = "cotton", type = "pants", quantity = 1 });
        // GenerationState.get().preparationState.items.Add(new ItemData {material = "cotton", type = "boots", quantity = 1});
        // GenerationState.get().preparationState.items.Add(new ItemData {material = "cotton", type = "hat", quantity = 1});
        // GenerationState.get().preparationState.items.Add(new ItemData {material = "cotton", type = "gloves", quantity = 1});
        // GenerationState.get().preparationState.items.Add(new ItemData {material = "cotton", type = "shirt", quantity = 1});

        GenerationState.get().preparationState.items.Add(new ItemData { material = "marble", type = "rock", quantity = 10 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "granite", type = "rock", quantity = 10 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "sandstone", type = "rock", quantity = 10 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "oak", type = "log", quantity = 10 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "oak", type = "plank", quantity = 10 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "silk", type = "cloth_roll", quantity = 10 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "meat", type = "simple_meal", quantity = 10 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "meat", type = "meat_piece", quantity = 5 });
    }

    private void createBuildings() {
        GenerationState.get().localMapGenerator.testBuildingsToGenerate.Add("tailor's table", "oak");
        GenerationState.get().localMapGenerator.testBuildingsToGenerate.Add("kitchen", "oak");
        GenerationState.get().localMapGenerator.testItemsToStore.Add("tailor's table",
            new[] {
                "cloth_roll/silk",
                "cloth_roll/linen", "cloth_roll/wool", "cloth_roll/cotton"
            });
        // GenerationState.get().localMapGenerator.itemsToStore.Add("kitchen",
        //     new[] { "meat_piece/meat", "turnip/plant", "pumpkin/plant", "pepper/plant" });
    }

    private void createEnemy() { }

    private void createCombatBuildings() {
        GenerationState.get().localMapGenerator.testBuildingsToGenerate.Add("training dummy", "oak");
    }

    private void createWeapons() {
        GenerationState.get().preparationState.items.Add(new ItemData { material = "iron", type = "sword", quantity = 2 });
        // GenerationState.get().preparationState.items.Add(new ItemData { material = "cotton", type = "hat", quantity = 1 });
        // GenerationState.get().preparationState.items.Add(new ItemData { material = "cotton", type = "gloves", quantity = 1 });
    }

    private void preLoadTypeMaps() {
        MaterialMap.get();
        ItemTypeMap.get();
        PlantTypeMap.get();
        CreatureTypeMap.get();
        BuildingTypeMap.get();
        RecipeMap.get();
        SubstrateTypeMap.get();
    }
}
}