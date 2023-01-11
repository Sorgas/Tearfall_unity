using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action.target;
using game.model.localmap;
using game.model.localmap.passage;
using types.action;
using UnityEngine;
using util;
using util.geometry.bounds;

namespace game.model.component.task.action {
    // action for removing performer from area
    public class StepOffAction : Action {
        public StepOffAction(Vector3Int from, LocalModel model) : base(new PositionActionTarget(from, ActionTargetTypeEnum.EXACT)) {
            Vector3Int targetPos = findPositionForSingleTile(from, model);
            handleFoundPosition(targetPos);
        }
        
        public StepOffAction(IntBounds3 bounds, Vector3Int performerPosition, LocalModel model) : base(null) {
            Vector3Int targetPos = findPositionForBounds(bounds, performerPosition);
            handleFoundPosition(targetPos);
        }

        private void handleFoundPosition(Vector3Int targetPos) {
            if (targetPos.z != -1) {
                startCondition = () => ActionConditionStatusEnum.OK;
                target = new PositionActionTarget(targetPos, ActionTargetTypeEnum.EXACT);
            } else {
                Debug.LogError("stepOff position not found"); // TODO handle
            }
        }

        private Vector3Int findPositionForSingleTile(Vector3Int from, LocalModel model) {
            List<Vector3Int> positions = new NeighbourPositionStream(from, model).filterConnectedToCenter().stream.ToList();
            if (positions.Count > 0) {
                Debug.Log("[StepOffAction]: prosition from " + from.ToString() + " found " + positions[0]);
                return positions[0];
            }
            Debug.Log("[StepOffAction]: prosition from " + from.ToString() + " not found");
            return new Vector3Int(0, 0, -1); // unreachable position, 
        }
        
        // tries to find free position outside of bounds where performer can move. priorities: near performer, on same z-level, above and below bounds
        private Vector3Int findPositionForBounds(IntBounds3 bounds, Vector3Int performerPosition) {
            LocalMap map = model.localMap;
            UtilByteArray areas = map.passageMap.area;
            PassageMap passageMap = map.passageMap;
            int performerArea = areas.get(performerPosition);

            // try around performer
            List<Vector3Int> result = new NeighbourPositionStream(performerPosition, model)
                .filterConnectedToCenter().stream
                .Where(position => !bounds.isIn(position)).ToList();
            if (result.Count != 0) {
                return result[0];
            }

            Vector3Int? res = null;
            // position to step into should be in map, reachable for performer, and connected to adjacent tile inside building
            bool checkPositionReachability(int x1, int y1, int z1, int x2, int y2, int z2) {
                if (map.inMap(x1, y1, z1) && areas.get(x1, y1, z1) == performerArea
                    && passageMap.hasPathBetweenNeighbours(x1, y1, z1, x2, y2, z2)) {
                    res = new Vector3Int(x1, y1, z1);
                    return true;
                }
                return false;
            }

            // try on same z levels
            for (int z = bounds.minZ; z <= bounds.maxZ; z++) {
                for (int x = bounds.minX; x <= bounds.maxX; x++) {
                    if (checkPositionReachability(x, bounds.minY - 1, z, x, bounds.minY, z)) return res.Value;
                    if (checkPositionReachability(x, bounds.maxY + 1, z, x, bounds.minY, z)) return res.Value;
                }
                for (int y = bounds.minY; y <= bounds.maxY + 1; y++) {
                    if (checkPositionReachability(bounds.minX - 1, y, z, bounds.minX, y, z)) return res.Value;
                    if (checkPositionReachability(bounds.maxX + 1, y, z, bounds.minX, y, z)) return res.Value;
                }
            }
            // try with z levels
            for (int x = bounds.minX - 1; x <= bounds.maxX + 1; x++) {
                for (int y = bounds.minY - 1; y <= bounds.maxY + 1; y++) {
                    if (checkPositionReachability(x, y, bounds.minZ - 1, x, y, bounds.minZ)) return res.Value;
                    if (checkPositionReachability(x, y, bounds.maxZ + 1, x, y, bounds.maxZ)) return res.Value;
                }
            }
            return new Vector3Int(0, 0, -1); // unreachable position, 
        }
    }
}