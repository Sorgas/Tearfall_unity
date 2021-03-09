using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.util.pathfinding {
    public class OpenSet {
        private SortedDictionary<Node, Node> dictionary = new SortedDictionary<Node, Node>(new NodeComparer());

        public void add(Node node) {
            dictionary.Add(node, node);
        }

        public Node poll() {
            KeyValuePair<Node, Node> pair = dictionary.First();
            dictionary.Remove(pair.Key);
            return dictionary.First().Value;
        }

        public Node get(Node node) {
            return dictionary[node];
        }

        public int size() {
            return dictionary.Count;
        }
    }

    class NodeComparer : IComparer<Node> {
        public int Compare(Node node1, Node node2) {
            if (node1.position == node2.position) return 0;
            return node1.cost() < node2.cost() ? -1 : 1;
        }
    }
}
