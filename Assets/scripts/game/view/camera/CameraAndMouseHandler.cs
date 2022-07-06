namespace game.view.camera {
    // container for all logic of mouse and camera
    public class CameraAndMouseHandler {
        private CameraInputSystem cameraInputSystem;
        public CameraMovementSystem cameraMovementSystem = new();
        private MouseInputSystem mouseInputSystem = new(); // handles mouse movement and clicks
        public MouseMovementSystem mouseMovementSystem; // moves selector go to 'follow' mouse on screen
        public SelectionHandler selectionHandler = new();
        public bool enabled = true;

        public CameraAndMouseHandler(LocalGameRunner initializer) {
            // TODO link systems after creation with init() methods
            mouseMovementSystem = new MouseMovementSystem(initializer);
            cameraInputSystem = new CameraInputSystem(cameraMovementSystem);
        }

        public void init() {
            selectionHandler.init();
            mouseInputSystem.init();
            cameraMovementSystem.init();
        }

        public void update() {
            if (!enabled) return;
            mouseInputSystem.update();
            mouseMovementSystem.update();
            cameraInputSystem.update();
            cameraMovementSystem.update();
        }
    }
}