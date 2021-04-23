using Assets.scripts.util.geometry;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.view {
    // controls camera and selector movement on local map
    // TODO document controls
    public class LocalMapCameraController {
        // common
        public Camera camera;
        public RectTransform selector;
        public int mapSize;
        public int mapLayers;
        public RectTransform mapHolder;
        private IntBounds3 bounds = new IntBounds3(); // bounds for logical target
        public bool enabled = true;

        // camera
        private Vector3 cameraSpeed = new Vector3();
        private ValueRange cameraFovRange = new ValueRange(4, 40);
        private FloatBounds2 visibleArea = new FloatBounds2(); // visible area around !cameraTarget!
        private Vector3 cameraTarget = new Vector3(0, 0, -1);

        // selector
        private IntVector3 selectorPosition = new IntVector3();
        private Vector3 selectorSpeed = new Vector3();
        private Vector3 selectorTarget = new Vector3(0, 0, -1);

        public LocalMapCameraController(Camera camera, RectTransform selector, RectTransform mapHolder, int mapSize, int mapLayers) {
            this.camera = camera;
            this.selector = selector;
            this.mapHolder = mapHolder;
            this.mapSize = mapSize;
            this.mapLayers = mapLayers;
            bounds.set(0, 0, 0, mapSize, mapSize, mapLayers);
        }

        public void update() {
            if (!enabled) return;
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) handleMouseMovement(); // update selector position if mouse moved
            // update and move selector
            selectorTarget.Set(selectorPosition.x, selectorPosition.y + selectorPosition.z / 2f, -2 * selectorPosition.z - 0.1f); // update target by in-model position
            selector.localPosition = Vector3.SmoothDamp(selector.localPosition, selectorTarget, ref selectorSpeed, 0.05f); // move selector

            if (camera.transform.localPosition.z != selector.localPosition.z - 0.9f) { // keep camera on same level as selector
                camera.transform.Translate(0,0, selector.localPosition.z - 0.9f - camera.transform.localPosition.z);
            }
            updateVisibleArea();
            if (!visibleArea.isIn(selector.localPosition)) {
                Vector2 vector = visibleArea.getDirectionVector(selector.localPosition);
                cameraTarget.x += vector.x;
                cameraTarget.y += vector.y;
            }
            if (camera.transform.localPosition != cameraTarget) camera.transform.localPosition = Vector3.SmoothDamp(camera.transform.localPosition, cameraTarget, ref cameraSpeed, 0.2f);
        }

        private void updateVisibleArea() {
            float cameraWidth = camera.orthographicSize * Screen.width / Screen.height;
            visibleArea.set((int)(cameraTarget.x - cameraWidth + 1),
                (int)(cameraTarget.y - camera.orthographicSize + 1),
                (int)(cameraTarget.x + cameraWidth - 1),
                (int)(cameraTarget.y + camera.orthographicSize - 1));
        }

        // moves model position of selector and visual movement target. takes parameters in model coordinates
        public void move(int dx, int dy, int dz) {
            if (!enabled) return;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { // adjust delta for faster scrolling
                dx *= 10;
                dy *= 10;
            }
            // TODO replace with in-model selector
            selectorPosition.add(dx, dy, dz); // update model position of selector
            ensureSelectorBounds();
        }

        // reset selector to mouse position on current level
        private void handleMouseMovement() {
            Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mapHolderPosition = mapHolder.InverseTransformPoint(worldPosition);
            selectorPosition.set((int)mapHolderPosition.x, -selectorPosition.z / 2f + mapHolderPosition.y, selectorPosition.z); // update model position of selector
            ensureSelectorBounds();
        }

        private void ensureSelectorBounds() {
            if (!bounds.isIn(selectorPosition)) selectorPosition.add(bounds.getInVector(selectorPosition)); // return selector into map, if needed
        }

        public void zoomCamera(float delta) {
            if (delta == 0) return;
            float oldZoom = camera.orthographicSize;
            camera.orthographicSize = cameraFovRange.clamp(camera.orthographicSize + delta * 2);
        }
    }
}