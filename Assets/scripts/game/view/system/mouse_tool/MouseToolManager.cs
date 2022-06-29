using System;
using game.model;
using game.view.camera;
using game.view.tilemaps;
using game.view.ui;
using game.view.ui.toolbar;
using game.view.util;
using types;
using types.building;
using types.material;
using UnityEngine;
using util.geometry.bounds;
using util.lang;
using static game.view.system.mouse_tool.MouseToolTypes;

namespace game.view.system.mouse_tool {
    // TODO split into different tools
    public class MouseToolManager : Singleton<MouseToolManager> {
        private SelectorHandler selector;
        private MaterialSelectionWidgetHandler materialSelector;
        private MouseToolType tool = NONE;

        // set if tool is BUILD TODO move to MouseToolType
        private BuildingType buildingType;
        private Orientations orientation;

        // set if tool is CONSTRUCT TODO move to MouseToolType
        private ConstructionType constructionType;
        private string itemType;
        private int material;
        private string visualMaterial;

        public MouseToolManager() {
            selector = GameView.get().sceneObjectsContainer.selector.GetComponent<SelectorHandler>();
            materialSelector = GameView.get().sceneObjectsContainer.materialSelectionWidgetHandler;
        }

        public static void set(MouseToolType tool) => get()._set(tool, null, null);

        public static void set(BuildingType buildingType) => get()._set(BUILD, buildingType, null);

        public static void set(ConstructionType type) => get()._set(CONSTRUCT, null, type);

        private void _set(MouseToolType tool, BuildingType buildingType, ConstructionType constructionType) {
            this.tool = tool;
            this.buildingType = buildingType;
            this.constructionType = constructionType;
            bool materialsOk = updateMaterialSelector(); // enough items for building or items not required
            updateSelectionType(materialsOk);
            updateToolSprite(materialsOk);
        }

        // returns true, if enough materials for construction
        private bool updateMaterialSelector() {
            if (tool == CONSTRUCT) return fillSelectorForVariants(constructionType.variants);
            if (tool == BUILD) return fillSelectorForVariants(buildingType.variants);
            materialSelector.close();
            return true;
        }

        // TODO make buildings able to be designated in draw mode (like in ONI)
        private void updateSelectionType(bool hasMaterials) {
            GameView.get().cameraAndMouseHandler.selectionHandler.state.type = SelectionTypes.AREA; // set default type
            if (tool == CONSTRUCT) {
                if (constructionType.name == "wall") {
                    GameView.get().cameraAndMouseHandler.selectionHandler.state.type = SelectionTypes.ROW;
                }
            } else if (tool == BUILD) {
                GameView.get().cameraAndMouseHandler.selectionHandler.state.type = SelectionTypes.SINGLE;
            }
        }

        private bool fillSelectorForVariants(BuildingVariant[] variants) {
            bool hasMaterials = materialSelector.fill(variants); // do not set tool if not enough materials
            materialSelector.selectFirst();
            materialSelector.open();
            return hasMaterials;
        }

        public static void handleSelection(IntBounds3 bounds) => get().applyTool(bounds);

        public void setItemForConstruction(string itemType, int material) {
            this.itemType = itemType;
            this.material = material;
            visualMaterial = MaterialMap.variateValue(MaterialMap.get().material(material).name, itemType);
            updateToolSprite(true);
        }

        public void rotateBuilding() {
            if (tool == BUILD) orientation = OrientationUtil.getNext(orientation);
        }
        
        private void applyTool(IntBounds3 bounds) {
            if (tool == NONE) return; // TODO add unit/building/item/plant/block selection for NONE tool
            bounds.iterate((x, y, z) => {
                Vector3Int position = new(x, y, z);
                if (tool == CONSTRUCT) {
                    GameModel.get().designationContainer.createConstructionDesignation(position, constructionType, itemType, material);
                } else if (tool == CLEAR) {
                    // tool clears designation
                    GameModel.get().designationContainer.cancelDesignation(position);
                } else if (tool.designation != null) {
                    // tool applies designation
                    if (tool.designation.VALIDATOR.validate(position)) {
                        GameModel.get().designationContainer.createDesignation(position, tool.designation);
                    }
                }
            });
        }

        private void updateToolSprite(bool ok) {
            if (tool == BUILD) {
                bool flip = orientation == Orientations.E || orientation == Orientations.W;
                selector.setBuildingSprite(selectSpriteByBuildingType(), buildingType.size[flip ? 1 : 0]);
            } else if (tool == CONSTRUCT) {
                selector.setConstructionSprite(selectSpriteByBlockType());
            } else {
                selector.setToolSprite(IconLoader.get(tool.iconPath));
            }
        }

        private Sprite selectSpriteByBlockType() {
            return BlockTileSetHolder.get().getSprite(visualMaterial,
                constructionType.blockType.CODE == BlockTypes.RAMP.CODE ? "C" : constructionType.blockType.PREFIX);
        }

        private Sprite selectSpriteByBuildingType() {
            switch (orientation) {
                case Orientations.N:
                    return BuildingTilesetHolder.get().sprites[buildingType].n;
                case Orientations.E:
                    return BuildingTilesetHolder.get().sprites[buildingType].e;
                case Orientations.S:
                    return BuildingTilesetHolder.get().sprites[buildingType].s;
                case Orientations.W:
                    return BuildingTilesetHolder.get().sprites[buildingType].w;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}