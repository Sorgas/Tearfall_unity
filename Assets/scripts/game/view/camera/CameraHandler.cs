using game.view.no_es;
using game.view.with_entity_selector;

namespace game.view {
    public class CameraHandler {
        // with ES
        private EntitySelectorInputSystem entitySelectorInputSystem;
        private EntitySelectorVisualMovementSystem entitySelectorVisualMovementSystem;
        public CameraWithEsMovementSystem cameraWithEsMovementSystem;

        // without ES
        private CameraInputSystem cameraInputSystem;
        public CameraMovementSystem cameraMovementSystem;
        private MouseInputSystem mouseInputSystem;
        public MouseMovementSystem mouseMovementSystem;
        public bool enabled = true;
        private bool useSelector;

        public CameraHandler(LocalGameRunner initializer, bool useSelector) {
            this.useSelector = useSelector;
            if (useSelector) {
                cameraWithEsMovementSystem = new CameraWithEsMovementSystem(initializer.mainCamera, initializer.selector);
                entitySelectorVisualMovementSystem = new EntitySelectorVisualMovementSystem(initializer);
                entitySelectorInputSystem = new EntitySelectorInputSystem(initializer, cameraWithEsMovementSystem);
            } else {
                mouseMovementSystem = new MouseMovementSystem(initializer);
                cameraMovementSystem = new CameraMovementSystem(initializer.mainCamera);
                mouseInputSystem = new MouseInputSystem(initializer, mouseMovementSystem, cameraMovementSystem);
                cameraInputSystem = new CameraInputSystem(mouseMovementSystem, cameraMovementSystem);
            }
        }

        public void update() {
            if (!enabled) return;
            if (!useSelector) {
                mouseInputSystem.update();
                mouseMovementSystem.update();
                cameraInputSystem.update();
                cameraMovementSystem.update();
            } else {
                entitySelectorInputSystem?.update();
                entitySelectorVisualMovementSystem?.update();
                cameraWithEsMovementSystem?.update();
                cameraInputSystem?.update();
            }
        }
    }
}