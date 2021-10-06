using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.util.pathfinding {
    public class Node : IEquatable<Node>, IComparable<Node> {
        public readonly Vector3Int position;
        public readonly Node parent;
        public readonly int pathLength;
        public readonly float heuristic;
        public readonly float cost;

        public Node(Vector3Int position, Node parent, float heuristic) {
            this.position = position;
            this.parent = parent;
            this.pathLength = parent != null ? parent.pathLength + 1 : 0;
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
            return obj != null && obj is Node node && this.Equals(node);
        }

        public bool Equals(Node other) {
            return other != null && other.position.Equals(position);
        }

        public int CompareTo(Node other) {
            if (other.position.Equals(position)) return 0;
            return other.cost < cost ? -1 : 1;
        }

        public override int GetHashCode() {
            return position.GetHashCode();
        }
    }
}
