using game.view.system.mouse_tool;
using UnityEngine;

namespace game.view.camera {
    // stores state of selection
    // TODO cancel selection when type changed
    public class SelectionHandler {
        private Vector3Int start; // inclusive start of selection
        private Vector3Int finish; // inclusive finish of selection
        private MouseMovementSystem mouseMovementSystem;
        public SelectionState state = new();
        private bool started;
        public bool enabled = true;

        public void init() {
            mouseMovementSystem = GameView.get().cameraAndMouseHandler.mouseMovementSystem;
            state.updater = GameView.get().tileUpdater;
        }

        public void handleMouseDown() {
            if(enabled && !started) startSelection();
        }

        public void handleMouseUp() {
            if(started) finishSelection();
        }

        public void handleSecondaryMouseClick() {
            if(started) cancelSelection();
        }
        
        public void handleMouseMove() {
            if (started) {
                Vector3Int newPosition = mouseMovementSystem.getTarget();
                state.update(newPosition);
            }
        }
        
        private void startSelection() {
            started = true;
            state.startSelection(mouseMovementSystem.getTarget());
        }

        private void finishSelection() {
            started = false;
            MouseToolManager.handleSelection(state.bounds);
            state.reset();
        }

        private void cancelSelection() {
            started = false;
        }
    }
}