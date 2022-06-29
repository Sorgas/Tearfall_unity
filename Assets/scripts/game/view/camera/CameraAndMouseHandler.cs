namespace game.view.camera {
    // facade for all logic of mouse and camera
    public class CameraAndMouseHandler {
        private CameraInputSystem cameraInputSystem;
        public CameraMovementSystem cameraMovementSystem;
        private MouseInputSystem mouseInputSystem; // handles mouse movement and clicks
        public MouseMovementSystem mouseMovementSystem; // moves selector go to 'follow' mouse on screen
        public SelectionHandler selectionHandler = new();
        public bool enabled = true;

        public CameraAndMouseHandler(LocalGameRunner initializer) { // TODO link systems after creation with init() methods
            mouseMovementSystem = new MouseMovementSystem(initializer);
            cameraMovementSystem = new CameraMovementSystem(initializer.mainCamera);
            mouseInputSystem = new MouseInputSystem(initializer);
            cameraInputSystem = new CameraInputSystem(cameraMovementSystem);
        }
        public void init() {
            selectionHandler.init();
            mouseInputSystem.init();
        }
        
        public void update() {
            if (!enabled) return;
            mouseInputSystem.update();
            mouseMovementSystem.update();
            cameraInputSystem.update();
            cameraMovementSystem.update();
        }

        // with ES
        // private EntitySelectorInputSystem entitySelectorInputSystem;
        // private EntitySelectorVisualMovementSystem entitySelectorVisualMovementSystem;
        // public CameraWithEsMovementSystem cameraWithEsMovementSystem;
        
        // cameraWithEsMovementSystem = new CameraWithEsMovementSystem(initializer.mainCamera, initializer.selector);
        // entitySelectorVisualMovementSystem = new EntitySelectorVisualMovementSystem(initializer);
        // entitySelectorInputSystem = new EntitySelectorInputSystem(initializer, cameraWithEsMovementSystem);

        // entitySelectorInputSystem?.update();
        // entitySelectorVisualMovementSystem?.update();
        // cameraWithEsMovementSystem?.update();
        // cameraInputSystem?.update();
    }
}