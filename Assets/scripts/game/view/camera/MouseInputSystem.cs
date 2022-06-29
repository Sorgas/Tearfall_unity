using System.Collections.Generic;
using System.Linq;
using game.model;
using game.view.util;
using UnityEngine;
using UnityEngine.EventSystems;
using util.geometry.bounds;
using util.lang.extension;
using Image = UnityEngine.UI.Image;

namespace game.view.camera {
    // fetches mousePosition from Input, calls MouseMovementSystem and CameraMovementSystem
    // handles:
    //      mouse move -> update selector position,
    //      mouse click -> call selection handler,
    public class MouseInputSystem {
        private readonly Camera camera;
        private readonly RectTransform mapHolder;
        private MouseMovementSystem mouseMovementSystem;
        private readonly IntBounds2 screenBounds = new(Screen.width, Screen.height);
        private SelectionHandler selectionHandler;

        public MouseInputSystem(LocalGameRunner initializer) {
            mapHolder = initializer.mapHolder;
            camera = initializer.mainCamera;
            screenBounds.extendX((int)(-Screen.width * 0.01f));
            screenBounds.extendY((int)(-Screen.height * 0.01f));
        }

        public void init() {
            selectionHandler = GameView.get().cameraAndMouseHandler.selectionHandler;
            mouseMovementSystem = GameView.get().cameraAndMouseHandler.mouseMovementSystem;
        }

        public void update() {
            // mouse is inside screen
            if (screenBounds.isIn(Input.mousePosition)) {
                if (Input.GetMouseButtonDown(0) 
                    && GameModel.localMap.bounds.isIn(ViewUtil.fromSceneToModel(screenToScenePosition(Input.mousePosition)))
                    && !clickIsOverUi()) {
                    selectionHandler.handleMouseDown();
                }
            }
            mouseMovementSystem.setTarget(screenToScenePosition(Input.mousePosition));
            selectionHandler.handleMouseMove();
            if (Input.GetMouseButtonUp(0)) selectionHandler.handleMouseUp();
            if (Input.GetMouseButtonDown(1)) selectionHandler.handleSecondaryMouseClick();
        }

        private Vector3 screenToScenePosition(Vector3 screenPosition) {
            Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);
            return mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
        }

        private bool clickIsOverUi() {
            PointerEventData data = new(EventSystem.current);
            data.position = Input.mousePosition;
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(data, results);
            return results.Any(result => result.gameObject.hasComponent<Image>());
        }
    }
}