using UnityEngine;

namespace game.model.component.task.order {
// order for construction and building. defines number of items of 1 material and item type
public class GenericBuildingOrder : AbstractItemConsumingOrder {
    public Vector3Int position; // targetPosition

    public GenericBuildingOrder(Vector3Int position) : base() {
        this.position = position;
    }

    protected GenericBuildingOrder(GenericBuildingOrder source) : base(source) {
        position = source.position;
    }
}
}