using System;
using game.model;
using game.model.localmap;
using UnityEngine;
using util.geometry;

// moves camera to be on same z-level with entity selector sprite and see it on screen
namespace game.view.no_es {
    // smoothly moves camera to camera target
    // keeps target within bounds for camera target
    // TODO add camera velocity
    public class CameraMovementSystem2 {
        private readonly Camera camera;
        private Vector3 target = new Vector3(0, 0, -1); // target in scene coordinates
        private readonly FloatBounds2 cameraBounds = new FloatBounds2(); // scene
        private readonly ValueRange cameraFovRange = new ValueRange(2, 20);
        private Vector3 cameraSpeed = new Vector3();
        private const int overlookTiles = 3;

        public CameraMovementSystem2(Camera camera) {
            this.camera = camera;
        }

        public void init() {
            updateCameraBounds();
        }

        public void update() {
            if (camera.transform.localPosition != target)
                camera.transform.localPosition =
                    Vector3.SmoothDamp(camera.transform.localPosition, target, ref cameraSpeed, 0.1f);
        }

        public void zoomCamera(float delta) {
            if (delta == 0) return;
            camera.orthographicSize = cameraFovRange.clamp(camera.orthographicSize + delta * 2);
            updateCameraBounds(); // visible area size changed, but still need to maintain 3 visible offmap tiles
            ensureCameraBounds();
        }

        public void moveCameraTarget(int dx, int dy) {
            setCameraTarget(target.x + dx, target.y + dy, target.z);
        }

        // dz in model units
        public void moveCameraTargetZ(int dz) {
            setCameraTarget(target.x, target.y + dz / 2f, target.z - dz * 2f);
            updateCameraBounds();
        }

        // sets camera target by value (scene)
        private void setCameraTarget(float x, float y, float z) {
            target.Set(x, y, z);
            updateCameraBounds();
            ensureCameraBounds();
        }

        // updates camera bounds to make 3 tiles around map visible
        private void updateCameraBounds() {
            LocalMap map = GameModel.get().localMap;
            cameraBounds.set(0, 0, map.xSize, map.ySize);
            cameraBounds.extendX((int)(overlookTiles - cameraWidth()));
            cameraBounds.extendY((int)(overlookTiles - camera.orthographicSize));
            cameraBounds.move(0, -target.z / 4f);
        }

        private void ensureCameraBounds() => target = cameraBounds.putInto(target);

        private float cameraWidth() => camera.orthographicSize * Screen.width / Screen.height;
    }
}