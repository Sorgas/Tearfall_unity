using game.model;
using game.view.ui;
using UnityEngine;
using util.geometry;
using util.lang;
using static game.view.system.mouse_tool.MouseToolEnum;

namespace game.view.system.mouse_tool {
    public class MouseToolManager : Singleton<MouseToolManager> {
        private MouseToolType currentTool = NONE;
        
        public static void set(MouseToolType newTool) => get()._set(newTool);

        public static void reset() => get()._reset();

        public static void handleSelection(IntBounds3 bounds) => get()._handleSelection(bounds);

        private void _set(MouseToolType newTool) {
            currentTool = newTool;
            updateToolSprite();
        }
        
        private void _reset() {
            _set(NONE);
        }
        
        private void _handleSelection(IntBounds3 bounds) {
            if (currentTool == NONE) return; // TODO add unit/building/item/plant/block selection for NONE tool
            bounds.iterate((x, y, z) => {
                if (currentTool.designation != null) { // tool applies designation
                    if (currentTool.designation.VALIDATOR.validate(x, y, z)) {
                        GameModel.get().designationContainer.createDesignation(new Vector3Int(x, y, z), currentTool.designation);
                    }
                } else if (currentTool == CLEAR) {
                    GameModel.get().designationContainer.cancelDesignation(new Vector3Int(x, y, z));
                }
            });
        }

        private void updateToolSprite() {
            Sprite sprite = currentTool.sprite;
            SpriteRenderer iconRenderer = GameView.get().selector.gameObject.GetComponent<SelectorHandler>().toolIcon;
            if (sprite != null) { // scale should be updated for non null sprite
                float width = iconRenderer.gameObject.GetComponent<RectTransform>().rect.width;
                float scale = width / sprite.rect.width * sprite.pixelsPerUnit;
                iconRenderer.transform.localScale = new Vector3(scale, scale, 1);
            }
            iconRenderer.sprite = sprite;
        }
    }
}