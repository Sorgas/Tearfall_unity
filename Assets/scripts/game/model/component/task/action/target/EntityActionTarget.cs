using enums.action;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.target {
    public class EntityActionTarget : ActionTarget {
        public EcsEntity entity;

        public EntityActionTarget(EcsEntity entity, ActionTargetTypeEnum placement) : base(placement) {
            this.entity = entity;
        }

        public override Vector3Int? Pos => entity.pos();
    }
}