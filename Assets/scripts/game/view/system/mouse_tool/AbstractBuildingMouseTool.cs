using game.model;
using game.model.util.validation;
using game.view.camera;
using game.view.tilemaps;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
// Parent class for BuildingMouseTool and DebugBuildingMouseTool
public abstract class AbstractBuildingMouseTool : ItemConsumingMouseTool {
    protected BuildingType type;
    protected Orientations orientation;
    protected Vector2Int size;
    protected readonly BuildingValidator validator = new();
    private bool materialsValid;
    private bool wasValid;

    public AbstractBuildingMouseTool() {
        selectionType = SelectionType.SINGLE;
    }

    public void setBuildingType(BuildingType newType) {
        type = newType;
        updateOrientationAndSize(Orientations.N);
    }

    public override void onSelectionInToolbar() {
        fillSelector(type.dummyOrder);
        prioritySelector.open();
        prioritySelector.setForTool(this);
        updateSprite();
    }

    // validate by selector position
    public override void onPositionChange(Vector3Int position) {
        selectorHandler.setConstructionValid(validator.validateArea(position, size, GameModel.get().currentLocalModel));
    }

    public override void rotate() {
        Debug.Log($"rotating {name}");
        updateOrientationAndSize(OrientationUtil.getNext(orientation));
        GameView.get().selector.changeSelectorSize(size);
        updateSprite();
        onPositionChange(new Vector3Int());
    }

    // select sprite by type and rotation
    private void updateSprite() {
        selectorHandler.setBuildingSprite(BuildingTilesetHolder.get().get(type, orientation, 0),
            type.size[OrientationUtil.isHorizontal(orientation) ? 1 : 0]);
        if (type.access != null) {
            Vector3Int offset = type.getAccessOffsetByRotation(orientation);
            selectorHandler.setAccessPoint(offset.x, offset.y, "building_access_point");
        }
    }

    private void updateOrientationAndSize(Orientations newOrientation) {
        orientation = newOrientation;
        size = type.getSizeByOrientation(orientation);
    }

    protected void validateSelectionBounds(IntBounds3 bounds) {
        if (!bounds.isSingleTile()) {
            Debug.LogWarning("building selection bounds not single tile !!!");
        }
        if (GameView.get().selector.position != bounds.getStart()) {
            Debug.LogWarning("building selection bounds not on selector position !!!");
        }
    }
}
}