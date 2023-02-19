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

        // should recreate item buttons in material selector widget. called when tool is selected in MouseToolManager
        public abstract bool updateMaterialSelector();
        
        public abstract void applyTool(IntBounds3 bounds, Vector3Int start);
        
        public virtual void updateSprite() {
            selectorGO.setToolSprite(null);
        }

        public virtual void rotate() { }

        public abstract void updateSpriteColor(Vector3Int position);

        public abstract void reset();
    }
}