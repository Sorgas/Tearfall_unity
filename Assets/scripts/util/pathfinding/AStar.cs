using System.Collections.Generic;
using System.Linq;
using game.model.localmap.passage;
using MoreLinq;
using UnityEngine;
using util.geometry;

namespace util.pathfinding {
// TODO add weights to tiles, add pathfinding weight visual overlay
// TODO add 'difficult terrain'
public class AStar {
    private AbstractPassageHelper helper;
    private bool debug = false;

    public AStar(AbstractPassageHelper helper) {
        this.helper = helper;
    }

    public List<Vector3Int> makeShortestPath(Vector3Int start, Vector3Int target) {
        Node initialNode = new(start, null, getH(target, start), 0);
        string message = "[AStar]: finding path " + start + " -> " + target;
        List<Vector3Int> path = search(initialNode, target);
        logResult(path, message);
        return path;
    }

    private List<Vector3Int> search(Node initialNode, Vector3Int target) {
        BinaryHeap openSet = new BinaryHeap(initialNode);
        HashSet<Vector3Int> closedSet = new();
        Dictionary<Vector3Int, Vector3Int?> fetchedNodes = new();
        while (openSet.count > 0) {
            if (!openSet.tryPop(out var currentNode)) {
                return null; // get node from open set or return not found
            }
            if (target == currentNode.position) {
                return getPath(currentNode, fetchedNodes); //check if path is complete
            }
            List<Vector3Int> vectors = getSuccessors(currentNode.position, closedSet);
            float pathLength = currentNode.pathLength + 1;
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

    // searches path from start and target points simultaneously
    public bool pathExistsBiDirectional(Vector3Int start, Vector3Int target) {
        BinaryHeap startOpenSet = new(new(start, null, getH(target, start), 0));
        BinaryHeap targetOpenSet = new(new(target, null, getH(start, target), 0));
        HashSet<Vector3Int> startClosedSet = new();
        HashSet<Vector3Int> targetClosedSet = new();
        while (startOpenSet.count > 0 && targetOpenSet.count > 0) {
            if (makeStepForBiDirectionalAstar(startOpenSet, startClosedSet, targetClosedSet, target)) return true;
            if (startOpenSet.count == 0 || targetOpenSet.count == 0) return false;
            if (makeStepForBiDirectionalAstar(targetOpenSet, targetClosedSet, startClosedSet, start)) return true;
        }
        Debug.Log($"checking path existence from {start} to {target}: not found");
        return false;
    }

    // Performs one step for bidirectional aStar pathfinding.
    // Checks successors of current position against closed set of opposite direction (if closed contains successor - path exists). 
    private bool makeStepForBiDirectionalAstar(BinaryHeap openSet, HashSet<Vector3Int> selfClosedSet, HashSet<Vector3Int> oppositeClosedSet, Vector3Int target) {
        openSet.tryPop(out Node currentNode);
        IEnumerable<Vector3Int> vectors = getSuccessors(currentNode.position, selfClosedSet);
        // Debug.Log($"successors for tile {currentNode.position}: {vectors.toString()}");
        foreach (var vector in vectors) {
            if (oppositeClosedSet.Contains(vector)) return true;
        }
        float pathLength = currentNode.pathLength + 1;
        vectors.ForEach(vector => { // iterate passable near positions
            if (!openSet.contains(vector)) { // if successor node is newly found
                openSet.push(new Node(vector, currentNode.position, getH(target, vector), pathLength)); // add to open set
            }
        });
        selfClosedSet.Add(currentNode.position); // node processed
        return false;
    }

    // Gets tiles that can be stepped into from given tile.
    private List<Vector3Int> getSuccessors(Vector3Int center, HashSet<Vector3Int> closedSet) {
        return PositionUtil.all
            .Select(vector => center + vector)
            .Where(vector => !closedSet.Contains(vector))
            .Where(vector => helper.hasPathBetweenNeighbours(center, vector)).ToList();
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