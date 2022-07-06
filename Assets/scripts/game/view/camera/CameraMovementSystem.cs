using game.model;
using game.model.localmap;
using game.view.util;
using UnityEngine;
using util.geometry;

namespace game.view.camera {
    // smoothly moves camera to camera target
    // keeps target within bounds for camera target
    // TODO add camera velocity
    public class CameraMovementSystem {
        private Camera camera;
        private Vector3 target = new(0, 0, -1); // target in scene coordinates
        private readonly FloatBounds2 cameraBounds = new(); // scene bounds in 1 z-level
        private readonly ValueRange cameraZoomRange = new(2, 20);
        private Vector3 cameraSpeed = new();
        private const int overlookTiles = 3;

        public void init() {
            camera = GameView.get().sceneObjectsContainer.mainCamera;
            updateCameraBounds();
        }

        public void update() {
            if (camera.transform.localPosition == target) return;
            camera.transform.localPosition = Vector3.SmoothDamp(camera.transform.localPosition, target, ref cameraSpeed, 0.08f);
        }

        public void zoomCamera(float delta) {
            if (delta == 0) return;
            camera.orthographicSize = cameraZoomRange.clamp(camera.orthographicSize + delta * 2);
            updateCameraBounds(); // visible area size changed, but still need to maintain 3 visible offmap tiles
            ensureCameraBounds();
        }
        
        public void moveCameraTarget(int dx, int dy) {
            setCameraTarget(target.x + dx, target.y + dy, target.z);
        }

        // dz in model units
        public void moveCameraTargetZ(int dz) {
            dz = GameView.get().changeLayer(dz);
            if (dz == 0) return;
            setCameraTarget(target.x, target.y + dz / 2f, target.z - dz * 2f);
            updateCameraBounds();
        }

        // update camera target without updating position in model (called from GV)
        public void setTargetModel(Vector3Int modelPosition) {
            Vector3 scenePosition = ViewUtil.fromModelToScene(modelPosition);
            setCameraTarget(scenePosition.x, scenePosition.y, scenePosition.z -1);
        }
        
        // sets camera target by value (scene)
        private void setCameraTarget(float x, float y, float z) {
            target.Set(x, y, z);
            updateCameraBounds();
            ensureCameraBounds();
        }

        // updates camera bounds to make 3 tiles around map visible
        private void updateCameraBounds() {
            LocalMap map = GameModel.localMap;
            cameraBounds.set(0, 0, map.bounds.maxX, map.bounds.maxY);
            cameraBounds.extendX((int)(overlookTiles - cameraWidth()));
            cameraBounds.extendY((int)(overlookTiles - camera.orthographicSize));
            cameraBounds.move(0, -target.z / 4f);
        }

        private void ensureCameraBounds() => target = cameraBounds.putInto(target);

        private float cameraWidth() => camera.orthographicSize * Screen.width / Screen.height;
    }
}