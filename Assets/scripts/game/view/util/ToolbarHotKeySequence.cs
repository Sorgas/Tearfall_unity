using UnityEngine;
using static UnityEngine.KeyCode;

namespace game.view.util {
// hotkey sequence for toolbar panels
public class ToolbarHotKeySequence {
    private KeyCode[] keys = { Z, X, C, V, B, N, M };
    private int current;

    public KeyCode getNext() {
        KeyCode result = keys[current++];
        if (current >= keys.Length) current = 0;
        return result;
    }
}
}