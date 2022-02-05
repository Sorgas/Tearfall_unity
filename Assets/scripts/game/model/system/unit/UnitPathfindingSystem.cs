using System.Collections.Generic;
using enums.action;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using util.pathfinding;

namespace game.model.system.unit {
    // When unit has UnitMovementTargetComponent, this system finds path to target and adds UnitMovementPathComponent to unit
    // Can fail unit's task if path not found
    // TODO check unit and target areas to fail task
    public class UnitPathfindingSystem : IEcsRunSystem {
        EcsFilter<UnitComponent, UnitMovementTargetComponent>.Exclude<UnitMovementPathComponent> filter = null;

        public void Run() {
            foreach (int i in filter) {
                UnitMovementTargetComponent target = filter.Get2(i);
                ref EcsEntity unit = ref filter.GetEntity(i);
                ref UnitMovementComponent component = ref unit.Get<UnitMovementComponent>();
                findPath(ref component, target, ref unit);
            }
        }

        private void findPath(ref UnitMovementComponent component, UnitMovementTargetComponent target, ref EcsEntity unit) {
            List<Vector3Int> path = AStar.get().makeShortestPath(unit.pos(), target.target, target.targetType);
            if (path != null) {
                unit.Replace(new UnitMovementPathComponent { path = path });
            } else {
                Debug.Log("no path");
                unit.Replace(new TaskFinishedComponent { status = TaskStatusEnum.FAILED });
                unit.Del<UnitMovementTargetComponent>();
            }
        }
    }
}