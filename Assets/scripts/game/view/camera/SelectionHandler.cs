using game.view.system.mouse_tool;
using UnityEngine;

namespace game.view.camera {
    // handles input events of mouse. stores state and logic of selecting tiles with frames.
    // passes completed selections to MouseToolManager
    // TODO cancel selection when type changed
    // TODO add visual validation for designations
    public class SelectionHandler {
        public SelectionState state = new();
        public bool enabled = true;

        public void init() {
            state.updater = GameView.get().tileUpdater;
        }

        public void handleMouseDown(Vector3Int position) {
            if (enabled && !state.started) state.startSelection(position);
        }

        // finishes selection
        public void handleMouseUp() {
            if (state.started) {
                MouseToolManager.handleSelection(state.bounds);
                state.reset();
            }
        }

        // TODO also cancel tools, close menus (ES2)
        public void handleSecondaryMouseClick() {
            if (state.started) state.reset();
        }

        public void handleMouseMove(Vector3Int newPosition) {
            if (state.started) {
                state.update(newPosition);
            }
        }
    }
}