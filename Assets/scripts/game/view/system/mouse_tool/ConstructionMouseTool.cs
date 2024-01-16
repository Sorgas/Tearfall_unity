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

    public override void updateSprite() {
        Sprite sprite = BlockTileSetHolder.get().getSprite("template", type.blockType.CODE == BlockTypes.RAMP.CODE ? "C" : type.blockType.PREFIX);
        selectorGO.setConstructionSprite(sprite);
    }

    public override void rotate() { }

    public override void updateSpriteColor(Vector3Int position) {
        selectorGO.buildingValid(validate(position));
    }

    public override void reset() {
        materialSelector.close();
        selectorGO.setConstructionSprite(null);
        selectionType = AREA;
    }

    public bool validate(Vector3Int position) {
        return validator.validateForConstruction(position.x, position.y, position.z, type, GameModel.get().currentLocalModel);
    }
}
}