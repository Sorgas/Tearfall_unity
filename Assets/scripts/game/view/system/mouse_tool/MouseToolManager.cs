using game.model;
using game.view.tilemaps;
using game.view.ui;
using game.view.ui.toolbar;
using game.view.util;
using types;
using types.building;
using UnityEngine;
using util.geometry;
using util.lang;
using static game.view.system.mouse_tool.MouseToolEnum;

namespace game.view.system.mouse_tool {
    public class MouseToolManager : Singleton<MouseToolManager> {
        private RectTransform selector;
        private MaterialSelectionWidgetHandler materialSelector;
        private MouseToolType tool = NONE;
        private string buildingType; // set if tool is BUILD
        
        // set if tool is CONSTRUCT
        private ConstructionType constructionType;
        private string itemType;
        private int material;

        public MouseToolManager() {
            selector = GameView.get().sceneObjectsContainer.selector;
            materialSelector = GameView.get().sceneObjectsContainer.materialSelectionWidgetHandler;
        }
        
        public static void set(MouseToolType tool) => get()._set(tool, null, null);

        public static void set(string buildingType) => get()._set(BUILD, buildingType, null);

        public static void set(ConstructionType type) => get()._set(CONSTRUCT, null, type);
        
        private void _set(MouseToolType tool, string buildingType, ConstructionType constructionType) {
            this.tool = tool;
            this.buildingType = buildingType;
            this.constructionType = constructionType;
            if (tool == CONSTRUCT) {
                materialSelector.fillForConstructionType(constructionType);
                materialSelector.open();
            } else {
                materialSelector.close();
            }
            updateToolSprite();
        }
        
        public static void reset() => set(NONE);

        public static void handleSelection(IntBounds3 bounds) => get()._handleSelection(bounds);

        public void setItemForConstruction(string itemType, int material) {
            this.itemType = itemType;
            this.material = material;
        }
        
        private void _handleSelection(IntBounds3 bounds) {
            if (tool == NONE) return; // TODO add unit/building/item/plant/block selection for NONE tool
            bounds.iterate((x, y, z) => {
                if (tool.designation != null) { // tool applies designation
                    if (tool.designation.VALIDATOR.validate(x, y, z)) {
                        GameModel.get().designationContainer.createDesignation(new Vector3Int(x, y, z), tool.designation);
                    }
                } else if (tool == CLEAR) { // tool clears designation
                    GameModel.get().designationContainer.cancelDesignation(new Vector3Int(x, y, z));
                }
            });
        }

        private void updateToolSprite() {
            SpriteRenderer iconRenderer = selector.gameObject.GetComponent<SelectorHandler>().toolIcon;
            Sprite sprite = null;
             if (tool == BUILD) {
                sprite = selectSpriteByBuildingType();
            } else if (tool == CONSTRUCT) {
                sprite = selectSpriteByBlockType();
            } else {
                sprite = IconLoader.get(tool.iconPath);
            }
            if (sprite != null) { // scale should be updated for non null sprite
                float width = iconRenderer.gameObject.GetComponent<RectTransform>().rect.width;
                float scale = width / sprite.rect.width * sprite.pixelsPerUnit;
                iconRenderer.transform.localScale = new Vector3(scale, scale, 1);
            }
            iconRenderer.sprite = sprite;
        }

        private Sprite selectSpriteByBlockType() {
            return TileSetHolder.get().getSprite("template", 
                constructionType.blockType.CODE == BlockTypeEnum.RAMP.CODE ? "C" : constructionType.blockType.PREFIX);
        }

        private Sprite selectSpriteByBuildingType() {
            // TODO
            return null;
        }
    }
}