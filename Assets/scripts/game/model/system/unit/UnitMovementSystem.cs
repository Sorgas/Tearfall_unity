using enums;
using enums.action;
using game.model.component.task;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.pathfinding;
using static enums.PassageEnum;

namespace game.model.system.unit {
    // finds path for units and moves them along path
    // can fail task when path gets blocked
    // TODO split into pathfinding system and movement system
    // TODO check unit and target areas to fail task
    public class UnitMovementSystem : IEcsRunSystem {
        EcsFilter<MovementComponent, MovementTargetComponent> filter = null;

        public void Run() {
            foreach (int i in filter) {
                ref MovementComponent component = ref filter.Get1(i);
                MovementTargetComponent target = filter.Get2(i);
                ref EcsEntity unit = ref filter.GetEntity(i);
                updateMovement(ref component, target, ref unit);
            }
        }

        private void updateMovement(ref MovementComponent component, MovementTargetComponent target, ref EcsEntity unit) {
            if (component.path == null || component.path.Count == 0) { // target w/o path, create path
                component.path = AStar.get().makeShortestPath(component.position, target.target, target.targetType);
                if(component.path == null) {
                    Debug.Log("no path");
                    failTask(unit);
                }
            } else if (component.path.Count > 0) { // not empty path exists
                if (GameModel.localMap.passageMap.passage.get(component.path[0]) == IMPASSABLE.VALUE) {
                    failTask(unit);
                } else {
                    component.step += component.speed; // accumulate speed
                    if (component.step > 1f) {
                        component.step -= 1f;
                        makeStep(ref component, ref unit);
                    }
                }
            }
        }

        // change position to next position in path
        private void makeStep(ref MovementComponent component, ref EcsEntity unit) {
            updateOrientation(ref component);
            component.position = component.path[0];
            component.path.RemoveAt(0);
            if(component.path.Count == 0) { // path finished
                unit.Del<MovementTargetComponent>(); // remove target
            }
        }

        private void updateOrientation(ref MovementComponent component) {
            Vector3Int direction = component.path[0] - component.position;
            if (direction.x != 0 || direction.y != 0) {
                component.orientation = getOrientation(direction);
            }
        }

        private OrientationEnum getOrientation(Vector3Int directionVector) {
            if (directionVector.x < 0) return OrientationEnum.W;
            if (directionVector.x > 0) return OrientationEnum.E;
            return directionVector.y < 0 ? OrientationEnum.S : OrientationEnum.N;
        }

        private void failTask(EcsEntity unit) {
            unit.Replace(new TaskComponents.TaskStatusComponent { status = TaskStatusEnum.FAILED });
            unit.Del<MovementTargetComponent>();
        }
    }
}