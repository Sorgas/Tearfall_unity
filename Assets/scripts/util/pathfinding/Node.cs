using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.util.pathfinding {
    public class Node {
        public readonly Vector3Int position;
        public readonly Node parent;
        public readonly int pathLength;
        public readonly float heuristic;
        public readonly float cost;

        public Node(Vector3Int position, Node parent, int pathLength, float heuristic) {
            this.position = position;
            this.parent = parent;
            this.pathLength = pathLength;
            this.heuristic = heuristic;
            this.cost = pathLength + heuristic;
        }

        public List<Vector3Int> getPath() {
            List<Vector3Int> path = new List<Vector3Int>();
            path.Add(position);
            Node current = this;
            while (current != null) {
                path.Insert(0, current.position);
                current = current.parent;
            }
            return path;
        }

        public override string ToString() {
            return position.ToString();
        }

        public override bool Equals(object obj) {
            return obj is Node node &&
                   EqualityComparer<Vector3Int>.Default.Equals(position, node.position);
        }

        public override int GetHashCode() {
            return 1206833562 + EqualityComparer<Vector3Int>.Default.GetHashCode(position);
        }
    }
}
