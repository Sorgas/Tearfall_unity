using game.view.system.mouse_tool;
using UnityEngine;

namespace game.view.camera {
    // stores state of selection
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

        public void handleMouseUp() {
            if (state.started) finishSelection();
        }

        public void handleSecondaryMouseClick() {
            if (state.started) state.reset();
        }

        public void handleMouseMove() {
            if (state.started) {
                Vector3Int newPosition = GameView.get().selectorPosition;
                state.update(newPosition);
                // MouseToolManager.validate();
            }
        }

        private void finishSelection() {
            MouseToolManager.handleSelection(state.bounds);
            state.reset();
        }
    }
}