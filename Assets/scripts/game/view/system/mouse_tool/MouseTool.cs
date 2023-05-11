using System;
using game.model;
using game.model.container;
using game.model.localmap;
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

        // called when tool is selected in MouseToolManager
        // should return false if there are not enough materials
        public virtual bool updateMaterialSelector() {
            materialSelector.close();
            return true;
        }
        
        public abstract void applyTool(IntBounds3 bounds, Vector3Int start);
        
        public virtual void updateSprite() {
            selectorGO.setToolSprite(null);
        }

        public virtual void rotate() { }

        public abstract void updateSpriteColor(Vector3Int position);

        public abstract void reset();

        protected void addUpdateEvent(Action<LocalModel> action) {
            GameModel.get().currentLocalModel.addUpdateEvent(new ModelUpdateEvent(action));
        }
    }
}