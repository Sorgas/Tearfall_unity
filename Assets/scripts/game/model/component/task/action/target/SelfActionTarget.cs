using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;

namespace game.model.component.task.action.target {
    /**
    * Target for actions with no particular target.
    *
    * @author Alexander on 09.02.2020
    */
    public class SelfActionTarget : ActionTarget {

        public SelfActionTarget() : base(ActionTargetTypeEnum.ANY) {

        }

        public override Vector3Int pos => Vector3Int.back;

        public override List<Vector3Int> getPositions(LocalModel model) {
            throw new System.NotImplementedException();
        }
        public override ActionTargetStatusEnum check(EcsEntity performer, LocalModel model) {
            return ActionTargetStatusEnum.READY;
        }
    }
}