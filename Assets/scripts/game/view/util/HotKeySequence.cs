using UnityEngine;
using static UnityEngine.KeyCode;

namespace game.view.util {
    public class HotKeySequence {
        private KeyCode[] keys = { Z, X, C, V, B, N, M };
        private int current = 0;

        public KeyCode getNext() {
            KeyCode result = keys[current++];
            if (current >= keys.Length) current = 0;
            return result;
        }

        public void reset() {
            current = 0;
        }
    }
}