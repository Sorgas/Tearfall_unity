using game.model.component.building;
using Leopotam.Ecs;
using types.action;
using types.building;
using UnityEngine;
using util.lang.extension;

// target for performing actions in workbenches
namespace game.model.component.task.action.target {
    class WorkbenchActionTarget : EntityActionTarget {
        private Vector3Int accessPosition;

        public WorkbenchActionTarget(EcsEntity entity) : base(entity, ActionTargetTypeEnum.EXACT) {
            accessPosition = entity.pos();
            BuildingType type = entity.take<BuildingComponent>().type; 
            accessPosition.x += type.access[0];
            accessPosition.y += type.access[1];
        }

        public override Vector3Int? Pos {
            get => accessPosition;
        }
    }
}