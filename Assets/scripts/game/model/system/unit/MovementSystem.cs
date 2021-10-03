using Assets.scripts.util.pathfinding;
using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.enums;
using UnityEngine;

public class MovementSystem : IEcsRunSystem {
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
            component.path = AStar.get().makeShortestPath(component.position, target.target, component.targetType);
            // Debug.Log(component.path.Count);
            if(component.path == null) {
                Debug.Log("no path");
            }
            // TODO if no path found, cancel task
        } else if (component.path.Count > 0) { // not empty path exists
            // Debug.Log("moving");
            component.step += component.speed;
            if (component.step > 1f) {
                component.step -= 1f;
                makeStep(ref component, ref unit);
            }
        }
    }

    // change position to next position in path
    private void makeStep(ref MovementComponent component, ref EcsEntity unit) {
        updateOrientation(ref component);
        component.position = component.path[0];
        component.path.RemoveAt(0);
        // Debug.Log("stepped to " + component.position + " " + component.path.Count);
        if(component.path.Count == 0) { // path finished
            // Debug.Log("removing path ");
            component.path.Clear();
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
}