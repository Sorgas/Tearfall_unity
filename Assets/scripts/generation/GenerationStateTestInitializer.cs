using generation.unit;
using UnityEngine;

namespace generation {
// fills generation state for test level
public class GenerationStateTestInitializer {
    public void initState(int seed) {
        GenerationState state = GenerationState.get();
        state.worldGenConfig.size = 10;
        Random.InitState(seed);
        state.generateWorld(); // sets world map to game model
        createTestSettler();
        createTestItem();
        createBuildings();
        state.preparationState.location = new Vector2Int(5, 5);
        state.preparationState.size = 100;
    }

    // creates test settler as it was selected on preparation screen
    private void createTestSettler() {
        SettlerDataGenerator generator = new();
        GenerationState.get().preparationState.settlers.Add(generator.generate());
        // GenerationState.get().preparationState.settlers.Add(generator.generate());
        // GenerationState.get().preparationState.settlers.Add(generator.generate());
    }

    // creates test item as it was selected on preparation screen
    private void createTestItem() {
        GenerationState.get().preparationState.items.Add(new ItemData { material = "iron", type = "pickaxe", quantity = 1 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "iron", type = "axe", quantity = 1 });
        GenerationState.get().preparationState.items.Add(new ItemData { material = "iron", type = "hoe", quantity = 1 });
        
        GenerationState.get().preparationState.items.Add(new ItemData {material = "cotton", type = "pants", quantity = 1});
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
        GenerationState.get().preparationState.items.Add(new ItemData { material = "silk", type = "simple_meal", quantity = 10 });
        // GenerationState.get().preparationState.items.Add(new ItemData {material = "meat", type = "meat_piece", quantity = 5});
    }

    private void createBuildings() {
        GenerationState.get().localMapGenerator.buildingsToGenerate.Add("tailor's table", "oak");
        GenerationState.get().localMapGenerator.buildingsToGenerate.Add("kitchen", "oak");
        GenerationState.get().localMapGenerator.itemsToStore.Add("tailor's table",
            new[] { "cloth_roll/silk", 
                "cloth_roll/linen", "cloth_roll/wool", "cloth_roll/cotton" 
            });
        GenerationState.get().localMapGenerator.itemsToStore.Add("kitchen",
            new[] { "meat_piece/meat", "turnip/plant", "pumpkin/plant", "pepper/plant" });
    }
}
}