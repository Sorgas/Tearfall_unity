using System;
using System.Collections.Generic;
using Assets.scripts.util;
using Assets.scripts.util.geometry;
using Assets.scripts.util.input;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.view {
    public class LocalMapCameraController {
        public Camera camera;
        public int worldSize;

        //TODO private WorldmapPointerController pointerController; 
        private ValueRange cameraFovRange = new ValueRange(4, 40);
        private Vector3 speed = new Vector3();
        private FloatBounds2 bounds = new FloatBounds2(); // camera bounds for current layer
        private int padding = 2; // blanc tiles on map sides
        private FloatBounds2 effectiveCameraSize = new FloatBounds2(); // number of visible tiles, relative to camera position
        private FloatBounds2 visibleArea = new FloatBounds2();
        private List<DelayedKeyController> controllers = new List<DelayedKeyController>();

        public LocalMapCameraController(Camera camera, int worldSize) {
            this.camera = camera;
            this.worldSize = worldSize;
            defineCameraBounds();
            controllers.Add(new DelayedKeyController(KeyCode.W, () => moveCamera(0, 1, 0)));
            controllers.Add(new DelayedKeyController(KeyCode.A, () => moveCamera(-1, 0, 0)));
            controllers.Add(new DelayedKeyController(KeyCode.S, () => moveCamera(0, -1, 0)));
            controllers.Add(new DelayedKeyController(KeyCode.D, () => moveCamera(1, 0, 0)));
            controllers.Add(new DelayedKeyController(KeyCode.R, () => moveCamera(0, 0.5f, -2)));
            controllers.Add(new DelayedKeyController(KeyCode.F, () => moveCamera(0, -0.5f, 2)));
        }

        public void handleInput() {
            zoom(Input.GetAxis("Mouse ScrollWheel"));
            float deltaTime = Time.deltaTime;
            controllers.ForEach(controller => controller.update(deltaTime));
            // ensureCameraBounds();
            updateCameraPosition();
            // checkPointer();
        }

        private void updateCameraPosition() {
            clampSpeed();
            camera.transform.Translate(speed, Space.Self);
            speed.x *= 0.9f;
            speed.y *= 0.9f;
            if (Math.Abs(speed.x) < 0.1f) speed.x = 0;
            if (Math.Abs(speed.y) < 0.1f) speed.y = 0;
        }

        // ensures, that speed not too high to move camera offbounds in one update
        private void clampSpeed() {
            Vector3 position = camera.transform.localPosition;
            if (speed.x != 0) {
                if (position.x - bounds.minX < -speed.x) speed.x = bounds.minX - position.x;
                if (bounds.maxX - position.x < speed.x) speed.x = bounds.maxX - position.x;
            }
            if (speed.y != 0) {
                if (position.y - bounds.minY < -speed.y) speed.y = bounds.minY - position.y;
                if (bounds.maxY - position.y < speed.y) speed.y = bounds.maxY - position.y;
            }
        }

        //Defines rectangular bounds where camera can move. Supports fixed padding on world borders.
        private void defineCameraBounds() {
            bounds.set(0, 0, worldSize, worldSize);
            bounds.extend(padding - camera.orthographicSize); // add padding to map to clearly show world's borders
            if (bounds.minX > bounds.maxX) {
                bounds.minX = (worldSize + padding) / 2;
                bounds.maxX = bounds.minX;
            }
            // float hiddenTiles = (1f - paneSize.height / paneSize.width) * camera.orthographicSize;
            // bounds.minY -= hiddenTiles;
            // bounds.maxY += hiddenTiles;
            // effectiveCameraSize.set(-camera.orthographicSize, hiddenTiles - camera.orthographicSize, camera.orthographicSize - 1, camera.orthographicSize - hiddenTiles - 1);
            // Debug.Log(effectiveCameraSize);
            // cameraFovRange.max = worldSize / 2 + padding + hiddenTiles;
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

        private void zoom(float delta) {
            if (delta == 0) return;
            float oldZoom = camera.orthographicSize;
            camera.orthographicSize = cameraFovRange.clamp(camera.orthographicSize + delta * 2);
            if (oldZoom != camera.orthographicSize) defineCameraBounds();
        }

        private void moveCamera(int x, float y, int z) {
            camera.transform.Translate(x, y, z, Space.World);
        }

        // moves camera towards pointer
        // private void checkPointer() {
        //     visibleArea.set(effectiveCameraSize).move(camera.transform.localPosition);
        //     speed = visibleArea.getDirectionVector(pointerController.targetPosition);
        // }
    }
}