using enums;
using enums.action;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;
using util.lang.extension;
using static game.model.component.task.action.target.ActionTargetStatusEnum;

namespace game.model.component.task.action.target {
    public abstract class ActionTarget {
        public ActionTargetTypeEnum type;

        public ActionTarget(ActionTargetTypeEnum type) {
            this.type = type;
        }

        public abstract Vector3Int? getPosition();

        /**
         * Checks if task performer has reached task target. Does not check target availability (map area).
         * Returns fail if checked from out of map.
         */
        public ActionTargetStatusEnum check(EcsEntity performer) {
            Vector3Int performerPosition = performer.pos();
            Vector3Int? target = getPosition();
            if (!target.HasValue) return READY; // target without position 
            int distance = getDistance(performerPosition, target.Value);
            if (distance > 1) return WAIT; // target not yet reached
            switch (type) {
                case ActionTargetTypeEnum.EXACT:
                    return distance == 0 ? READY : WAIT;
                case ActionTargetTypeEnum.NEAR:
                    return distance == 1 ? READY : STEP_OFF;
                case ActionTargetTypeEnum.ANY:
                    return READY; // distance is 0 or 1 here
            }
            return FAIL;
        }

        private int getDistance(Vector3Int current, Vector3Int target) {
            if (current == target) return 0;
            if (!current.isNeighbour(target)) return 2;
            if (current.z == target.z) return 1;
            if (current.z < target.z && GameModel.localMap.blockType.get(current) == BlockTypeEnum.RAMP.CODE) return 1;
            return 2;
        }
    }
}