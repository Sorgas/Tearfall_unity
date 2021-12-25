using game.view.ui;
using UnityEngine;
using util.lang;

namespace game.view.system {
    public class MouseToolManager : Singleton<MouseToolManager> {
        private MouseToolEnum currentTool;

        private void _set(MouseToolEnum newTool) {
            currentTool = newTool;
            Sprite sprite;
            sprite = currentTool == MouseToolEnum.NONE
                ? null
                : MouseToolUtil.getSprite(currentTool);
            SpriteRenderer iconRenderer = GameView.get().selector.gameObject.GetComponent<SelectorHandler>().icon;
            float width = iconRenderer.gameObject.GetComponent<RectTransform>().rect.width;
            float scale = width / sprite.rect.width * sprite.pixelsPerUnit;
            iconRenderer.transform.localScale = new Vector3(scale, scale,1);
            iconRenderer.sprite = sprite;
        }

        public static void createDesignation() {

        }

        private void _reset() {
            _set(MouseToolEnum.NONE);
        }

        public static void set(MouseToolEnum newTool) => get()._set(newTool);

        public static void reset() => get()._reset();

    }
}