using System.Collections.Generic;
using System.Linq;
using game.view;
using game.view.camera;
using game.view.util;
using UnityEngine;
using UnityEngine.EventSystems;
using util.lang.extension;
using Image = UnityEngine.UI.Image;

namespace game.input {
// Reads input from mouse, passes them to MouseMovementSystem for updating visual, to SelectionHandler for updating logic
public class MouseInputSystem {
    private CameraMovementSystem cameraMovementSystem;
    private SelectionHandler selectionHandler;
    private EntitySelector selector;
    private const int LMB = 0;
    private const int RMB = 1;

    // detects mouse move, scroll, and mouse clicks
    public void update() {
        // if in screen, handle moves and lmb clicks
        if (GameView.get().screenBounds.isIn(Input.mousePosition)) {
            Vector3Int modelPosition = ViewUtil.mouseScreenPositionToModel(Input.mousePosition, GameView.get());
            selector.setPosition(modelPosition); // put selector to mouse position
            selectionHandler.handleMouseMove(selector.position); // update selection handler with selector position
            if (Input.GetMouseButtonDown(LMB) && !mouseIsOverUi()) { // pass click to selection handler
                selectionHandler.handleMouseDown(selector.position);
            }
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDelta != 0 && !mouseIsOverUi()) { // pass zoom to camera system
                cameraMovementSystem.zoomCamera(zoomDelta);
            }
        }
        if (Input.GetMouseButtonUp(LMB)) selectionHandler.handleMouseUp();
        if (Input.GetMouseButtonDown(RMB)) selectionHandler.handleSecondaryMouseClick();
    }

    public void init() {
        selectionHandler = GameView.get().cameraAndMouseHandler.selectionHandler;
        cameraMovementSystem = GameView.get().cameraAndMouseHandler.cameraMovementSystem;
        selector = GameView.get().cameraAndMouseHandler.selector;
    }

    // raycasts mouse position to ui element
    private bool mouseIsOverUi() {
        PointerEventData data = new(EventSystem.current);
        data.position = Input.mousePosition;
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(data, results);
        return results.Any(result => result.gameObject.hasComponent<Image>()); // all ui elements have background with image component
    }
}
}