using System;
using game.model;
using game.view.camera;
using game.view.tilemaps;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class BuildingMouseTool : ItemConsumingMouseTool {
        public BuildingType type;
        private Orientations orientation;

        public override bool updateMaterialSelector() {
            return fillSelectorForVariants(type.variants);
        }

        // TODO make buildings able to be designated in draw mode (like in ONI)
        public override void updateSelectionType(bool materialsOk) {
            GameView.get().cameraAndMouseHandler.selectionHandler.state.type = SelectionTypes.SINGLE;
        }

        // TODO add unit/building/item/plant/block selection for NONE tool
        public override void applyTool(IntBounds3 bounds) {
            if (bounds.minX != bounds.maxX || bounds.minY != bounds.maxY || bounds.minZ != bounds.maxZ) {
                Debug.LogError("building bounds not single tile !!!");
            }
            Vector3Int position = new(bounds.minX, bounds.minY, bounds.minZ);
            GameModel.get().designationContainer.createBuildingDesignation(position, type, orientation, itemType, material);
        }

        public override void updateSprite(bool materialsOk) {
            bool flip = orientation == Orientations.E || orientation == Orientations.W;
            selector.setBuildingSprite(selectSpriteByBuildingType(), type.size[flip ? 1 : 0]);
        }

        private Sprite selectSpriteByBuildingType() {
            switch (orientation) {
                case Orientations.N:
                    return BuildingTilesetHolder.get().sprites[type].n;
                case Orientations.E:
                    return BuildingTilesetHolder.get().sprites[type].e;
                case Orientations.S:
                    return BuildingTilesetHolder.get().sprites[type].s;
                case Orientations.W:
                    return BuildingTilesetHolder.get().sprites[type].w;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void rotate() {
            orientation = OrientationUtil.getNext(orientation);
            updateSprite(true);
        }
    }
}