using System.Collections.Generic;
using System.Linq;

namespace Assets.scripts.util.pathfinding {
    public class OpenSet {
        private SortedDictionary<Node, Node> dictionary = new SortedDictionary<Node, Node>(new NodeComparer());

        public void add(Node node) {
            dictionary[node] = node;
        }

        public Node poll() {
            if (dictionary.Count <= 0) return null;
            KeyValuePair<Node, Node> pair = dictionary.First();
            dictionary.Remove(pair.Key);
            return pair.Value;
        }

        public Node get(Node node) {
            return dictionary.ContainsKey(node) ? dictionary[node] : null;
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
