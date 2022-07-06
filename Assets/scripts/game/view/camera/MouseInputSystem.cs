using System.Collections.Generic;
using System.Linq;
using game.model;
using game.view.util;
using UnityEngine;
using UnityEngine.EventSystems;
using util.lang.extension;
using Image = UnityEngine.UI.Image;

namespace game.view.camera {
    public class MouseInputSystem {
        private MouseMovementSystem mouseMovementSystem;
        private SelectionHandler selectionHandler;

        public void init() {
            selectionHandler = GameView.get().cameraAndMouseHandler.selectionHandler;
            mouseMovementSystem = GameView.get().cameraAndMouseHandler.mouseMovementSystem;
        }

        public void update() {
            Vector3Int modelPosition = ViewUtil.fromSceneToModelInt(GameView.get().screenToScenePosition(Input.mousePosition));
            bool inScreen = GameView.get().screenBounds.isIn(Input.mousePosition);
            if (Input.GetMouseButtonDown(0)
                && inScreen // in screen 
                && GameModel.localMap.bounds.isIn(modelPosition)
                && !clickIsOverUi()) {
                selectionHandler.handleMouseDown(modelPosition);
            }
            if (inScreen) {
                selectionHandler.handleMouseMove();
                mouseMovementSystem.setTarget(GameView.get().screenToScenePosition(Input.mousePosition));
            }
            if (Input.GetMouseButtonUp(0)) selectionHandler.handleMouseUp();
            if (Input.GetMouseButtonDown(1)) selectionHandler.handleSecondaryMouseClick();
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