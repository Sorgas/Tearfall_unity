using System;
using game.model.component;
using game.model.component.building;
using game.view.tilemaps;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.view.system.building {
public class DoorVisualSystem : IEcsRunSystem {
    public EcsFilter<BuildingDoorComponent, VisualUpdatedComponent> filter;

    public void Run() {
        foreach (var i in filter) {
            EcsEntity entity = filter.GetEntity(i);
            updateSprite(entity, filter.Get1(i));
            entity.Del<VisualUpdatedComponent>();
        }
    }

    private void updateSprite(EcsEntity entity, BuildingDoorComponent door) {
        BuildingComponent building = entity.take<BuildingComponent>();
        float modifier = building.type.tileCount - 2 + 0.1f;
        int spriteIndex = (int)Math.Ceiling(door.openness * modifier);
        Debug.Log(spriteIndex);
        entity.take<BuildingVisualComponent>().gameObject.GetComponent<SpriteRenderer>().sprite
            = BuildingTilesetHolder.get().get(building.type, building.orientation, spriteIndex);
    }
}
}