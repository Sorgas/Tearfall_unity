using game.model.component;
using game.model.component.building;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.system.building {
// Rolls closing timout on doors

// Doors are opened when unit passes through door.
// After that, door waits for timeout, checks if no creatures in tile are present, and closes itself with default speed
public class DoorClosingSystem : LocalModelScalableEcsSystem {
    public EcsFilter<BuildingDoorComponent> filter;

    protected override void runLogic(int ticks) {
        foreach (var i in filter) {
            EcsEntity door = filter.GetEntity(i);
            ref BuildingDoorComponent doorComponent = ref door.takeRef<BuildingDoorComponent>();
            if (doorComponent.timeout > 0) {
                doorComponent.timeout -= 1;
            } else if (doorComponent.openness > 0) {
                doorComponent.openness -= doorComponent.closingSpeed;
                door.Replace(new VisualUpdatedComponent());
                if (doorComponent.openness < 1) {
                    door.Del<BuildingDoorOpenComponent>();
                }
            }
        }
    }
}
}