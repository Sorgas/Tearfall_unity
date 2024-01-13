using game.model.component.task.order;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
// debug tool that instantly creates a building
public class DebugBuildingMouseTool : MouseTool {
    private string buildingType;
    
    public override void applyTool(IntBounds3 bounds, Vector3Int start) {
        addUpdateEvent(model => {
            BuildingOrder order = new BuildingOrder(bounds.getStart(), BuildingTypeMap.get(buildingType), Orientations.N);
            model.buildingContainer.createBuilding(order);
        });
    }

    public void set(string buildingType) {
        this.buildingType = buildingType;
    }
    
    public override void updateSpriteColor(Vector3Int position) {
    }

    public override void reset() {
    }
}
}