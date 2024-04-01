using game.model;
using game.model.component;
using game.model.component.task.action;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.action;
using types.unit;
using UnityEngine;
using util.geometry.bounds;
using util.lang.extension;

namespace game.view.system.mouse_tool {
    public class UnitMovementTargetTool : MouseTool {
        public EcsEntity unit;

        public UnitMovementTargetTool() {
            name = "unit movement target tool";
        }

        public override void onSelectionInToolbar() {
            base.onSelectionInToolbar();
            selectorHandler.setToolSprite(IconLoader.get("mousetool/movementTarget"));
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            if (!bounds.isSingleTile()) { 
                Debug.LogError("unit movement target is not single tile !!!");
            } 
            Vector3Int target = bounds.getStart();
            addUpdateEvent(model => {
                if (model.localMap.passageMap.getPassage(target) == PassageTypes.PASSABLE.VALUE) {
                    if (unit.Has<TaskComponent>()) {
                        GameModel.get().currentLocalModel.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED);
                        // unit.Replace(new TaskFinishedComponent { status = TaskStatusEnum.FAILED });
                    }
                    unit.Replace(new UnitNextTaskComponent { action = new MoveAction(bounds.getStart()) });
                }
            });
        }

        public override void rotate() { }

        public override void onPositionChange(Vector3Int position) {
            bool passable = GameModel.get().currentLocalModel.localMap.passageMap.passage.get(position) == PassageTypes.PASSABLE.VALUE;
            selectorHandler.setDesignationValid(passable);
        }
    }
}