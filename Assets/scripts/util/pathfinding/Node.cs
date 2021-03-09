using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.scripts.util.geometry;

namespace Assets.scripts.util.pathfinding {
    public class Node {
        public IntVector3 position;
        public Node parent;
        public int pathLength;
        public float heuristic;

        public Node(IntVector3 position) {
            this.position = position;
        }

        public float cost() {
            return pathLength + heuristic;
        }

        public List<IntVector3> getPath() {
            List<IntVector3> path = new List<IntVector3>();
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
                   EqualityComparer<IntVector3>.Default.Equals(position, node.position);
        }

        public override int GetHashCode() {
            return 1206833562 + EqualityComparer<IntVector3>.Default.GetHashCode(position);
        }
    }
}
