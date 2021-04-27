using Assets.scripts.util.geometry;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.view {
    // controls camera and selector movement on local map
    // TODO document controls
    public class LocalMapCameraController {
        // common
        public Camera camera;
        public RectTransform selector;
        public RectTransform mapHolder;
        public bool enabled = true;
        private IntBounds2 screenBounds = new IntBounds2(Screen.width, Screen.height);
        private int mapSize;

        // camera
        private Vector3 cameraSpeed = new Vector3();
        private ValueRange cameraFovRange = new ValueRange(4, 40);
        private FloatBounds2 visibleArea = new FloatBounds2(); // visible area around !cameraTarget!
        private Vector3 cameraTarget = new Vector3(0, 0, -1);
        private IntBounds2 cameraBounds = new IntBounds2(); // bounds for camera target

        // selector
        private IntVector3 selectorPosition = new IntVector3(); // TODO replace to in-model position
        private Vector3 selectorTarget = new Vector3(0, 0, -1); // target in scene coordinates
        private Vector3 selectorSpeed = new Vector3();
        private ValueRange zRange = new ValueRange(); // inclusive range for selector z
        private IntBounds2 bounds = new IntBounds2(); // inclusive bounds for selector xy

        public LocalMapCameraController(Camera camera, RectTransform selector, RectTransform mapHolder, int mapSize, int mapLayers) {
            this.camera = camera;
            this.selector = selector;
            this.mapHolder = mapHolder;
            this.mapSize = mapSize;
            bounds.set(0, 0, mapSize - 1, mapSize - 1);
            zRange.set(0, mapLayers - 1);
            updateCameraBounds();
        }

        public void update() {
            if (!enabled) return;
            if (screenBounds.isIn(Input.mousePosition)) { // mouse inside screen
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) handleMouseMovement(); // update selector position if mouse moved
            }
            // update and move selector
            selectorTarget.Set(selectorPosition.x, selectorPosition.y + selectorPosition.z / 2f, -2 * selectorPosition.z - 0.1f); // update target by in-model position
            if (selector.localPosition != selectorTarget)
                selector.localPosition = Vector3.SmoothDamp(selector.localPosition, selectorTarget, ref selectorSpeed, 0.05f); // move selector

            updateVisibleArea();
            if (!visibleArea.isIn(selector.localPosition)) { // move camera to see selector
                Vector2 vector = visibleArea.getDirectionVector(selector.localPosition);
                cameraTarget.x += vector.x;
                cameraTarget.y += vector.y;
            }
            if (camera.transform.localPosition != cameraTarget)
                camera.transform.localPosition = Vector3.SmoothDamp(camera.transform.localPosition, cameraTarget, ref cameraSpeed, 0.05f); // move camera
        }

        private void updateVisibleArea() {
            float cameraWidth = camera.orthographicSize * Screen.width / Screen.height;
            visibleArea.set((int)(cameraTarget.x - cameraWidth + 1),
                (int)(cameraTarget.y - camera.orthographicSize + 1),
                (int)(cameraTarget.x + cameraWidth - 1),
                (int)(cameraTarget.y + camera.orthographicSize - 1));
            visibleArea.extend(-1);
        }

        // moves model position of selector and visual movement target. takes parameters in model coordinates
        public void move(int dx, int dy, int dz) {
            if (!enabled) return;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { // adjust delta for faster scrolling
                dx *= 10;
                dy *= 10;
            }
            // TODO replace with in-model selector
            if (dz != 0) handleZChange(dz);
            selectorPosition.add(dx, dy, 0); // update model position of selector
            ensureSelectorBounds();
        }

        private void handleZChange(int dz) {
            if (!zRange.check(selectorPosition.z + dz)) return; // new position out of range
            selectorPosition.add(0, 0, dz); // move selector
            cameraTarget.Set(cameraTarget.x, cameraTarget.y + dz / 2f, cameraTarget.z - dz * 2f); // move camera
        }

        // reset selector to mouse position on current level
        private void handleMouseMovement() {
            Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mapHolderPosition = mapHolder.InverseTransformPoint(worldPosition);
            selectorPosition.set((int)mapHolderPosition.x, -selectorPosition.z / 2f + mapHolderPosition.y, selectorPosition.z); // update model position of selector
            ensureSelectorBounds();
        }

        private void ensureSelectorBounds() {
            bounds.putInto(selectorPosition); // return selector into map, if needed
        }

        public void zoomCamera(float delta) {
            if (delta == 0) return;
            camera.orthographicSize = cameraFovRange.clamp(camera.orthographicSize + delta * 2);
            updateCameraBounds();
        }

        public void moveCamera(int dx, int dy) {
            cameraTarget.x += dx;
            cameraTarget.y += dy;
            updateVisibleArea();
            selectorPosition.x += dx;
            selectorPosition.y += dy;
            ensureSelectorBounds();
        }

        private void updateCameraBounds() {
            float cameraWidth = camera.orthographicSize * Screen.width / Screen.height;
            cameraBounds.set(0, 0, mapSize - 1, mapSize - 1);
            cameraBounds.extendX((int)(3 - cameraWidth));
            cameraBounds.extendY((int)(3 - camera.orthographicSize));
        }
    }
}