using System.Collections.Generic;
using game.model;
using game.model.entity_selector;
using game.model.localmap;
using UnityEngine;
using util.geometry;
using util.input;

// moves selector in model
namespace game.view.with_entity_selector {
    public class EntitySelectorInputSystem {
        public Camera camera;
        public bool enabled = true;
        private LocalMap localMap;
        private RectTransform mapHolder;
        private EntitySelector selector = GameModel.get().selector;
        private EntitySelectorSystem system = GameModel.get().selectorSystem;
        // private EntitySelectorVisualMovementSystem visualSystem;
        private CameraMovementSystem cameraSystem; // for zoom
        private List<DelayedConditionController> controllers = new List<DelayedConditionController>();
        private IntBounds2 screenBounds = new IntBounds2(Screen.width, Screen.height);
        // private Text text;

        public EntitySelectorInputSystem(LocalGameRunner initializer) {
            this.camera = initializer.mainCamera;
            mapHolder = initializer.mapHolder;
            localMap = GameModel.get().localMap;
            int xSize = localMap.xSize;
            int ySize = localMap.ySize;
            // this.text = initializer.text;
            screenBounds.extendX((int)(-Screen.width * 0.01f));
            screenBounds.extendY((int)(-Screen.height * 0.01f));
            // visualSystem = new EntitySelectorVisualMovementSystem(camera, initializer.selector, initializer.mapHolder);
            cameraSystem = new CameraMovementSystem(camera, initializer.selector);
            initControllers();
        }

        public void init() {
        }

        private void initControllers() {
            controllers.Add(new DelayedKeyController(KeyCode.W, () => handleWASD(0, 1)));
            controllers.Add(new DelayedKeyController(KeyCode.A, () => handleWASD(-1, 0)));
            controllers.Add(new DelayedKeyController(KeyCode.S, () => handleWASD(0, -1)));
            controllers.Add(new DelayedKeyController(KeyCode.D, () => handleWASD(1, 0)));
            // layers of map are placed with z gap 2 and shifted by y by 0.5
            controllers.Add(new DelayedKeyController(KeyCode.R, () => changeLayer(1)));
            controllers.Add(new DelayedKeyController(KeyCode.F, () => changeLayer(-1)));
            // move camera when mouse on screen border
            controllers.Add(new DelayedConditionController(() => moveSelector(0, 1), () => (Input.mousePosition.y > Screen.height * 0.99f && Input.mousePosition.y <= Screen.height)));
            controllers.Add(new DelayedConditionController(() => moveSelector(0, -1), () => (Input.mousePosition.y < Screen.height * 0.01f && Input.mousePosition.y >= 0)));
            controllers.Add(new DelayedConditionController(() => moveSelector(1, 0), () => (Input.mousePosition.x > Screen.width * 0.99f && Input.mousePosition.x <= Screen.width)));
            controllers.Add(new DelayedConditionController(() => moveSelector(-1, 0), () => (Input.mousePosition.x < Screen.width * 0.01f && Input.mousePosition.x >= 0)));
        }

        public void update() {
            if (!enabled) return;
            GameView.get().selectorOverlook.Set(0, 0);
            float deltaTime = Time.deltaTime;
            Vector3Int currentPosition = selector.position;
            controllers.ForEach(controller => controller.update(deltaTime));
            cameraSystem.zoomCamera(Input.GetAxis("Mouse ScrollWheel"));
            // mouse moved inside screen
            if (screenBounds.isIn(Input.mousePosition) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
                resetSelectorToMousePosition(Input.mousePosition); // update selector position
            
        }

        // moves selector in current z-layer
        private void handleWASD(int dx, int dy) {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { // adjust delta for faster scrolling
                dx *= 10;
                dy *= 10;
            }
            moveSelector(dx, dy);
        }

        private void moveSelector(int dx, int dy) {
            Vector3Int delta = system.moveSelector(dx, dy, 0);
            if(dx - delta.x < 0) {
                GameView.get().selectorOverlook.x = -1;
            } else if(dx - delta.x > 0) {
                GameView.get().selectorOverlook.x = 1;
            }
            if(dy - delta.y < 0) {
                GameView.get().selectorOverlook.y = -1;
            } else if(dy - delta.y > 0) {
                GameView.get().selectorOverlook.y = 1;
            }
        }

        public void resetSelectorToMousePosition(Vector3 mousePosition) {
            Vector3 worldPosition = camera.ScreenToWorldPoint(mousePosition);
            Vector3 mapHolderPosition = mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
            int zLayer = selector.position.z; // z-layer cannot be changed by moving mouse
            system.setSelectorPosition((int)mapHolderPosition.x, (int)(-zLayer / 2f + mapHolderPosition.y), zLayer);
        }

        private void changeLayer(int dz) {
            system.moveSelector(0, 0, dz);
        }
    }
}
