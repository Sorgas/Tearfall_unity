using types;
using UnityEngine;

namespace game.model.component.task.order {
public class ConstructionOrder : GenericBuildingOrder {
    public readonly BlockType blockType;

    public ConstructionOrder(BlockType blockType, Vector3Int position) : base(position) {
        this.blockType = blockType;
    }

    public ConstructionOrder(ConstructionOrder source) : base(source) {
        blockType = source.blockType;
    }
}
}