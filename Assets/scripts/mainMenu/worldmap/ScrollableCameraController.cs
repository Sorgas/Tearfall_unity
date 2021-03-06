﻿using System;
using Assets.scripts.util.geometry;
using UnityEngine;

namespace Assets.scripts.mainMenu.worldmap {
    // controls camera of worldmap. handles zoom input, moves camera to make pointer visible.
    public class ScrollableCameraController {
        public Camera camera;
        public Rect paneSize;
        public int worldSize;

        private WorldmapPointerController pointerController;
        private ValueRange cameraFovRange = new ValueRange(4, 40);
        private Vector3 speed = new Vector3();
        private FloatBounds2 bounds = new FloatBounds2(); // camera bounds
        private int padding = 2; // blang tiles on map sides
        private FloatBounds2 effectiveCameraSize = new FloatBounds2(); // number of visible tiles, relative to camera position
        private FloatBounds2 visibleArea = new FloatBounds2();

        public ScrollableCameraController(Rect paneSize, RectTransform image, Camera camera, int worldSize, WorldmapPointerController pointerController) {
            this.paneSize = paneSize;
            this.camera = camera;
            this.worldSize = worldSize;
            this.pointerController = pointerController;
            defineCameraBounds();
        }

        public void handleInput() {
            zoomMinimap(Input.GetAxis("Mouse ScrollWheel"));
            ensureCameraBounds();
            updateCameraPosition();
            checkPointer();
        }

        private void updateCameraPosition() {
            Vector3 position = camera.transform.localPosition;
            if (speed.x != 0) {
                if (position.x - bounds.minX < -speed.x) speed.x = bounds.minX - position.x;
                if (bounds.maxX - position.x < speed.x) speed.x = bounds.maxX - position.x;
            }
            if (speed.y != 0) {
                if (position.y - bounds.minY < -speed.y) speed.y = bounds.minY - position.y;
                if (bounds.maxY - position.y < speed.y) speed.y = bounds.maxY - position.y;
            }
            camera.transform.Translate(speed, Space.Self);
            speed.x *= 0.9f;
            speed.y *= 0.9f;
            if (Math.Abs(speed.x) < 0.1f) speed.x = 0;
            if (Math.Abs(speed.y) < 0.1f) speed.y = 0;
        }

        //Defines rectangular bounds where camera can move. Supports fixed padding on world borders.
        private void defineCameraBounds() {
            bounds.set(0, 0, worldSize, worldSize);
            bounds.extend(padding - camera.orthographicSize); // add padding to map to clearly show world's borders
            if (bounds.minX > bounds.maxX) {
                bounds.minX = (worldSize + padding) / 2;
                bounds.maxX = bounds.minX;
            }
            float hiddenTiles = (1f - paneSize.height / paneSize.width) * camera.orthographicSize;
            bounds.minY -= hiddenTiles;
            bounds.maxY += hiddenTiles;
            effectiveCameraSize.set(-camera.orthographicSize, hiddenTiles - camera.orthographicSize, camera.orthographicSize - 1, camera.orthographicSize - hiddenTiles - 1);
            Debug.Log(effectiveCameraSize);
            cameraFovRange.max = worldSize / 2 + padding + hiddenTiles;
        }

        private void ensureCameraBounds() {
            Vector3 position = camera.transform.localPosition;
            Vector3 translation = new Vector3();
            if (!bounds.isIn(position)) { // scroll into bounds, if camera get out on scrolling
                if (position.x < bounds.minX) translation.x = bounds.minX - position.x;
                if (position.x > bounds.maxX) translation.x = bounds.maxX - position.x;
                if (position.y < bounds.minY) translation.y = bounds.minY - position.y;
                if (position.y > bounds.maxY) translation.y = bounds.maxY - position.y;
                camera.transform.Translate(translation, Space.Self);
            }
        }

        private void zoomMinimap(float delta) {
            if (delta == 0) return;
            float oldZoom = camera.orthographicSize;
            camera.orthographicSize = cameraFovRange.clamp(camera.orthographicSize + delta * 2);
            if (oldZoom != camera.orthographicSize) defineCameraBounds();
        }

        // moves camera towards pointer
        private void checkPointer() {
            visibleArea.set(effectiveCameraSize).move(camera.transform.localPosition);
            speed = visibleArea.getDirectionVector(pointerController.targetPosition);
        }
    }
}