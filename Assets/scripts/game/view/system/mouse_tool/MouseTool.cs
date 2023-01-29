using game.view.camera;
using game.view.ui;
using game.view.ui.toolbar;
using UnityEngine;
using util.geometry.bounds;
using static game.view.camera.SelectionType;

namespace game.view.system.mouse_tool {
    public abstract class MouseTool {
        protected MaterialSelectionWidgetHandler materialSelector;
        protected SelectorHandler selectorGO;
        public SelectionType selectionType = AREA; // should be reset in subclasses

        protected MouseTool() {
            materialSelector = GameView.get().sceneObjectsContainer.materialSelectionWidgetHandler;
            selectorGO = GameView.get().sceneObjectsContainer.selector.GetComponent<SelectorHandler>();
        }

        public abstract bool updateMaterialSelector();

        public abstract void applyTool(IntBounds3 bounds, Vector3Int start);

        public abstract void updateSprite();

        public abstract void rotate();

        public abstract void updateSpriteColor(Vector3Int position);

        public abstract void reset();
    }
}