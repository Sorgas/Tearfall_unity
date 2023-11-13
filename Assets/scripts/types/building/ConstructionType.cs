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

// TODO remove
// public class BuildingVariant : IEquatable<BuildingVariant> {
//     public string itemType;
//     public int amount;
//
//     public BuildingVariant(string raw) {
//         string[] args = raw.Split("/");
//         itemType = args[0];
//         amount = int.Parse(args[1]);
//     }
//
//     public bool Equals(BuildingVariant other) {
//         if (ReferenceEquals(null, other)) return false;
//         if (ReferenceEquals(this, other)) return true;
//         return itemType == other.itemType && amount == other.amount;
//     }
//
//     public override bool Equals(object obj) {
//         if (ReferenceEquals(null, obj)) return false;
//         if (ReferenceEquals(this, obj)) return true;
//         if (obj.GetType() != this.GetType()) return false;
//         return Equals((BuildingVariant)obj);
//     }
//
//     public override int GetHashCode() {
//         return HashCode.Combine(itemType, amount);
//     }
// }

public class RawConstructionType {
    public string name;
    public string[] ingredients;
    public string blockTypeName;
}
}