using System;
using game.model.component;
using game.model.component.unit;
using game.model.localmap;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;
using static types.PassageTypes;

namespace game.model.system.unit {
    // Moves unit along path created in UnitPathfindingSystem.
    // If path is blocked, it will be recalculated. 
    public class UnitMovementSystem : LocalModelEcsSystem {
        public readonly float diagonalSpeedMod = (float)Math.Sqrt(2);
        public readonly float diagonalUpSpeedMod = (float)Math.Sqrt(2);
        public readonly float upSpeedMod = (float)Math.Sqrt(2);

        EcsFilter<UnitMovementComponent, UnitMovementPathComponent> filter = null;


        public UnitMovementSystem(LocalModel model) : base(model) { }

        public override void Run() {
            float delta = Time.deltaTime;
            foreach (int i in filter) {
                ref UnitMovementComponent movement = ref filter.Get1(i);
                ref UnitMovementPathComponent pathComponent = ref filter.Get2(i);
                ref EcsEntity unit = ref filter.GetEntity(i);
                updateMovement(ref movement, ref pathComponent, ref unit, delta);
            }
        }

        private void updateMovement(ref UnitMovementComponent movement, ref UnitMovementPathComponent path, ref EcsEntity unit, float delta) {
            if (path.path.Count == 0) { // path ended, finish movement
                unit.Del<UnitMovementPathComponent>();
                unit.Del<UnitMovementTargetComponent>();
                return;
            }
            if (model.localMap.passageMap.passage.get(path.path[0]) == IMPASSABLE.VALUE) { // path became blocked after finding
                movement.step = 0;
                unit.Del<UnitMovementPathComponent>(); // remove invalid path, will be found again on next tick
                return;
            }
            
            ref UnitVisualComponent visual = ref unit.takeRef<UnitVisualComponent>();
            visual.target = ViewUtil.fromModelToSceneForUnit(path.path[0], model) - new Vector3(0,0,1f);
            
            movement.step += movement.speed; // accumulate speed
            if (movement.step > 1f) {
                movement.step -= 1f;
                makeStep(ref movement, ref path, ref unit);
            }
        }

        // change position to next position in path
        private void makeStep(ref UnitMovementComponent movementComponent, ref UnitMovementPathComponent path, ref EcsEntity unit) {
            unit.Get<PositionComponent>().position = path.path[0];
            path.path.RemoveAt(0);
            Vector3Int nextTarget = path.path.Count > 0 ? path.path[0] : unit.pos();
            updateVisual(unit, nextTarget);
        }

        // updates target and orientation for visual movement
        private void updateVisual(EcsEntity unit, Vector3Int nextTarget) {
            ref UnitVisualComponent visual = ref unit.takeRef<UnitVisualComponent>();
            Vector3Int pos = unit.pos();
            visual.current = ViewUtil.fromModelToSceneForUnit(pos, model);
            visual.target = ViewUtil.fromModelToSceneForUnit(nextTarget, model);
            
            Vector3Int direction = nextTarget - pos;
            if (direction.x != 0 || direction.y != 0) {
                visual.orientation = getOrientation(direction);
            }
        }
        
        private Orientations getOrientation(Vector3Int directionVector) {
            if (directionVector.x < 0) return Orientations.W;
            if (directionVector.x > 0) return Orientations.E;
            return directionVector.y < 0 ? Orientations.S : Orientations.N;
        }
    }
}