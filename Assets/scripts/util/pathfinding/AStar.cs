using System.Collections.Generic;
using System.Linq;
using Assets.scripts.enums.action;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.game.model.system;
using Assets.scripts.util.geometry;
using Assets.scripts.util.lang;
using UnityEngine;

namespace Assets.scripts.util.pathfinding {
    public class AStar : Singleton<AStar> {
        private LocalMap localMap;

        /**
         * Returns the shortest Path from a start node to an end node according to
         * the A* heuristics (h must not overestimate). initialNode and last found node included.
         */

        public List<Vector3Int> makeShortestPath(Vector3Int initialPos, Vector3Int targetPos, ActionTargetTypeEnum targetType) {
            localMap = GameModel.get().localMap;
            Node initialNode = new Node(initialPos);
            initialNode.pathLength = 0;
            initialNode.heuristic = (targetPos - initialPos).magnitude;

            return search(initialNode, targetPos, targetType)?.getPath();
        }

        public List<Vector3Int> makeShortestPath(Vector3Int initialPos, Vector3Int targetPos) {
            return makeShortestPath(initialPos, targetPos, ActionTargetTypeEnum.EXACT);
        }

        /**
         * @param initialNode start of the search
         * @param targetPos   end of the search
         * @param targetType  see {@link ActionTarget}
         * @return goal node to restore path from
         */
        private Node search(Node initialNode, Vector3Int targetPos, ActionTargetTypeEnum targetType) {
            OpenSet openSet = new OpenSet();
            HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
            PathFinishCondition finishCondition = new PathFinishCondition(targetPos, targetType);

            openSet.add(initialNode);
            while (openSet.size() > 0) {
                Node currentNode = openSet.poll(); //get element with the least sum of costs
                if (finishCondition.check(currentNode.position)) return currentNode; //check if path is complete
                List<Node> nodes = getSuccessors(currentNode); // TODO rewrite to positions
                nodes = nodes.Where(node => !closedSet.Contains(node.position)).ToList();
                int pathLength = currentNode.pathLength + 1;
                nodes.ForEach(node => {
                    Node oldNode = openSet.get(node);
                    node.pathLength = pathLength;
                    if ((oldNode == null) || (oldNode.pathLength > node.pathLength)) { // if successor node is newly found, or has shorter path
                        node.parent = currentNode;
                        node.heuristic = (targetPos - node.position).magnitude;
                        openSet.add(node); // replace old node
                    }
                });
                closedSet.Add(currentNode.position); // node processed
            }
            //        Logger.PATH.logDebug("No path found");
            return null;
        }

        /**
         * Gets tiles that can be stepped in from given tile.
         */
        private List<Node> getSuccessors(Node node) {
            return PositionUtil.all
                .Select(vector => node.position + vector)
                .Where(vector => localMap.inMap(vector))
                .Where(vector => localMap.passageMap.hasPathBetweenNeighbours(node.position, vector))
                .Select(vector => new Node(vector)).ToList();
            //.map(delta->Position.add(node.position, delta))
            //.filter(localMap::inMap)
            //.filter(pos->localMap.passageMap.hasPathBetweenNeighbours(node.position, pos))
            //.map(Node::new);
        }
    }
}
