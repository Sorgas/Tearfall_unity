using enums.action;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.component.task.action.target {
    public class EntityActionTarget : ActionTarget {
        public EcsEntity entity;

        public EntityActionTarget(EcsEntity entity, ActionTargetTypeEnum placement) : base(placement) {
            this.entity = entity;
        }
        
        public override Vector3Int? getPosition() {
            if (!entity.Has<PositionComponent>()) {
                Debug.LogWarning("Getting position of entity " + entity + " without PositionComponent");
                return null;
            }
            return entity.Get<PositionComponent>().position;
        }
    }
}