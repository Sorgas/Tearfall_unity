using System.Collections.Generic;
using System.Linq;
using enums.action;
using game.model.localmap;
using UnityEngine;
using util.geometry;
using util.lang;

namespace util.pathfinding {
    public class AStar : Singleton<AStar> {
        private LocalMap localMap;

        public List<Vector3Int> makeShortestPath(Vector3Int start, Vector3Int target, ActionTargetTypeEnum targetType, LocalMap map) {
            localMap = map;
            Debug.Log("searching path from " + start + " to " + target);
            return search(new Node(start, null, getH(target, start), 0), target, targetType);
        }

        public List<Vector3Int> makeShortestPath(Vector3Int start, Vector3Int target, LocalMap map) =>
            makeShortestPath(start, target, ActionTargetTypeEnum.EXACT, map);

        /**
         * @param targetType  see {@link ActionTarget}
         * @return goal node to restore path from
         */
        private List<Vector3Int> search(Node initialNode, Vector3Int target, ActionTargetTypeEnum targetType) {
            var openSet = new BinaryHeap();
            var closedSet = new HashSet<Vector3Int>();
            var fetchedNodes = new Dictionary<Vector3Int, Vector3Int?>();
            var finishCondition = new PathFinishCondition(target, targetType, localMap);

            openSet.push(initialNode);
            while (openSet.Count > 0) {
                if (!openSet.tryPop(out var currentNode)) return null; // get node from open set or return not found
                if (finishCondition.check(currentNode.position))
                    return getPath(currentNode, fetchedNodes); //check if path is complete
                var vectors = getSuccessors(currentNode.position, closedSet);
                var pathLength = currentNode.pathLength + 1;
                vectors.ForEach(vector => { // iterate passable near positions
                    openSet.tryGet(vector, out var oldNode);
                    if (oldNode == null || oldNode.Value.pathLength > pathLength) // if successor node is newly found, or has shorter path
                        openSet.push(new Node(vector, currentNode.position, getH(target, vector), pathLength)); // replace old node
                });
                closedSet.Add(currentNode.position); // node processed
                fetchedNodes[currentNode.position] = currentNode.parent;
            }
            Debug.Log("No path found");
            return null;
        }

        // counts heuristic for current vector
        private static float getH(Vector3Int target, Vector3Int current) => (target - current).magnitude;

        // Gets tiles that can be stepped in from given tile.
        private List<Vector3Int> getSuccessors(Vector3Int center, HashSet<Vector3Int> closedSet) {
            return PositionUtil.all
                .Select(vector => center + vector)
                .Where(vector => localMap.inMap(vector) && localMap.passageMap.hasPathBetweenNeighbours(center, vector))
                .Where(vector => !closedSet.Contains(vector)).ToList();
        }

        private List<Vector3Int> getPath(Node node, Dictionary<Vector3Int, Vector3Int?> nodes) {
            var path = new List<Vector3Int> { node.position };
            Vector3Int? current = node.parent;
            while (current.HasValue) {
                path.Insert(0, current.Value);
                current = nodes[current.Value];
            }
            return path;
        }
    }
}