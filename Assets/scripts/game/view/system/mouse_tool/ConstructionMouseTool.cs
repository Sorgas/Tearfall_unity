using game.model;
using game.model.component.task.order;
using game.model.util.validation;
using game.view.tilemaps;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;
using static game.view.camera.SelectionType;

namespace game.view.system.mouse_tool {
// Allows player to create designations on the map. Designations will create tasks for building constructions. 
public class ConstructionMouseTool : ItemConsumingMouseTool {
    public ConstructionType type;
    private ConstructionValidator validator = new();

    public ConstructionMouseTool() {
        name = "construction mouse tool";
    }

    public override void onSelectionInToolbar() {
        fillSelector(type.dummyOrder); // fills selector by dummy order in ConstructionType
        prioritySelector.open();
        prioritySelector.setForTool(this);
    }

    public void set(ConstructionType type) {
        this.type = type;
        selectionType = type.name == "wall" ? ROW : AREA;
    }

    public override void applyTool(IntBounds3 bounds, Vector3Int start) {
        if (!hasMaterials) {
            Debug.LogWarning("no materials for construction");
            return;
        }
        addUpdateEvent(model => {
            bounds.iterate((x, y, z) => {
                Vector3Int position = new(x, y, z);
                Debug.Log(position);
                model.designationContainer.createConstructionDesignation(position, type.dummyOrder, priority);
            });
        });
    }

    public override void rotate() { }

    public override void onPositionChange(Vector3Int position) {
        Sprite sprite = BlockTileSetHolder.get().getSprite("template", type.blockType.CODE == BlockTypes.RAMP.CODE ? "C" : type.blockType.PREFIX);
        selectorHandler.setConstructionSprite(sprite);
        selectorHandler.setConstructionValid(validate(position));
    }

    public bool validate(Vector3Int position) {
        return validator.validateForConstruction(position.x, position.y, position.z, type, GameModel.get().currentLocalModel);
    }
}
}