using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.unit;
using game.model.localmap;
using game.view.util;
using Leopotam.Ecs;
using MoreLinq;
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
            Vector3Int unitPosition = unit.pos();
            List<Vector3Int> positions = target.target.getAcceptablePositions(model)
                .Where(position => model.localMap.passageMap.inSameArea(unitPosition, position))
                .ToList();
            if (positions.Count == 0) {
                log("no acceptable positions found");
            }
            if (positions.Count > 0) {
                log("acceptable positions" + positions.Select(pos => pos.ToString()).Aggregate((s1, s2) => $"{s1} {s2}"));
                Vector3Int targetPosition = positions.MinBy(position => (unitPosition - position).sqrMagnitude);
            
                List<Vector3Int> path = 
                    model.localMap.passageMap.defaultHelper.aStar.makeShortestPath(unit.pos(), targetPosition, model);
                if (path != null) {
                    unit.Replace(new UnitMovementPathComponent { path = path });
                    return;
                }
            } 
            model.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED); // resets unit for new task
        }
    }
}