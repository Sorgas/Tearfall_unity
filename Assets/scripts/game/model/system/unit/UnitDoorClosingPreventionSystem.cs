using game.model.component.building;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.unit {
// resets closing timeouts for doors when units are on same tile
public class UnitDoorClosingPreventionSystem : LocalModelScalableEcsSystem {
    public EcsFilter<UnitComponent> filter;
    
    protected override void runLogic(int ticks) {
        foreach (var i in filter) {
            EcsEntity unit = filter.GetEntity(i);
            EcsEntity currentDoor = model.buildingContainer.getBuilding(unit.pos());
            if (currentDoor != EcsEntity.Null || currentDoor.Has<BuildingDoorComponent>()) {
                if (!currentDoor.Has<BuildingDoorOpenComponent>()) {
                    Debug.LogWarning($"Unit on closed door detected {unit.pos()}");
                }
                ref BuildingDoorComponent component = ref currentDoor.takeRef<BuildingDoorComponent>();
                component.timeout = 3; // reset closing timout
            }
        }
    }
}
}