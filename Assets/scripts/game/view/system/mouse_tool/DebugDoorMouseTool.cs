using game.model.component.task.order;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
public class DebugDoorMouseTool : MouseTool {
    public override void applyTool(IntBounds3 bounds, Vector3Int start) {
        addUpdateEvent(model => {
            BuildingOrder order = new BuildingOrder(bounds.getStart(), BuildingTypeMap.get("door"), Orientations.N);
            model.buildingContainer.createBuilding(order);
        });
    }

    public override void updateSpriteColor(Vector3Int position) {
    }

    public override void reset() {
    }
}
}