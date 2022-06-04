using enums;
using game.model.component;
using game.model.component.unit;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;
using static types.PassageTypes;

namespace game.model.system.unit {
    // Moves unit along path created in UnitPathfindingSystem. If path is blocked, it will be recalculated. 
    public class UnitMovementSystem : IEcsRunSystem {
        EcsFilter<UnitMovementComponent, UnitMovementPathComponent> filter = null;

        public void Run() {
            foreach (int i in filter) {
                ref UnitMovementComponent component = ref filter.Get1(i);
                ref UnitMovementPathComponent pathComponent = ref filter.Get2(i);
                ref EcsEntity unit = ref filter.GetEntity(i);
                updateMovement(ref component, ref pathComponent, ref unit);
            }
        }
        
        private void updateMovement(ref UnitMovementComponent component, ref UnitMovementPathComponent path, ref EcsEntity unit) {
            if (path.path.Count == 0) { // path ended, finish movement
                unit.Del<UnitMovementPathComponent>();
                unit.Del<UnitMovementTargetComponent>();
                return;
            }
            if (GameModel.localMap.passageMap.passage.get(path.path[0]) == IMPASSABLE.VALUE) { // path became blocked after finding
                unit.Del<UnitMovementPathComponent>(); // remove invalid path, will be found again on next tick
                return;
            }
            component.step += component.speed; // accumulate speed
            if (component.step > 1f) {
                component.step -= 1f;
                makeStep(ref component, ref path, ref unit);
            }
        }

        // change position to next position in path
        private void makeStep(ref UnitMovementComponent movementComponent, ref UnitMovementPathComponent pathComponent, ref EcsEntity unit) {
            updateOrientation(ref movementComponent, ref pathComponent, unit);
            unit.Get<PositionComponent>().position = pathComponent.path[0];
            pathComponent.path.RemoveAt(0);
        }

        private void updateOrientation(ref UnitMovementComponent component, ref UnitMovementPathComponent path, EcsEntity unit) {
            Vector3Int direction = path.path[0] - unit.pos();
            if (direction.x != 0 || direction.y != 0) {
                component.orientation = getOrientation(direction);
            }
        }

        private Orientations getOrientation(Vector3Int directionVector) {
            if (directionVector.x < 0) return Orientations.W;
            if (directionVector.x > 0) return Orientations.E;
            return directionVector.y < 0 ? Orientations.S : Orientations.N;
        }
    }
}