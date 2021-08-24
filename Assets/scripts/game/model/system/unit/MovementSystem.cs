using Assets.scripts.util.pathfinding;
using Leopotam.Ecs;

public class MovementSystem : IEcsRunSystem {
    EcsFilter<MovementComponent> filter = null;

    public void Run() {
        foreach(int i in filter) {
            MovementComponent component = filter.Get1(i);
            if(component.target == null) continue; // target is set by TaskSystem
            updateMovement(component);
        }
    }

    private void updateMovement(MovementComponent component) {
        if(component.path == null) {
            component.path = AStar.get().makeShortestPath(component.position, component.target, component.targetType);
        } else {
            component.step += component.speed;
            if(component.step > 1f) component.step -= 1f; 
            component.position = component.path[0];
            component.path.RemoveAt(0);
        }
    }
}