using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.scripts.util.pathfinding {
    public class OpenSet {
        public SortedDictionary<Node, Node> dictionary = new SortedDictionary<Node, Node>(new NodeComparer());
        // SortedSet<Node> set = new SortedSet<Node>(new NodeComparer());
        // private Dictionary<Vector3Int, Node> vectorDictionary = new Dictionary<Vector3Int, Node>();

        public void add(Node node) {
            // set.Add(node);
            dictionary[node] = node;
            // vectorDictionary[node.position] = node;
            // log("add", node.position);
        }

        public Node poll() {
            if (dictionary.Count <= 0) return null;
            // KeyValuePair<Node, Node> pair = dictionary.First();
            Node node = dictionary.Keys.First();
            // vectorDictionary.Remove(pair.Key.position);
            dictionary.Remove(node);
            // log("poll", pair.Key.position);
            return node;
        }

        // public Node get(Vector3Int vector) {
        //     log("get", vector);
        //     if (vectorDictionary.ContainsKey(vector)) {
        //         if (dictionary.ContainsKey(vectorDictionary[vector])) {
        //             return dictionary[vectorDictionary[vector]];
        //         }
        //         Debug.Log("inconsintensy!");
        //     }
        //     return null;
        // }

        public Node get(Node node) {
            // set.Contains(node);
            // set[node];
            if(dictionary.ContainsKey(node) ) return dictionary[node];
            return null;
        }

        public int size() {
            return dictionary.Count;
        }

        public void log(string action, Vector3Int vector) {
            // Debug.Log(action + " " + vector + " nodes: " + dictionary.Count 
            // + " vectors: " + vectorDictionary.Count
            // );
            // foreach (Vector3Int qwer in vectorDictionary.Keys) {
            //     if (!dictionary.ContainsKey(vectorDictionary[qwer])) {
            //         Debug.Log("inconsistent: " + qwer + " to " + vectorDictionary[qwer].position);
            //     }
            // }
            // foreach (Node qwer in dictionary.Keys) {
            //     if (!vectorDictionary.ContainsKey(qwer.position)) {
            //         Debug.Log("orphan: " + qwer);
            //     }
            // }
        }
    }

    class NodeComparer : IComparer<Node> {
        public int Compare(Node node1, Node node2) {
            if (node1.position == node2.position) return 0;
            return node1.cost < node2.cost ? -1 : 1;
        }
    }
}
