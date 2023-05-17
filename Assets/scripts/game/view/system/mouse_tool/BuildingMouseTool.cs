using System;
using game.model;
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
        public BuildingType type;
        private Orientations orientation;
        private Vector2Int size;
        private readonly BuildingValidator validator = new();
        private bool materialsValid;
        private bool wasValid;

        public BuildingMouseTool() {
            selectionType = SINGLE;
        }

        public override void onSelectionInToolbar() {
            fillSelectorForVariants(type.name, type.variants);
            prioritySelector.open();
            prioritySelector.init(this);
        }

        // TODO make buildings able to be designated in draw mode (like in OnI)
        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            validateBounds(bounds);
            addUpdateEvent(model => {
                Vector3Int position = bounds.getStart();
                if (validator.validateArea(position, size, model)) {
                    model.designationContainer.createBuildingDesignation(bounds.getStart(), type, orientation, itemType, material);
                } else {
                    Debug.LogErrorFormat("Position {0} for building {1} is invalid.", position, type.name);
                }
            });
        }

        // select sprite by type and rotation
        public override void updateSprite() {
            selectorGO.setBuildingSprite(BuildingTilesetHolder.get().get(type, orientation),
                type.size[OrientationUtil.isHorizontal(orientation) ? 1 : 0]);
            if (type.access != null) {
                int[] rotatedAccessPoint = getRotatedAccessPoint();
                selectorGO.setAccessPoint(rotatedAccessPoint[0], rotatedAccessPoint[1], "building_access_point");
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
            orientation = OrientationUtil.getNext(orientation);
            GameView.get().selector.changeSelectorSize(type.getSizeByOrientation(orientation));
            updateSprite();
            updateSpriteColor(new Vector3Int());
        }
        
        private int[] getRotatedAccessPoint() {
            int[] result = new int[2];
            switch (orientation) {
                case Orientations.N:
                    result[0] = type.access[0];
                    result[1] = type.access[1];
                    break;
                case Orientations.E:
                    result[0] = type.access[1];
                    result[1] = type.size[0] - 1 - type.access[0];
                    break;
                case Orientations.S:
                    result[0] = type.size[0] - 1 - type.access[0];
                    result[1] = type.size[1] - 1 - type.access[1];
                    break;
                case Orientations.W:
                    result[0] = type.size[1] - 1 - type.access[1];
                    result[1] = type.access[0];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        private void validateBounds(IntBounds3 bounds) {
            if (!bounds.isSingleTile()) {
                Debug.LogWarning("building bounds not single tile !!!");
            }
            if (GameView.get().selector.position != bounds.getStart()) {
                Debug.LogWarning("building bounds not on selector position !!!");
            }
        }
    }
}