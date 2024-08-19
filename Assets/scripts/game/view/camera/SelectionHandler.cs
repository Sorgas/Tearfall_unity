using game.view.system.mouse_tool;
using UnityEngine;

namespace game.view.camera {
    // Handles input events of mouse. Receives model positions as input.
    // Stores state and logic of selecting tiles with frames.
    // Passes completed selections to MouseToolManager
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

        
        public void handleSecondaryMouseUp(Vector3Int position) {
            if (state.started) {
                state.reset();
            } else {
                MouseToolManager.get().handleRightClick(position);
            }
        }
        
        // reset selection frame when rmb clicked offscreen
        public void handleSecondaryMouseOffscreenUp() {
            if (state.started) {
                state.reset();
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