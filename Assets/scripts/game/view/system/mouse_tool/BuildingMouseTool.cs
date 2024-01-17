using game.model.component.task.order;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
// creates designations for buildings
public class BuildingMouseTool : AbstractBuildingMouseTool {
    public BuildingMouseTool() {
        name = "building mouse tool";
    }

    // TODO make buildings able to be designated in draw mode (like in OnI)
    public override void applyTool(IntBounds3 bounds, Vector3Int start) {
        validateSelectionBounds(bounds);
        addUpdateEvent(model => {
            Vector3Int position = bounds.getStart();
            if (validator.validateArea(position, size, model)) {
                BuildingOrder order = new(type.dummyOrder);
                model.designationContainer.createBuildingDesignation(bounds.getStart(), orientation, order, priority);
            } else {
                Debug.LogError($"Position {position} for building {type.name} is invalid.");
            }
        });
    }
}
}