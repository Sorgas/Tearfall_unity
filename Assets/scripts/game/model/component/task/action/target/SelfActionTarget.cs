using enums.action;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;

namespace game.model.component.task.action.target {
    /**
 * Target for actions with no particular target.
 *
 * @author Alexander on 09.02.2020
 */
    public class SelfActionTarget : ActionTarget {

        public SelfActionTarget() : base(ActionTargetTypeEnum.ANY) {

        }

        public override Vector3Int? Pos => null;

        public ActionTargetStatusEnum check(EcsEntity performer) {
            return ActionTargetStatusEnum.READY;
        }
    }
}