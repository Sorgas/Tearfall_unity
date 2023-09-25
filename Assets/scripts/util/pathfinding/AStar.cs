using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action.target;
using game.model.localmap;
using types.action;
using UnityEngine;
using util.geometry;
using util.lang;

namespace util.pathfinding {
    // TODO make not singleton
    // TODO add weights to tiles, add pathfinding weight visual overlay
    // TODO add 'difficult terrain',
    // TODO add doors(passable, counted with special weight), if door is locked by player, it becomes impassable
    public class AStar : Singleton<AStar> {
        private bool debug = true;
        private LocalMap localMap;
        
        public List<Vector3Int> makeShortestPath(Vector3Int start, Vector3Int target, LocalModel model) {
            localMap = model.localMap;
            Node initialNode = new(start, null, getH(target, start), 0);
            string message = "[AStar]: finding path " + start + " -> " + target;
            List<Vector3Int> path = search(initialNode, target);
            logResult(path, message);
            return path;
        }

        public bool pathExists(Vector3Int start, Vector3Int target, LocalMap map) {
            localMap = map;
            Node initialNode = new(start, null, getH(target, start), 0);
            return pathExists(initialNode, target);
        }
        
        private List<Vector3Int> search(Node initialNode, Vector3Int target) {
            var openSet = new BinaryHeap();
            var closedSet = new HashSet<Vector3Int>();
            var fetchedNodes = new Dictionary<Vector3Int, Vector3Int?>();

            openSet.push(initialNode);
            while (openSet.Count > 0) {
                if (!openSet.tryPop(out var currentNode)) {
                    return null; // get node from open set or return not found
                }
                if (target == currentNode.position) {
                    return getPath(currentNode, fetchedNodes); //check if path is complete
                }
                List<Vector3Int> vectors = getSuccessors(currentNode.position, closedSet);
                int pathLength = currentNode.pathLength + 1;
                vectors.ForEach(vector => { // iterate passable near positions
                    openSet.tryGet(vector, out var oldNode);
                    if (oldNode == null || oldNode.Value.pathLength > pathLength) // if successor node is newly found, or has shorter path
                        openSet.push(new Node(vector, currentNode.position, getH(target, vector), pathLength)); // replace old node
                });
                closedSet.Add(currentNode.position); // node processed
                fetchedNodes[currentNode.position] = currentNode.parent;
            }
            return null;
        }

        // counts heuristic for current vector
        private static float getH(Vector3Int target, Vector3Int current) => (target - current).magnitude;

        // Gets tiles that can be stepped in from given tile.
        private List<Vector3Int> getSuccessors(Vector3Int center, HashSet<Vector3Int> closedSet) {
            return PositionUtil.all
                .Select(vector => center + vector)
                .Where(vector => !closedSet.Contains(vector))
                .Where(vector => localMap.inMap(vector) && localMap.passageMap.hasPathBetweenNeighbours(center, vector))
                .ToList();
        }

        private List<Vector3Int> getPath(Node node, Dictionary<Vector3Int, Vector3Int?> nodes) {
            var path = new List<Vector3Int> { node.position };
            Vector3Int? current = node.parent;
            while (current.HasValue) {
                path.Insert(0, current.Value);
                current = nodes[current.Value];
            }
            // path.RemoveAt(0);
            return path;
        }

        private bool pathExists(Node initialNode, Vector3Int target) {
            BinaryHeap openSet = new();
            HashSet<Vector3Int> closedSet = new();

            openSet.push(initialNode);
            while (openSet.Count > 0) {
                if (!openSet.tryPop(out Node currentNode)) return false; // no more tiles to search
                if (target == currentNode.position) return true; // path found
                List<Vector3Int> vectors = getSuccessors(currentNode.position, closedSet);
                int pathLength = currentNode.pathLength + 1;
                vectors.ForEach(vector => { // iterate passable near positions
                    openSet.tryGet(vector, out Node? oldNode);
                    if (oldNode == null) // if successor node is newly found
                        openSet.push(new Node(vector, currentNode.position, getH(target, vector), pathLength)); // add to open set
                });
                closedSet.Add(currentNode.position); // node processed
            }
            return false;
        }

        private void logResult(List<Vector3Int> path, string message) {
            if (!debug) return;
            if (path == null) {
                Debug.LogWarning(message + ". No path");
            } else {
                string points = path.Select(pos => pos.ToString())
                    .Aggregate((s1, s2) => $"{s1} {s2}");
                Debug.Log(points);
                Debug.Log(message + ". Length " + (path.Count - 1));
            }
        }
    }
}