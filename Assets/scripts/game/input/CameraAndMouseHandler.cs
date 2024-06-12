using game.view.camera;
using UnityEngine;

namespace game.input {
    // container for all logic of mouse and camera
    public class CameraAndMouseHandler {
        public readonly EntitySelector selector = new(); 
        public readonly SelectionHandler selectionHandler = new();
        public readonly CameraMovementSystem cameraMovementSystem = new();
        private readonly CameraInputSystem cameraInputSystem;
        public readonly MouseInputSystem mouseInputSystem; // handles mouse movement and clicks
        public bool enabled = true;

        public CameraAndMouseHandler(PlayerControls playerControls) {
            cameraInputSystem = new CameraInputSystem(cameraMovementSystem, playerControls);
            mouseInputSystem = new MouseInputSystem(cameraMovementSystem, selectionHandler, selector);
            playerControls.Enable();
        }

        public void update() {
            if (!enabled) return;
            cameraInputSystem.update();
            cameraMovementSystem.update();
            mouseInputSystem.update();
        }
        
    public void resetCameraPosition(Vector3Int cameraPosition) {
        selector.setPosition(cameraPosition);
        selector.setLayer(cameraPosition.z + 1); // hack to disable unseen levels renderers
        selector.setLayer(cameraPosition.z);
        cameraMovementSystem.setCameraTarget(cameraPosition, true);
    }
}

// move selector and camera to ground level in map center
}