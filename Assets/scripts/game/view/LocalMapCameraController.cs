using Assets.scripts.game.model;
using Assets.scripts.util.geometry;
using Tearfall_unity.Assets.scripts.game.model.entity_selector;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.view {
    // controls camera and selector movement on local map. Moves selector in game model, selector sprite and camara on the scene.
    // TODO update visible area for rectangular selector
    public class LocalMapCameraController {
        // common
        public Camera camera;
        public RectTransform selectorSprite;
        public RectTransform mapHolder;
        public bool enabled = true;
        // camera
        private Vector3 cameraSpeed = new Vector3();
        private ValueRange cameraFovRange = new ValueRange(4, 40);
        private FloatBounds2 visibleArea = new FloatBounds2(); // visible area around !cameraTarget!
        private Vector3 cameraTarget = new Vector3(0, 0, -1);
        private IntBounds2 cameraBounds = new IntBounds2(); // bounds for camera target
        // selector
        private EntitySelector selector = GameModel.get().selector;
        private EntitySelectorSystem system = GameModel.get().selectorSystem;
        private Vector3 selectorTarget = new Vector3(0, 0, -1); // target in scene coordinates
        private Vector3 selectorSpeed = new Vector3();

        public LocalMapCameraController(Camera camera, RectTransform selectorSprite, RectTransform mapHolder) {
            this.camera = camera;
            this.selectorSprite = selectorSprite;
            this.mapHolder = mapHolder;
            updateCameraBounds();
        }

        public void update() {
            if (!enabled) return;
            // Debug.Log("update "+ selector.position.x);
            selectorTarget.Set(selector.position.x, selector.position.y + selector.position.z / 2f, -2 * selector.position.z - 0.1f); // update target by in-model position
            if (selectorSprite.localPosition != selectorTarget)
                selectorSprite.localPosition = Vector3.SmoothDamp(selectorSprite.localPosition, selectorTarget, ref selectorSpeed, 0.05f); // move selector

            updateVisibleArea();
            if (!visibleArea.isIn(selectorSprite.localPosition)) { // move camera to see selector
                Vector2 vector = visibleArea.getDirectionVector(selectorSprite.localPosition);
                cameraTarget.x += vector.x;
                cameraTarget.y += vector.y;
            }
            if (camera.transform.localPosition != cameraTarget)
                camera.transform.localPosition = Vector3.SmoothDamp(camera.transform.localPosition, cameraTarget, ref cameraSpeed, 0.05f); // move camera
        }

        // moves model position of selector and visual movement target. takes parameters in model coordinates
        public void moveSelector(int dx, int dy, int dz) {
            if (!enabled) return;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { // adjust delta for faster scrolling
                dx *= 10;
                dy *= 10;
            }
            int prevZ = selector.position.z;
            system.moveSelector(dx, dy, dz); // update model position of selector
            if (selector.position.z != prevZ) cameraTarget.Set(cameraTarget.x, cameraTarget.y + dz / 2f, cameraTarget.z - dz * 2f); // move camera to other z-level
        }

        public void resetSelectorToMousePosition() {
            Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mapHolderPosition = mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
            int zLayer = selector.position.z; // z-layer cannot be changed by moving mouse
            system.setSelectorPosition((int)mapHolderPosition.x, (int)(-zLayer / 2f + mapHolderPosition.y), zLayer);
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
            system.moveSelector(dx, dy, 0);
        }

        public void setCameraPosition(Vector3Int position) {
            Vector3 scenePosition = new Vector3(position.x, position.y - position.z / 2f, position.z * -2f - 1);
            camera.transform.Translate(scenePosition - camera.transform.localPosition, Space.Self);
            cameraTarget.Set(scenePosition.x, scenePosition.y, scenePosition.z);
        }

        private void updateVisibleArea() {
            float cameraWidth = camera.orthographicSize * Screen.width / Screen.height;
            visibleArea.set((int)(cameraTarget.x - cameraWidth + 1),
                (int)(cameraTarget.y - camera.orthographicSize + 1),
                (int)(cameraTarget.x + cameraWidth - 1),
                (int)(cameraTarget.y + camera.orthographicSize - 1));
            visibleArea.extend(-1);
        }

        private void updateCameraBounds() {
            float cameraWidth = camera.orthographicSize * Screen.width / Screen.height;
            int mapSize = GameModel.get().localMap.xSize;
            cameraBounds.set(0, 0, mapSize - 1, mapSize - 1);
            cameraBounds.extendX((int)(3 - cameraWidth));
            cameraBounds.extendY((int)(3 - camera.orthographicSize));
        }
    }
}