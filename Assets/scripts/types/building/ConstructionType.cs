using System;
using game.model.component.task.order;
using types.item.recipe;
using util.lang;

namespace types.building {
// Represents buildable constructions. When construction is built, map block type and material changes.
// Constructions are not stored in BuildingContainer.
[Serializable]
public class ConstructionType {
    public string name;
    public MultiValueDictionary<string, Ingredient> ingredients = new(); // materials for building
    public BlockType blockType; // target block type

    public ConstructionOrder dummyOrder;
}

public class RawConstructionType {
    public string name;
    public string[] ingredients;
    public string blockTypeName;
}
}