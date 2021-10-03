using System.Collections.Generic;
using System.Linq;
using Assets.scripts.enums.action;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.geometry;
using Assets.scripts.util.lang;
using UnityEngine;

namespace Assets.scripts.util.pathfinding {
    public class AStar : Singleton<AStar> {
        private LocalMap localMap;

        public List<Vector3Int> makeShortestPath(Vector3Int start, Vector3Int target, ActionTargetTypeEnum targetType) {
            localMap = GameModel.get().localMap;
            Debug.Log("searching path from " + start + " to " + target);
            Debug.Log("searching path from " + localMap.passageMap.area.get(start) + " to " + localMap.passageMap.area.get(start)); 
            Node initialNode = new Node(start, null, getHeuristic(target, start));
            return search(initialNode, target, targetType)?.getPath();
        }

        public List<Vector3Int> makeShortestPath(Vector3Int initialPos, Vector3Int targetPos) => makeShortestPath(initialPos, targetPos, ActionTargetTypeEnum.EXACT);

        /**
         * @param targetType  see {@link ActionTarget}
         * @return goal node to restore path from
         */
        private Node search(Node initialNode, Vector3Int target, ActionTargetTypeEnum targetType) {
            OpenSet openSet = new OpenSet();
            HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
            PathFinishCondition finishCondition = new PathFinishCondition(target, targetType);

            openSet.add(initialNode);
            int count = 0;
            while (openSet.size() > 0 && count < 1000) {
                count++;
                Debug.Log(openSet.size());
                Node currentNode = openSet.poll(); //get element with the least sum of costs
                Debug.Log(openSet.dictionary.Keys.Contains(currentNode));
                Debug.Log(currentNode.position + " " + openSet.size());
                // openSet.logSize();
                if (finishCondition.check(currentNode.position)) return currentNode; //check if path is complete

                List<Vector3Int> vectors = getSuccessors(currentNode.position, closedSet); // TODO rewrite to positions
                Debug.Log(vectors.Count);
                int pathLength = currentNode.pathLength + 1;
                vectors.ForEach(vector => {
                    Debug.Log(vector);
                    Node node = new Node(vector, currentNode, getHeuristic(target, vector));
                    Node oldNode = openSet.get(node);
                    // if(oldNode != null) {Debug.Log("old node exists");}
                    if ((oldNode == null) || (oldNode.pathLength > pathLength)) { // if successor node is newly found, or has shorter path
                        Debug.Log("adding " + node.position);
                        openSet.add(node); // replace old node
                    }
                });
                Debug.Log("closing " + currentNode.position);
                closedSet.Add(currentNode.position); // node processed
            }
            Debug.Log("No path found");
            return null;
        }

        private float getHeuristic(Vector3Int target, Vector3Int current) {
            return (target - current).magnitude;
        }

        // Gets tiles that can be stepped in from given tile.
        private List<Vector3Int> getSuccessors(Vector3Int center, HashSet<Vector3Int> closedSet) {
            return PositionUtil.all
                .Select(vector => center + vector)
                .Where(vector => localMap.inMap(vector) && localMap.passageMap.hasPathBetweenNeighbours(center, vector))
                .Where(vector => !closedSet.Contains(vector)).ToList();
        }
    }
}
