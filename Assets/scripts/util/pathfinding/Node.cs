using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.util.pathfinding {
    public class Node {
        public Vector3Int position;
        public Node parent;
        public int pathLength;
        public float heuristic;

        public Node(Vector3Int position) {
            this.position = position;
        }

        public float cost() {
            return pathLength + heuristic;
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

        public override bool Equals(object obj) {
            return obj is Node node &&
                   EqualityComparer<Vector3Int>.Default.Equals(position, node.position);
        }

        public override int GetHashCode() {
            return 1206833562 + EqualityComparer<Vector3Int>.Default.GetHashCode(position);
        }
    }
}
