using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.enums.action;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.game.model.system;
using Assets.scripts.util.geometry;

namespace Assets.scripts.util.pathfinding {
    public class AStar : ModelComponent {
        private LocalMap localMap;

        /**
         * Returns the shortest Path from a start node to an end node according to
         * the A* heuristics (h must not overestimate). initialNode and last found node included.
         */
        public List<IntVector3> makeShortestPath(IntVector3 initialPos, IntVector3 targetPos, ActionTargetTypeEnum targetType) {
            localMap = GameModel.get<LocalMap>();
            Node initialNode = new Node(initialPos);
            initialNode.pathLength = 0;
            initialNode.heuristic = initialPos.getDistance(targetPos);

            return search(initialNode, targetPos, targetType)?.getPath();
        }

        public List<IntVector3> makeShortestPath(IntVector3 initialPos, IntVector3 targetPos) {
            return makeShortestPath(initialPos, targetPos, ActionTargetTypeEnum.EXACT);
        }

        /**
         * @param initialNode start of the search
         * @param targetPos   end of the search
         * @param targetType  see {@link ActionTarget}
         * @return goal node to restore path from
         */
        private Node search(Node initialNode, IntVector3 targetPos, ActionTargetTypeEnum targetType) {
            OpenSet openSet = new OpenSet();
            HashSet<IntVector3> closedSet = new HashSet<IntVector3>();
            PathFinishCondition finishCondition = new PathFinishCondition(targetPos, targetType);

            openSet.add(initialNode);
            while (openSet.size() > 0) {
                Node currentNode = openSet.poll(); //get element with the least sum of costs
                if (finishCondition.check(currentNode.position)) return currentNode; //check if path is complete
                int pathLength = currentNode.pathLength + 1;
                List<Node> nodes = getSuccessors(currentNode);
                nodes = nodes.Where(node => !closedSet.Contains(node.position))
                    .ToList();
                nodes.ForEach(newNode => {
                    newNode.pathLength = pathLength;
                    Node oldNode = openSet.get(newNode);
                    if ((oldNode == null) || (oldNode.pathLength > newNode.pathLength)) { // if successor node is newly found, or has shorter path
                        newNode.parent = currentNode;
                        newNode.heuristic = newNode.position.getDistance(targetPos);
                        openSet.add(newNode); // replace old node
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
                .Select(vector => IntVector3.add(node.position, vector))
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
