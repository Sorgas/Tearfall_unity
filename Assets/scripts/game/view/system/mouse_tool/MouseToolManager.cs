using game.model.component;
using game.view.ui;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;
using util.lang;
using static game.model.component.task.DesignationComponents;

namespace game.view.system.mouse_tool {
    public class MouseToolManager : Singleton<MouseToolManager> {
        private MouseToolType currentTool;

        private void _set(MouseToolType newTool) {
            currentTool = newTool;
            Sprite sprite = currentTool.sprite;
            SpriteRenderer iconRenderer = GameView.get().selector.gameObject.GetComponent<SelectorHandler>().icon;
            float width = iconRenderer.gameObject.GetComponent<RectTransform>().rect.width;
            float scale = width / sprite.rect.width * sprite.pixelsPerUnit;
            iconRenderer.transform.localScale = new Vector3(scale, scale,1);
            iconRenderer.sprite = sprite;
        }

        public static void createDesignation() {
            
        }

        public void _handleSelection(IntBounds3 bounds) {
            bounds.iterate((x,y,z) => {
                if (currentTool.validator.validate(x, y, z)) {
                    EcsEntity entity = new EcsEntity();
                    entity.Replace(new DesignationComponent { type = currentTool.designation });
                    entity.Replace(new PositionComponent { position = new Vector3Int(x, y, z)});
                    entity.Replace(new OpenDesignation());
                }
            });
        }

        public static void handleSelection(IntBounds3 bounds) => get()._handleSelection(bounds);
        
        private void _reset() {
            _set(MouseToolEnum.NONE);
        }

        public static void set(MouseToolType newTool) => get()._set(newTool);

        public static void reset() => get()._reset();

    }
}