using game.view.camera;
using UnityEngine;

namespace game.input {
    // container for all logic of mouse and camera
    public class CameraAndMouseHandler {
        private readonly CameraInputSystem cameraInputSystem;
        public readonly CameraMovementSystem cameraMovementSystem = new();
        public readonly MouseInputSystem mouseInputSystem = new(); // handles mouse movement and clicks
        public readonly SelectionHandler selectionHandler = new();
        public readonly EntitySelector selector = new(); 
        public bool enabled = true;

        public CameraAndMouseHandler(LocalGameRunner initializer, PlayerControls playerControls) {
            // TODO link systems after creation with init() methods
            cameraInputSystem = new CameraInputSystem(cameraMovementSystem, playerControls);
            playerControls.Enable();
        }

        public void init() {
            selectionHandler.init();
            mouseInputSystem.init();
            cameraMovementSystem.init();
            selector.init();
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