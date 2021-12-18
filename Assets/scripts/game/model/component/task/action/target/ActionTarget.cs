using enums;
using enums.action;
using game.model.component.unit.components;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;

namespace game.model.component.task.action.target {
    public abstract class ActionTarget {
        public ActionTargetTypeEnum type;

        public ActionTarget(ActionTargetTypeEnum type) {
            this.type = type;
        }

        public abstract Vector3Int getPosition();

        /**
     * Checks if task performer has reached task target. Does not check target availability (map area).
     * Returns fail if checked from out of map.
     */
        public ActionTargetStatusEnum check(EcsEntity performer) {
            Vector3Int performerPosition = performer.Get<MovementComponent>().position;
            Vector3Int targetPosition = getPosition();
            int distance = getDistance(performerPosition);
            if (distance > 1) return ActionTargetStatusEnum.WAIT; // target not yet reached
            switch (type) {
                case ActionTargetTypeEnum.EXACT:
                    return distance == 0 ? ActionTargetStatusEnum.READY : ActionTargetStatusEnum.WAIT;
                case ActionTargetTypeEnum.NEAR:
                    return distance == 1 ? ActionTargetStatusEnum.READY : ActionTargetStatusEnum.STEP_OFF;
                case ActionTargetTypeEnum.ANY:
                    return ActionTargetStatusEnum.READY; // distance is 0 or 1 here
            }
            return ActionTargetStatusEnum.FAIL;
        }

        private int getDistance(Vector3Int current) {
            Vector3Int target = getPosition();
            if (current == target) return 0;
            if (!current.isNeighbour(target)) return 2;
            if (current.z == target.z) return 1;
            if (current.z < target.z && GameModel.localMap.blockType.get(current) == BlockTypeEnum.RAMP.CODE) return 1;
            return 2;
        }
    }
}
