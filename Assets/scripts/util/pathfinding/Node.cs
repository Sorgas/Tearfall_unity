using System;
using UnityEngine;

namespace util.pathfinding {
    public readonly struct Node : IEquatable<Node>, IComparable<Node> {
        public readonly Vector3Int position;
        public readonly Vector3Int? parent; // points to parent
        public readonly int pathLength;
        public readonly float cost;

        public Node(Vector3Int position, Vector3Int? parent, float heuristic, int pathLength) {
            this.position = position;
            this.parent = parent;
            this.pathLength = pathLength;
            cost = pathLength + heuristic;
        }

        public Node(Vector3Int position, float heuristic) : this(position, null, heuristic, 0) {
        }

        public static bool operator >(Node node, Node node2) => node.position != node2.position && node.cost > node2.cost;

        public static bool operator <(Node node, Node node2) => node.position != node2.position && node.cost < node2.cost;

        public override string ToString() => position.ToString();

        public override bool Equals(object obj) {
            return obj is Node node && Equals(node);
        }

        public bool Equals(Node other) => other.position.Equals(position);

        public int CompareTo(Node other) {
            if (other.position.Equals(position)) return 0;
            return other.cost > cost ? -1 : 1;
        }

        public override int GetHashCode() => position.GetHashCode();
    }
}