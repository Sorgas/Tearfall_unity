using game.model;
using game.model.localmap;
using game.view.util;
using UnityEngine;
using util.geometry;
using util.geometry.bounds;

namespace game.view.camera {
    // smoothly moves camera to camera target
    // keeps target within bounds for camera target
    // TODO change camera speed on zoom.
    public class CameraMovementSystem {
        private const int OVERLOOK_TILES = 3; // how many space is visible outside map
        private Camera camera;
        private Vector3 target = new(0, 0, -1); // target in scene coordinates
        private readonly FloatBounds2 cameraBounds = new(); // scene bounds in 1 z-level
        private readonly ValueRange cameraZoomRange = new(2, 20);
        private float cameraTargetChangeMod = 0.5f; // 
        // private float maxCameraSpeed;
        // private Vector3 cameraSpeed;

        public void init() {
            camera = GameView.get().sceneElementsReferences.mainCamera;
            camera.farClipPlane = GlobalSettings.cameraLayerDepth * 2 + 1;
            updateCameraBounds();
            updateCameraSpeed();
        }

        public void update() {
            if (camera.transform.localPosition == target) return;
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, target, 0.5f);
            // camera.transform.localPosition = Vector3.SmoothDamp(camera.transform.localPosition, target, ref cameraSpeed, 0.08f);
        }

        public void zoomCamera(float delta) {
            if (delta == 0) return;
            camera.orthographicSize = cameraZoomRange.clamp(camera.orthographicSize + delta * 2);
            updateCameraSpeed();
            updateCameraBounds(); // visible area size changed, but still need to maintain 3 visible offmap tiles
            ensureCameraBounds();
        }

        // called from CIS
        public void moveCameraTarget(int dx, int dy) {
            setCameraTarget(target.x + dx * cameraTargetChangeMod, target.y + dy * cameraTargetChangeMod, target.z);
        }

        // called from CIS
        public void moveCameraTargetZ(int dz) {
            dz = GameView.get().selector.changeLayer(dz);
            if (dz == 0) return;
            setCameraTarget(target.x, target.y + dz / 2f, target.z - dz * 2f);
            updateCameraBounds();
        }

        // update camera target without updating position in model (called from GV)
        public void setTargetModel(Vector3Int modelPosition) {
            Vector3 scenePosition = ViewUtil.fromModelToScene(modelPosition);
            setCameraTarget(scenePosition.x, scenePosition.y, scenePosition.z - 1);
            updateCameraBounds();
            camera.transform.localPosition = target;
        }

        // camera speed depends on zoom level
        private void updateCameraSpeed() => cameraTargetChangeMod = camera.orthographicSize * 0.04f;

        // sets camera target by value (scene)
        private void setCameraTarget(float x, float y, float z) {
            target.Set(x, y, z);
            ensureCameraBounds();
        }

        // updates camera bounds to make 3 tiles around map visible
        private void updateCameraBounds() {
            LocalMap map = GameModel.get().currentLocalModel.localMap;
            float cameraWidth = camera.orthographicSize * Screen.width / Screen.height;
            cameraBounds.set(1, 1, map.bounds.maxX, map.bounds.maxY);
            cameraBounds.extendX((int)(OVERLOOK_TILES - cameraWidth));
            cameraBounds.extendY((int)(OVERLOOK_TILES - camera.orthographicSize));
            cameraBounds.move(0, -target.z / 4f);
        }

        // puts camera into bounds
        private void ensureCameraBounds() => target = cameraBounds.putInto(target);
    }
}