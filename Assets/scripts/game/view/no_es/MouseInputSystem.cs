using System.Collections.Generic;
using UnityEngine;
using util.geometry;
using util.input;

namespace game.view.no_es {
    public class MouseInputSystem {
        public bool enabled = true;
        private readonly Camera camera;
        private readonly RectTransform mapHolder;
        private readonly MouseMovementSystem mouseMovementSystem;
        private readonly CameraMovementSystem2 cameraMovementSystem;
        private readonly List<DelayedConditionController> controllers = new List<DelayedConditionController>();
        private readonly IntBounds2 screenBounds = new IntBounds2(Screen.width, Screen.height);

        public MouseInputSystem(LocalGameRunner initializer) {
            mouseMovementSystem = GameView.get().mouseMovementSystem;
            cameraMovementSystem = GameView.get().cameraMovementSystem2;
            mapHolder = initializer.mapHolder;
            camera = initializer.mainCamera;
            screenBounds.extendX((int)(-Screen.width * 0.01f));
            screenBounds.extendY((int)(-Screen.height * 0.01f));
            initControllers();
        }

        private void initControllers() {
            // move camera when mouse on screen border
            controllers.Add(new DelayedConditionController(() => panCamera(0, 1),
                () => Input.mousePosition.y > Screen.height * 0.99f && Input.mousePosition.y <= Screen.height));
            controllers.Add(new DelayedConditionController(() => panCamera(0, -1),
                () => Input.mousePosition.y < Screen.height * 0.01f && Input.mousePosition.y >= 0));
            controllers.Add(new DelayedConditionController(() => panCamera(1, 0),
                () => Input.mousePosition.x > Screen.width * 0.99f && Input.mousePosition.x <= Screen.width));
            controllers.Add(new DelayedConditionController(() => panCamera(-1, 0),
                () => Input.mousePosition.x < Screen.width * 0.01f && Input.mousePosition.x >= 0));
            // TODO handle click
        }

        public void update() {
            if (!enabled) return;
            float deltaTime = Time.deltaTime;
            controllers.ForEach(controller => controller.update(deltaTime));
            // mouse moved inside screen
            if (screenBounds.isIn(Input.mousePosition) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
                mouseMovementSystem.setTarget(screenToScenePosition(Input.mousePosition));
        }

        // moves camera when mouse is on screen border
        private void panCamera(int dx, int dy) => cameraMovementSystem.moveCameraTarget(dx, dy);

        private Vector3 screenToScenePosition(Vector3 screenPosition) {
            Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);
            return mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
        }
    }
}