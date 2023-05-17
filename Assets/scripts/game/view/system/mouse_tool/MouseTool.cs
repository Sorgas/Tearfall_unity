using System;
using game.model;
using game.model.container;
using game.model.localmap;
using game.view.camera;
using game.view.ui;
using game.view.ui.toolbar;
using types.action;
using UnityEngine;
using util.geometry.bounds;
using static game.view.camera.SelectionType;

namespace game.view.system.mouse_tool {
    public abstract class MouseTool {
        protected MaterialSelectionWidgetHandler materialSelector;
        protected PrioritySelectionWidgetHandler prioritySelector;
        protected SelectorHandler selectorGO;
        public SelectionType selectionType = AREA; // should be reset in subclasses
        public int priority = TaskPriorities.JOB;
        
        protected MouseTool() {
            materialSelector = GameView.get().sceneObjectsContainer.materialSelectionWidgetHandler;
            prioritySelector = GameView.get().sceneObjectsContainer.prioritySelectionWidgetHandler;
            selectorGO = GameView.get().sceneObjectsContainer.selector.GetComponent<SelectorHandler>();
        }

        // called when tool is selected in MouseToolManager
        public virtual void onSelectionInToolbar() {
            materialSelector.close();
            prioritySelector.close();
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