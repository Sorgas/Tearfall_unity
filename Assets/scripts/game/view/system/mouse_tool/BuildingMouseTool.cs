using System;
using game.model;
using game.model.util.validation;
using game.view.tilemaps;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;
using static game.view.camera.SelectionTypes;

namespace game.view.system.mouse_tool {
    // creates designations for buildings
    public class BuildingMouseTool : ItemConsumingMouseTool {
        public BuildingType type;
        private Orientations orientation;
        private BuildingValidator validator = new();
        private bool materialsValid;
        private bool wasValid;

        public BuildingMouseTool() {
            selectionType = SINGLE;
        }

        public override bool updateMaterialSelector() {
            return fillSelectorForVariants(type.variants);
        }

        // TODO make buildings able to be designated in draw mode (like in ONI)

        // TODO add unit/building/item/plant/block selection for NONE tool
        public override void applyTool(IntBounds3 bounds) {
            if (bounds.minX != bounds.maxX || bounds.minY != bounds.maxY || bounds.minZ != bounds.maxZ) {
                Debug.LogError("building bounds not single tile !!!");
            }
            Vector3Int position = new(bounds.minX, bounds.minY, bounds.minZ);
            if (GameView.get().selector.position != position) {
                Debug.LogError("building bounds not on selector position !!!");
            }
            if (validate()) {
                GameModel.get().designationContainer.createBuildingDesignation(position, type, orientation, itemType, material);
            }
        }

        // select sprite by type and rotation
        public override void updateSprite() {
            selectorGO.setBuildingSprite(BuildingTilesetHolder.get().get(type, orientation),
                type.size[OrientationUtil.isHorisontal(orientation) ? 1 : 0]);
            int[] rotatedAccessPoint = getRotatedAccessPoint();
            selectorGO.setAccessPoint(rotatedAccessPoint[0], rotatedAccessPoint[1], "building_access_point");
        }

        // validate by selector position
        public override void updateSpriteColor(Vector3Int position) {
            selectorGO.buildingValid(validate());
        }

        public override void reset() {
            materialSelector.close();
            selectorGO.setBuildingSprite(null, 1);
            selectorGO.setAccessPoint(0,0, null);
        }

        public override void rotate() {
            orientation = OrientationUtil.getNext(orientation);
            updateSelectorSize();
            updateSprite();
            updateSpriteColor(new Vector3Int());
        }

        private bool validate() {
            EntitySelector selector = GameView.get().selector;
            return validator.validateArea(selector.position, selector.size);
        }

        private void updateSelectorSize() {
            if (!OrientationUtil.isHorisontal(orientation)) {
                GameView.get().selector.changeSelectorSize(type.size[0], type.size[1]);
            } else {
                GameView.get().selector.changeSelectorSize(type.size[1], type.size[0]);
            }
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
    }
}