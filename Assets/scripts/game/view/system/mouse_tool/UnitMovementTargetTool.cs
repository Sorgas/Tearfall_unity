using game.model;
using game.model.component;
using game.model.component.task.action;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class UnitMovementTargetTool : MouseTool {
        public EcsEntity unit;

        public override bool updateMaterialSelector() {
            materialSelector.close();
            return true;
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            if (!bounds.isSingleTile()) { Debug.LogError("unit movement target is not single tile !!!");
            } 
            Vector3Int target = bounds.getStart();
            addUpdateEvent(model => {
                if (model.localMap.passageMap.getPassage(target) == PassageTypes.PASSABLE.VALUE) {
                    if (unit.Has<TaskComponent>()) {
                        unit.Replace(new TaskFinishedComponent { status = TaskStatusEnum.FAILED });
                    }
                    unit.Replace(new UnitNextTaskComponent { action = new MoveAction(bounds.getStart()) });
                }
            });
        }

        public override void updateSprite() {
            selectorGO.setToolSprite(IconLoader.get("mousetool/movementTarget"));
        }

        public override void rotate() { }

        public override void updateSpriteColor(Vector3Int position) {
            bool passable = GameModel.get().currentLocalModel.localMap.passageMap.passage.get(position) == PassageTypes.PASSABLE.VALUE;
            selectorGO.designationValid(passable);
        }

        public override void reset() {
            materialSelector.close();
            selectorGO.setToolSprite(null);
        }
    }
}