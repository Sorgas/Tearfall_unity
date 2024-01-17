using game.model.component.task.order;
using game.view.camera;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
// debug tool that instantly creates a building
public class DebugBuildingMouseTool : AbstractBuildingMouseTool {
    
    public DebugBuildingMouseTool() {
        selectionType = SelectionType.SINGLE;
        name = "debug building mouse tool";
    }

    public override void applyTool(IntBounds3 bounds, Vector3Int start) {
        validateSelectionBounds(bounds);
        addUpdateEvent(model => {
            Vector3Int position = bounds.getStart();
            if (validator.validateArea(position, size, model)) {
                BuildingOrder order = new(type.dummyOrder);
                order.position = position;
                order.orientation = orientation;
                model.buildingContainer.createBuilding(order);
            } else {
                Debug.LogError($"Position {position} for building {type.name} is invalid.");
            }
        });
    }
}
}