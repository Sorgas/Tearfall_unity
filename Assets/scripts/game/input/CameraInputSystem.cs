using System.Collections.Generic;
using game.view;
using game.view.camera;
using UnityEngine;
using util.input;

// moves selector in model
namespace game.input {
    public class CameraInputSystem {
        private readonly CameraMovementSystem cameraMovementSystem;
        private readonly List<DelayedConditionController> controllers = new();
        
        public CameraInputSystem(CameraMovementSystem cameraMovementSystem, PlayerControls playerControls) {
            this.cameraMovementSystem = cameraMovementSystem;
            controllers.Add(new DelayedCompositeNavigationController(playerControls.Player.CameraMove, handleWasd));
            controllers.Add(new DelayedLayersController(playerControls.Player.ChangeLayer, changeLayer));
            // move camera when mouse on screen border
            controllers.Add(new DelayedConditionController(() => moveCameraTarget(0, 1),
                () => (Input.mousePosition.y > Screen.height * 0.99f && Input.mousePosition.y <= Screen.height)));
            controllers.Add(new DelayedConditionController(() => moveCameraTarget(0, -1),
                () => (Input.mousePosition.y < Screen.height * 0.01f && Input.mousePosition.y >= 0)));
            controllers.Add(new DelayedConditionController(() => moveCameraTarget(1, 0),
                () => (Input.mousePosition.x > Screen.width * 0.99f && Input.mousePosition.x <= Screen.width)));
            controllers.Add(new DelayedConditionController(() => moveCameraTarget(-1, 0),
                () => (Input.mousePosition.x < Screen.width * 0.01f && Input.mousePosition.x >= 0)));
        }

        public void update() {
            float deltaTime = Time.deltaTime;
            if (GameView.get().screenBounds.isIn(Input.mousePosition)) {
                controllers.ForEach(controller => controller.update(deltaTime));
                float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
                if(zoomDelta != 0) cameraMovementSystem.zoomCamera(zoomDelta);
            }
        }

        private void handleWasd(int dx, int dy) {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                // adjust delta for faster scrolling
                dx *= 10;
                dy *= 10;
            }
            moveCameraTarget(dx, dy);
        }

        private void moveCameraTarget(int dx, int dy) {
            cameraMovementSystem.moveCameraTarget(dx, dy);
        }

        private void changeLayer(int dz) {
            if (dz != 0) cameraMovementSystem.moveCameraTargetZ(dz);
        }
    }
}