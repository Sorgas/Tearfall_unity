using System;
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
    // creates designations for buildings
    public class BuildingMouseTool : ItemConsumingMouseTool {
        private BuildingType type;
        private Orientations orientation;
        private Vector2Int size;
        private readonly BuildingValidator validator = new();
        private bool materialsValid;
        private bool wasValid;

        public BuildingMouseTool() {
            selectionType = SINGLE;
        }

        public void setBuildingType(BuildingType newType) {
            type = newType;
            orientation = Orientations.N;
            size = type.getSizeByOrientation(orientation);
        }
        
        public override void onSelectionInToolbar() {
            fillSelector(type.dummyOrder);
            prioritySelector.open();
            prioritySelector.setForTool(this);
        }

        // TODO make buildings able to be designated in draw mode (like in OnI)
        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            validateSelectionBounds(bounds);
            addUpdateEvent(model => {
                Vector3Int position = bounds.getStart();
                if (validator.validateArea(position, size, model)) {
                    BuildingOrder order = new(type.dummyOrder);
                    model.designationContainer.createBuildingDesignation(bounds.getStart(), order, priority);
                } else {
                    Debug.LogError($"Position {position} for building {type.name} is invalid.");
                }
            });
        }

        // select sprite by type and rotation
        public override void updateSprite() {
            selectorGO.setBuildingSprite(BuildingTilesetHolder.get().get(type, orientation),
                type.size[OrientationUtil.isHorizontal(orientation) ? 1 : 0]);
            if (type.access != null) {
                Vector3Int offset = type.getAccessOffsetByRotation(orientation);
                selectorGO.setAccessPoint(offset.x, offset.y, "building_access_point");
            }
        }

        // validate by selector position
        public override void updateSpriteColor(Vector3Int position) {
            selectorGO.buildingValid(validator.validateArea(position, size, GameModel.get().currentLocalModel));
        }

        public override void reset() {
            materialSelector.close();
            selectorGO.setBuildingSprite(null, 1);
            selectorGO.setAccessPoint(0, 0, null);
        }

        public override void rotate() {
            updateOrientationAndSize(OrientationUtil.getNext(orientation));
            GameView.get().selector.changeSelectorSize(size);
            updateSprite();
            updateSpriteColor(new Vector3Int());
        }

        private void updateOrientationAndSize(Orientations newOrientation) {
            orientation = newOrientation;
            size = type.getSizeByOrientation(orientation);
        }

        private void validateSelectionBounds(IntBounds3 bounds) {
            if (!bounds.isSingleTile()) {
                Debug.LogWarning("building selection bounds not single tile !!!");
            }
            if (GameView.get().selector.position != bounds.getStart()) {
                Debug.LogWarning("building selection bounds not on selector position !!!");
            }
        }
    }
}