using game.model.component.building;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
public abstract class EntityActionTarget : ActionTarget {
    public EcsEntity entity;

    public EntityActionTarget(EcsEntity entity, ActionTargetTypeEnum placement) : base(placement) {
        this.entity = entity;
    }

    public override Vector3Int pos {
        get {
            if (entity.Has<BuildingComponent>()) {
                BuildingComponent building = entity.take<BuildingComponent>();
                if (building.type.access != null) {
                    return entity.take<BuildingComponent>().getAccessPosition(entity.pos());
                }
            }
            return entity.pos();
        }
    }
}
}