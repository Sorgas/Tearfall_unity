using System;
using game.model.component;
using game.model.component.unit;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;
using static types.PassageTypes;

namespace game.model.system.unit {
// Moves unit along path created in UnitPathfindingSystem.
// If path is blocked, it will be recalculated.

// TODO when door is on the path, it should be opened first, then settler passes and closes door.
// Doors counted as 'difficult terrain' during pathfinding
// Add 'openness' value to door. This system should contribute to door openness (displayed visually)
// Animals should not be able to use doors. 

public class UnitMovementSystem : LocalModelScalableEcsSystem {
    public readonly float diagonalSpeedMod = (float)Math.Sqrt(2);
    public readonly float diagonalUpSpeedMod = (float)Math.Sqrt(2);
    public readonly float upSpeedMod = (float)Math.Sqrt(2);

    public EcsFilter<UnitMovementComponent, UnitMovementPathComponent> filter;

    protected override void runLogic(int ticks) {
        foreach (int i in filter) {
            ref UnitMovementComponent movement = ref filter.Get1(i);
            ref UnitMovementPathComponent pathComponent = ref filter.Get2(i);
            ref EcsEntity unit = ref filter.GetEntity(i);
            updateMovement(ref movement, ref pathComponent, ref unit, ticks);
        }
    }

    // checks path, accumulates unit speed and makes step to next tile of path
    private void updateMovement(ref UnitMovementComponent movement, ref UnitMovementPathComponent path,
        ref EcsEntity unit, int ticks) {
        if (!checkPath(unit, ref path, ref movement)) return;
        movement.step += movement.speed * ticks; // accumulate speed
        if (movement.step > 1f) {
            movement.step -= 1f;
            makeStep(ref path, ref unit);
        }
    }

    // checks if path is completed or blocked, removes related components
    // returns true, if path continues
    private bool checkPath(EcsEntity unit, ref UnitMovementPathComponent path, ref UnitMovementComponent movement) {
        if (path.path.Count == 0) { // path ended
            movement.step = 0;
            unit.Del<UnitMovementPathComponent>();
            unit.Del<UnitMovementTargetComponent>();
            return false;
        }
        if (model.localMap.passageMap.passage.get(path.path[0]) == IMPASSABLE.VALUE) {
            // path became blocked after finding
            movement.step = 0;
            unit.Del<UnitMovementPathComponent>(); // remove invalid path, will be found again on next tick
            return false;
        }
        return true;
    }

    // change position to next position in path
    private void makeStep(ref UnitMovementPathComponent path, ref EcsEntity unit) {
        Vector3Int current = path.path[0];
        unit.Get<PositionComponent>().position = current;
        path.path.RemoveAt(0);

        bool hasNextTile = (path.path.Count > 0);
        Vector3Int next = hasNextTile ? path.path[0] : current;
        updateVisual(unit, current, next);
    }

    // passes new current and target positions to visual component
    private void updateVisual(EcsEntity unit, Vector3Int current, Vector3Int next) {
        ref UnitVisualComponent visual = ref unit.takeRef<UnitVisualComponent>();
        visual.current = ViewUtil.fromModelToSceneForUnit(current, model);
        visual.target = ViewUtil.fromModelToSceneForUnit(next, model);
        Vector3Int direction = next - current;
        if (direction.x != 0 || direction.y != 0) {
            visual.orientation = getOrientation(direction);
        }
    }

    // TODO rework to take in account previous orientation
    private SpriteOrientations getOrientation(Vector3Int directionVector) {
        if (directionVector.y > 0) {
            if (directionVector.x > 0) return SpriteOrientations.BR;
            return SpriteOrientations.BL;
        } else {
            if (directionVector.x > 0) return SpriteOrientations.FR;
            return SpriteOrientations.FL;
        }
    }
}
}