using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace util.pathfinding {
    public class BinaryHeap {
        private readonly Dictionary<Vector3Int, int> map = new();
        private readonly List<Node> list = new();
        public int Count => list.Count;

        public void push(Node item) {
            list.Add(item);
            var i = list.Count - 1;
            map[item.position] = i;
            while (i > 0) {
                var j = (i - 1) / 2;
                if (list[i] > list[j]) break;
                swap(i, j);
                i = j;
            }
        }

        private Node? pop() {
            if (list.Count == 0) return null;
            var result = list.First();
            removeRoot();
            map.Remove(result.position);
            return result;
        }

        public bool tryPop(out Node value) {
            var q = pop();
            value = q ?? default;
            return q.HasValue;
        }

        public void clear() {
            list.Clear();
            map.Clear();
        }

        public bool tryGet(Vector3Int key, out Node? value) {
            if (!map.TryGetValue(key, out var index)) {
                value = null;
                return false;
            }
            value = list[index];
            return true;
        }

        public void modify(Node node) {
            if (!map.TryGetValue(node.position, out var index))
                throw new KeyNotFoundException(nameof(node));
            list[index] = node;
        }

        private void removeRoot() {
            list[0] = list.Last();
            map[list[0].position] = 0;
            list.RemoveAt(list.Count - 1);

            var i = 0;
            while (true) {
                var largest = largestIndex(i);
                if (largest == i) return;
                swap(i, largest);
                i = largest;
            }
        }

        private void swap(int i, int j) {
            (list[i], list[j]) = (list[j], list[i]);
            map[list[i].position] = i;
            map[list[j].position] = j;
        }

        private int largestIndex(int i) {
            var leftInd = 2 * i + 1;
            var rightInd = 2 * i + 2;
            var largest = i;
            if (leftInd < list.Count && list[leftInd] < list[largest]) largest = leftInd;
            if (rightInd < list.Count && list[rightInd] < list[largest]) largest = rightInd;
            return largest;
        }
    }
}