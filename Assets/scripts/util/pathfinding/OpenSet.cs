using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.scripts.util.pathfinding {
    public class OpenSet {
        public SortedDictionary<Node, Node> dictionary = new SortedDictionary<Node, Node>(new NodeComparer());

        public void add(Node node) {
            dictionary[node] = node;
        }

        public Node poll() {
            if (dictionary.Count <= 0) return null;
            Node node = dictionary.Keys.First();
            Debug.Log("node " + node + " polled, " + dictionary.Count);
            if (!dictionary.ContainsKey(node)) {
                // string log = "";
                Node item2 = null;
                foreach (Node item in dictionary.Keys) {
                    if(item.Equals(node)) {
                        item2 = item;
                        Debug.Log(item2.GetHashCode() + " " + node.GetHashCode());
                        Debug.Log("equal!");
                        break;
                    }
                    // log += item.position.ToString() + " ";
                }
                if(item2 != null) {
                    bool removeResult = dictionary.Remove(item2);
                    Debug.Log("node2 removed, " + removeResult + dictionary.Count);
                }
                // Debug.Log(log);
            } else {
                bool removeResult = dictionary.Remove(node);
                Debug.Log("node removed, " + removeResult + dictionary.Count);
            }
            return node;
        }

        public Node get(Node node) {
            if (dictionary.ContainsKey(node)) return dictionary[node];
            return null;
        }

        public int size() {
            return dictionary.Count;
        }
    }

    class NodeComparer : IComparer<Node> {
        public int Compare(Node node1, Node node2) {
            if (node1.position == node2.position) return 0;
            return node1.cost < node2.cost ? -1 : 1;
        }
    }
}