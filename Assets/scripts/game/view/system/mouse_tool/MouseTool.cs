using game.view.ui;
using game.view.ui.toolbar;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public abstract class MouseTool {
        protected MaterialSelectionWidgetHandler materialSelector;
        protected SelectorHandler selector;
        protected string itemType;
        protected int material;

        protected MouseTool() {
            materialSelector = GameView.get().sceneObjectsContainer.materialSelectionWidgetHandler;
            selector = GameView.get().sceneObjectsContainer.selector.GetComponent<SelectorHandler>();
        }

        public abstract bool updateMaterialSelector();

        public abstract void updateSelectionType(bool materialsOk);

        public abstract void applyTool(IntBounds3 bounds);

        public abstract void updateSprite(bool materialsOk);

        public void rotate() {
            
        }
    }
}