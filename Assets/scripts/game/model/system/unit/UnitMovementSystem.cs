using System;
using System.Collections.Generic;
using game.model.component;
using game.model.component.building;
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
    private void updateMovement(ref UnitMovementComponent movement, ref UnitMovementPathComponent path, ref EcsEntity unit, int ticks) {
        Vector3Int nextPosition = path.path[0];
        if (pathBlocked(unit, nextPosition, ref movement)) return; // cannot move
        if (movement.currentSpeed < 0) {
            pathTilesChanged(unit, ref movement, unit.pos(), nextPosition);
        }
        if (handleDoors(unit.pos(), nextPosition)) {
            movement.step += movement.currentSpeed * ticks; // accumulate speed
            if (movement.step > 1f) {
                movement.step -= 1f;
                makeStep(ref path, ref movement, ref unit);
            }
        }
    }

    // checks if path is blocked by impassable tile. 
    private bool pathBlocked(EcsEntity unit, Vector3Int position, ref UnitMovementComponent movement) {
        if (model.localMap.passageMap.passage.get(position) != IMPASSABLE.VALUE) return false;
        movement.step = 0;
        unit.Del<UnitMovementPathComponent>(); // remove invalid path, will be found again on next tick
        pathTilesChanged(unit, ref movement, unit.pos());
        return true;
    }

    // change position to next position in path
    private void makeStep(ref UnitMovementPathComponent path, ref UnitMovementComponent movement, ref EcsEntity unit) {
        Vector3Int current = path.path[0];
        unit.Get<PositionComponent>().position = current;
        path.path.RemoveAt(0);
        if (path.path.Count > 0) {
            pathTilesChanged(unit, ref movement, current, path.path[0]);
        } else {
            movement.step = 0; // reset movement
            unit.Del<UnitMovementPathComponent>();
            unit.Del<UnitMovementTargetComponent>();
            pathTilesChanged(unit, ref movement, unit.pos());
        }
    }

    // prevents door on current tile from closing
    // opens door on next tile
    // prevents door on next tile from closing
    // returns true if path not blocked by closed door
    private bool handleDoors(Vector3Int currentPosition, Vector3Int nextPosition) {
        EcsEntity currentDoor = model.buildingContainer.getBuilding(currentPosition);
        if (isDoor(currentDoor)) {
            ref BuildingDoorComponent component = ref currentDoor.takeRef<BuildingDoorComponent>();
            component.openness = 1;
            component.timeout = 3;
        }
        EcsEntity nextDoor = model.buildingContainer.getBuilding(nextPosition);
        if (isDoor(nextDoor)) {
            ref BuildingDoorComponent component = ref nextDoor.takeRef<BuildingDoorComponent>();
            component.timeout = 3;
            if (component.openness < 1) { // door is closed
                component.openness += component.openingSpeed; // TODO add multiplication to unit's work speed
                nextDoor.Replace(new VisualUpdatedComponent());
                if (component.openness < 1) return false; // still closed
                nextDoor.Replace(new BuildingDoorOpenComponent());
            }
        }
        return true;
    }

    private bool isDoor(EcsEntity entity) => entity != EcsEntity.Null && entity.Has<BuildingDoorComponent>();

    private void pathTilesChanged(EcsEntity unit, ref UnitMovementComponent movement, Vector3Int current) => pathTilesChanged(unit, ref movement, current, current); 

    // calculates speed for reaching next tile. updates sprites orientation
    private void pathTilesChanged(EcsEntity unit, ref UnitMovementComponent movement, Vector3Int current, Vector3Int next) {
        updateCurrentSpeed(ref movement, current, next);
        updateVisual(unit, current, next);
    }

    private void updateCurrentSpeed(ref UnitMovementComponent movement, Vector3Int current, Vector3Int next) {
        if (current == next) {
            movement.currentSpeed = -1;
            return;
        }
        Vector3Int direction = next - current;
        movement.currentSpeed = movement.speed;
        if (direction.z != 0) {
            if (direction.x != 0 || direction.y != 0) {
                movement.currentSpeed /= diagonalSpeedMod;
            }
        } else if (direction.x != 0 && direction.y != 0) {
            movement.currentSpeed /= diagonalSpeedMod;
        }
    }
    
    private void updateVisual(EcsEntity unit, Vector3Int current, Vector3Int next) {
        Vector3Int direction = next - current;
        ref UnitVisualComponent visual = ref unit.takeRef<UnitVisualComponent>();
        visual.current = ViewUtil.fromModelToSceneForUnit(current, model);
        visual.target = ViewUtil.fromModelToSceneForUnit(next, model);
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