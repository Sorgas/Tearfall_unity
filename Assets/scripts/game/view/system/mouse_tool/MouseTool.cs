using game.view.camera;
using game.view.ui;
using game.view.ui.toolbar;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public abstract class MouseTool {
        protected MaterialSelectionWidgetHandler materialSelector;
        protected SelectorHandler selectorGO;
        public int selectionType = SelectionTypes.AREA; // should be reset in subclasses

        protected MouseTool() {
            materialSelector = GameView.get().sceneObjectsContainer.materialSelectionWidgetHandler;
            selectorGO = GameView.get().sceneObjectsContainer.selector.GetComponent<SelectorHandler>();
        }

        public abstract bool updateMaterialSelector();

        public abstract void applyTool(IntBounds3 bounds);

        public abstract void updateSprite();

        public abstract void rotate();

        public abstract void updateSpriteColor();
    }
}