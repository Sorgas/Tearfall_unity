using game.view.no_es;
using game.view.with_entity_selector;

namespace game.view.camera {
    public class CameraAndMouseHandler {
        // without ES
        private CameraInputSystem cameraInputSystem;
        public CameraMovementSystem cameraMovementSystem;
        private MouseInputSystem mouseInputSystem;
        public MouseMovementSystem mouseMovementSystem;
        public SelectionHandler selectionHandler = new SelectionHandler();
        public bool enabled = true;

        public CameraAndMouseHandler(LocalGameRunner initializer) { // TODO link systems after creation with init() methods
            mouseMovementSystem = new MouseMovementSystem(initializer);
            cameraMovementSystem = new CameraMovementSystem(initializer.mainCamera);
            mouseInputSystem = new MouseInputSystem(initializer);
            cameraInputSystem = new CameraInputSystem(mouseInputSystem, mouseMovementSystem, cameraMovementSystem);
            cameraMovementSystem.mouseInputSystem = mouseInputSystem;
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