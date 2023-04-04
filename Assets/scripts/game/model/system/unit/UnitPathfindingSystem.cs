using System.Collections.Generic;
using game.model.component;
using game.model.component.unit;
using game.model.localmap;
using game.view.util;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;
using util.pathfinding;

namespace game.model.system.unit {
    // When unit has UnitMovementTargetComponent, this system finds path to target and adds UnitMovementPathComponent to unit
    // Can fail unit's task if path not found
    public class UnitPathfindingSystem : LocalModelUnscalableEcsSystem {
        EcsFilter<UnitComponent, UnitMovementTargetComponent>.Exclude<UnitMovementPathComponent> filter = null;

        public override void Run() {
            foreach (int i in filter) {
                UnitMovementTargetComponent target = filter.Get2(i);
                ref EcsEntity unit = ref filter.GetEntity(i);
                findPath(target, ref unit);
            }
        }

        private void findPath(UnitMovementTargetComponent target, ref EcsEntity unit) {
            if (model.localMap.passageMap.tileIsAccessibleFromArea(target.target, unit.pos())) {
                List<Vector3Int> path = AStar.get().makeShortestPath(unit.pos(), target.target, target.targetType, model.localMap);
                if (path != null) {
                    unit.Replace(new UnitMovementPathComponent { path = path });
                    return;
                }
            }
            unit.Replace(new TaskFinishedComponent { status = TaskStatusEnum.FAILED });
            unit.Del<UnitMovementTargetComponent>();
        }

    }
}