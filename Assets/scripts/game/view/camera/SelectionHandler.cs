using game.view.system.mouse_tool;
using UnityEngine;

namespace game.view.camera {
    // handles input events of mouse. stores state and logic of selecting tiles with frames.
    // passes completed selections to MouseToolManager
    // TODO cancel selection when type changed
    // TODO add visual validation for designations
    // TODO rename to not have 'handler' in name, handlers are ui 
    public class SelectionHandler {
        public SelectionState state = new();
        public bool enabled = true;
        public bool debug = false;

        public SelectionHandler() {
            state.updater = GameView.get().tileUpdater;
        }

        public void handleMouseDown(Vector3Int position) {
            log(position.ToString());
            if (enabled && !state.started) state.startSelection(position);
        }

        // finishes selection
        public void handleMouseUp() {
            if (state.started) {
                log(state.bounds.toString());
                MouseToolManager.get().handleSelection(state.bounds.clone(), state.selectionStart);
                state.reset();
            }
        }

        public void handleSecondaryMouseClick() {
            if (state.started) {
                state.reset();
            } else {
                MouseToolManager.get().reset();
                // TODO make mouse tool always align with state of toolbar (close toolbar on RMB click)
                // WindowManager.get().closeAll();
                // TODO closing menus on RMB click (ES2)
                
            }
        }

        public void handleMouseMove(Vector3Int newPosition) {
            if (state.started) {
                state.update(newPosition);
            }
        }

        private void log(string message) {
            if(debug) Debug.Log($"[Selection Handler] {message}");
        }
    }
}