﻿using System.Collections.Generic;
using System.Linq;
using game.model;
using game.view;
using game.view.camera;
using game.view.util;
using UnityEngine;
using UnityEngine.EventSystems;
using util.lang.extension;
using Image = UnityEngine.UI.Image;

namespace game.input {
    // reads input events from mouse, passes them to MouseMovementSystem for updating visual, to SelectionHandler for updating logic
    public class MouseInputSystem {
        private MouseMovementSystem mouseMovementSystem;
        private SelectionHandler selectionHandler;

        public void update() {
            // if in screen, handle moves and lmb clicks
            if (GameView.get().screenBounds.isIn(Input.mousePosition)) {
                Vector3Int modelPosition = ViewUtil.fromScreenToModel(Input.mousePosition, GameView.get());
                modelPosition = GameView.get().selector.updatePosition(modelPosition);
                selectionHandler.handleMouseMove(modelPosition);
                mouseMovementSystem.updateTarget(modelPosition);
                // if pressed inside map and not on ui, start selection
                if (Input.GetMouseButtonDown(0)
                    && GameModel.get().currentLocalModel.localMap.bounds.isIn(modelPosition)
                    && !clickIsOverUi()) {
                    selectionHandler.handleMouseDown(modelPosition);
                }
            }
            if (Input.GetMouseButtonUp(0)) selectionHandler.handleMouseUp();
            if (Input.GetMouseButtonDown(1)) selectionHandler.handleSecondaryMouseClick();
        }

        public void init() {
            selectionHandler = GameView.get().cameraAndMouseHandler.selectionHandler;
            mouseMovementSystem = GameView.get().cameraAndMouseHandler.mouseMovementSystem;
        }

        // raycasts mouse position to ui element
        private bool clickIsOverUi() {
            PointerEventData data = new(EventSystem.current);
            data.position = Input.mousePosition;
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(data, results);
            return results.Any(result => result.gameObject.hasComponent<Image>());
        }
    }
}