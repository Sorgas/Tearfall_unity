using UnityEngine;
using util.geometry;

namespace game.view.camera {
    // stores state of selection
    public class SelectionHandler {
        private Vector3Int start; // inclusive start of selection
        private Vector3Int finish; // inclusive finish of selection
        private MouseMovementSystem mouseMovementSystem;
        private SelectionState state = new SelectionState();
        private bool started = false;
        private IntBounds3 bounds = new IntBounds3();
        public bool enabled = true;
        
        public void init() {
            mouseMovementSystem = GameView.get().cameraAndMouseHandler.mouseMovementSystem;
        }
        
        public void update() {
            if (!enabled) return;
            if (Input.GetMouseButtonDown(0)) {
                handleMouseDown();
            }
            if (Input.GetMouseButtonUp(0)) {
                handleMouseUp();
            }
            if (Input.GetMouseButtonDown(1)) {
                handleSecondaryMouseClick();
            }
        }

        public void handleMouseDown() {
            if(!started) startSelection();
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
            Debug.Log("selection started " + mouseMovementSystem.getTarget());
        }

        private void finishSelection() {
            started = false;
            state.reset();
            Debug.Log("selection finished " + mouseMovementSystem.getTarget());

        }

        private void cancelSelection() {
            started = false;
            Debug.Log("selection canceled " + mouseMovementSystem.getTarget());
        }
    }
}