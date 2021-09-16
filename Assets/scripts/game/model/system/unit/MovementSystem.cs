using Assets.scripts.util.pathfinding;
using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.enums;
using UnityEngine;

public class MovementSystem : IEcsRunSystem {
    EcsFilter<MovementComponent, MovementTargetComponent> filter = null;

    public void Run() {
        foreach (int i in filter) {
            MovementComponent component = filter.Get1(i);
            MovementTargetComponent target = filter.Get2(i);
            EcsEntity unit = filter.GetEntity(i);
            updateMovement(component, target);
        }
    }

    private void updateMovement(MovementComponent component, MovementTargetComponent target) {
        if (component.path == null) { // target wo path, create path
            component.path = AStar.get().makeShortestPath(component.position, target.target, component.targetType);
            // TODO if no path found, cancel task
        } else if (component.path.Count > 0) { // not empty path exists
            component.step += component.speed;
            if (component.step > 1f) {
                component.step -= 1f;
                makeStep(component);
            }
        }
    }

    // change position to next position in path
    private void makeStep(MovementComponent component) {
        updateOrientation(component);
        component.position = component.path[0];
        component.path.RemoveAt(0);
    }

    private void updateOrientation(MovementComponent component) {
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
}