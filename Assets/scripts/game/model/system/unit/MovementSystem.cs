public class MovementSystem : IEcsRunSystem {
    EcsFilter<MovementComponent> filter = null;
    private AStar astar = new AStar();

    public void Run() {
        foreach(int i in filter) {
            MovementComponent component = filter.Get1(i);
            if(component.target == null) continue; // target is set by TaskSystem
            updateMovement(component)
        }
    }

    private void updateMovement(MovementComponent component) {
        if(component.path == null) {
            component.path = AStar.findPath(component);
        } else {
            component.step += component.speed;
            if(component.step > 1f) component.accumulatedStem -= 1f; 
            component.position = component.path.remove(0);
        }
    }
}