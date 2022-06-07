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
    public class MouseToolManager : Singleton<MouseToolManager> {
        private SelectorHandler selector;
        private MaterialSelectionWidgetHandler materialSelector;
        private MouseToolType tool = NONE;
        private string buildingType; // set if tool is BUILD TODO move to MouseToolType
        
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

        public static void set(string buildingType) => get()._set(BUILD, buildingType, null);

        public static void set(ConstructionType type) => get()._set(CONSTRUCT, null, type);
        
        private void _set(MouseToolType tool, string buildingType, ConstructionType constructionType) {
            GameView.get().cameraAndMouseHandler.selectionHandler.state.type = SelectionTypes.AREA; // reset selection type
            this.tool = tool;
            this.buildingType = buildingType;
            this.constructionType = constructionType;
            updateMaterialSelector();
            updateToolSprite();
        }

        private bool updateMaterialSelector() {
            if (tool == CONSTRUCT) {
                if (!materialSelector.fill(constructionType)) return false; // fail setting tool if not enough materials
                materialSelector.selectFirst();
                materialSelector.open();
                if (constructionType.name == "wall") {
                    GameView.get().cameraAndMouseHandler.selectionHandler.state.type = SelectionTypes.ROW;
                }
            } else {
                materialSelector.close();
            }
            return true;
        }

        public static void handleSelection(IntBounds3 bounds) => get().applyTool(bounds);

        public void setItemForConstruction(string itemType, int material) {
            this.itemType = itemType;
            this.material = material;
            visualMaterial = MaterialMap.variateValue(MaterialMap.get().material(material).name, itemType);
            updateToolSprite();
        }
        
        private void applyTool(IntBounds3 bounds) {
            if (tool == NONE) return; // TODO add unit/building/item/plant/block selection for NONE tool
            bounds.iterate((x, y, z) => {
                Vector3Int position = new(x, y, z);
                if (tool == CONSTRUCT) {
                    GameModel.get().designationContainer.createConstructionDesignation(position, constructionType, itemType, material);
                } else if (tool == CLEAR) { // tool clears designation
                    GameModel.get().designationContainer.cancelDesignation(position);
                } else if (tool.designation != null) { // tool applies designation
                    if (tool.designation.VALIDATOR.validate(position)) {
                        GameModel.get().designationContainer.createDesignation(position, tool.designation);
                    }
                }
            });
        }

        private void updateToolSprite() {
            if (tool == BUILD) {
                // sprite = selectSpriteByBuildingType();
                // .color = new Color(1, 1, 1, 0.5f);
            } else if (tool == CONSTRUCT) {
                selector.setConstructionSprite(selectSpriteByBlockType());
            } else {
                selector.setToolSprite(IconLoader.get(tool.iconPath));
            }
        }

        private Sprite selectSpriteByBlockType() {
            return TileSetHolder.get().getSprite(visualMaterial,
                constructionType.blockType.CODE == BlockTypes.RAMP.CODE ? "C" : constructionType.blockType.PREFIX);
        }

        private Sprite selectSpriteByBuildingType() {
            // TODO
            return null;
        }
    }
}