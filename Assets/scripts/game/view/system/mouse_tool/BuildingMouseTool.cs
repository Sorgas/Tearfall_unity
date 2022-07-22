using System;
using game.model;
using game.model.util.validation;
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
        private BuildingValidator validator = new();
        private bool materialsValid;
        private bool wasValid;

        public BuildingMouseTool() {
            selectionType = SelectionTypes.SINGLE;
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
            bool flip = orientation == Orientations.E || orientation == Orientations.W;
            selectorGO.setBuildingSprite(selectSpriteByBuildingType(), type.size[flip ? 1 : 0]);
        }

        // validate by selector position
        public override void updateSpriteColor() {
            selectorGO.buildingValid(validate());
        }
        
        public override void rotate() {
            orientation = OrientationUtil.getNext(orientation);
            updateSelectorSize();
            updateSprite();
            updateSpriteColor();
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
        
        private bool validate() {
            EntitySelector selector = GameView.get().selector;
            return validator.validateArea(selector.position, selector.size);
        }

        private void updateSelectorSize() {
            if (orientation == Orientations.N || orientation == Orientations.S) {
                GameView.get().selector.changeSelectorSize(type.size[0], type.size[1]);
            } else {
                GameView.get().selector.changeSelectorSize(type.size[1], type.size[0]);
            }
        }
    }
}