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
    protected SelectorHandler selectorHandler;
    public SelectionType selectionType = AREA; // should be reset in subclasses
    public int priority = TaskPriorities.JOB;
    public string name = "mouse tool";

    protected MouseTool() {
        materialSelector = GameView.get().sceneElements.materialSelectionWidgetHandler;
        prioritySelector = GameView.get().sceneElements.prioritySelectionWidgetHandler;
        selectorHandler = GameView.get().sceneElements.selector.GetComponent<SelectorHandler>();
    }

    // called when tool is selected in MouseToolManager
    public virtual void onSelectionInToolbar() {
        materialSelector.close();
        prioritySelector.close();
        selectorHandler.clear();
    }

    // called when mouse changes position and rotation
    public abstract void onPositionChange(Vector3Int position);

    // should apply tool changes to localMap
    public abstract void applyTool(IntBounds3 bounds, Vector3Int start);

    // called when player presses rotate keys
    public virtual void rotate() { }

    protected void addUpdateEvent(Action<LocalModel> action) {
        GameModel.get().currentLocalModel.addUpdateEvent(new ModelUpdateEvent(action));
    }
}
}