using game.view.camera;

namespace game.input {
    // container for all logic of mouse and camera
    public class CameraAndMouseHandler {
        private PlayerControls playerControls;
        private CameraInputSystem cameraInputSystem;
        public CameraMovementSystem cameraMovementSystem = new();
        private MouseInputSystem mouseInputSystem = new(); // handles mouse movement and clicks
        public MouseMovementSystem mouseMovementSystem; // moves selector go to 'follow' mouse on screen
        public SelectionHandler selectionHandler = new();
        public bool enabled = true;

        public CameraAndMouseHandler(LocalGameRunner initializer, PlayerControls playerControls) {
            // TODO link systems after creation with init() methods
            this.playerControls = playerControls;
            mouseMovementSystem = new MouseMovementSystem(initializer);
            cameraInputSystem = new CameraInputSystem(cameraMovementSystem, playerControls);
            playerControls.Enable();
        }

        public void init() {
            selectionHandler.init();
            mouseInputSystem.init();
            cameraMovementSystem.init();
        }

        public void update() {
            if (enabled) {
                mouseInputSystem.update();
                mouseMovementSystem.update();
                cameraInputSystem.update();
                cameraMovementSystem.update();
            }
        }
    }
}