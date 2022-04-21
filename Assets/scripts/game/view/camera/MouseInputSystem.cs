using System.Collections.Generic;
using System.Linq;
using game.model;
using game.view.no_es;
using game.view.util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using util.geometry;
using util.input;
using util.lang;
using util.lang.extension;
using Image = UnityEngine.UI.Image;

namespace game.view.camera {
    // fetches mousePosition from Input, calls MouseMovementSystem and CameraMovementSystem
    public class MouseInputSystem {
        private readonly Camera camera;
        private readonly RectTransform mapHolder;
        private MouseMovementSystem mouseMovementSystem;
        private CameraMovementSystem cameraMovementSystem;
        private readonly List<DelayedConditionController> controllers = new List<DelayedConditionController>();
        private readonly IntBounds2 screenBounds = new IntBounds2(Screen.width, Screen.height);
        private SelectionHandler selectionHandler;

        public MouseInputSystem(LocalGameRunner initializer) {
            mapHolder = initializer.mapHolder;
            camera = initializer.mainCamera;
            screenBounds.extendX((int)(-Screen.width * 0.01f));
            screenBounds.extendY((int)(-Screen.height * 0.01f));
            initControllers();
        }

        public void init() {
            selectionHandler = GameView.get().cameraAndMouseHandler.selectionHandler;
            mouseMovementSystem = GameView.get().cameraAndMouseHandler.mouseMovementSystem;
            cameraMovementSystem = GameView.get().cameraAndMouseHandler.cameraMovementSystem;
        }

        private void initControllers() {
            // move camera when mouse is on screen border
            controllers.Add(new DelayedConditionController(() => panCamera(0, 1),
                () => Input.mousePosition.y > Screen.height * 0.99f && Input.mousePosition.y <= Screen.height));
            controllers.Add(new DelayedConditionController(() => panCamera(0, -1),
                () => Input.mousePosition.y < Screen.height * 0.01f && Input.mousePosition.y >= 0));
            controllers.Add(new DelayedConditionController(() => panCamera(1, 0),
                () => Input.mousePosition.x > Screen.width * 0.99f && Input.mousePosition.x <= Screen.width));
            controllers.Add(new DelayedConditionController(() => panCamera(-1, 0),
                () => Input.mousePosition.x < Screen.width * 0.01f && Input.mousePosition.x >= 0));
        }

        public void update() {
            float deltaTime = Time.deltaTime;
            controllers.ForEach(controller => controller.update(deltaTime));
            // mouse is inside screen
            if (screenBounds.isIn(Input.mousePosition)) {
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
                    setSelectorToMousePosition();
                }
                if (Input.GetMouseButtonDown(0) 
                    && GameModel.localMap.bounds.isIn(ViewUtil.fromSceneToModel(screenToScenePosition(Input.mousePosition)))
                    && !checkClickIsOverUi()) {
                    selectionHandler.handleMouseDown();
                }
            }
            handleSelectionInput();
        }

        private void handleSelectionInput() {
            if (Input.GetMouseButtonUp(0)) selectionHandler.handleMouseUp();
            if (Input.GetMouseButtonDown(1)) selectionHandler.handleSecondaryMouseClick();
        }

        // moves camera when mouse is on screen border
        private void panCamera(int dx, int dy) => cameraMovementSystem.moveCameraTarget(dx, dy);

        public void setSelectorToMousePosition() {
            mouseMovementSystem.setTarget(screenToScenePosition(Input.mousePosition));
            selectionHandler.handleMouseMove();
        }

        private Vector3 screenToScenePosition(Vector3 screenPosition) {
            Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);
            return mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
        }

        private bool checkClickIsOverUi() {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            return results.Any(result => result.gameObject.hasComponent<Image>());
        }
    }
}