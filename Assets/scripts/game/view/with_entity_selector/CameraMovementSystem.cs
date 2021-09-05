using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.geometry;
using UnityEngine;

// moves camera to be on same z-level with entity selector sprite and see it on screen
public class CameraMovementSystem {
    public Camera camera;
    public RectTransform selectorSprite;
    private Vector3 cameraTarget = new Vector3(0, 0, -1); // target in scene coordinates
    private ValueRange cameraFovRange = new ValueRange(3, 20);
    private FloatBounds2 visibleArea = new FloatBounds2(); // scene
    private FloatBounds2 cameraBounds = new FloatBounds2(); // scene
    private Vector3 cameraSpeed = new Vector3();
    private readonly int overlookTiles = 3;
    private Vector3Int cacheVector = new Vector3Int();

    public CameraMovementSystem(Camera camera, RectTransform selectorSprite) {
        this.camera = camera;
        this.selectorSprite = selectorSprite;
    }

    public void update() {
        // move target to selector layer
        if (cameraTarget.z != selectorSprite.localPosition.z - 0.4f) moveCameraTargetZ(selectorSprite.localPosition.z - 0.4f - cameraTarget.z);
        // move camera target to see selector
        if (!visibleArea.isIn(selectorSprite.localPosition) || GameView.get().selectorOverlook.magnitude != 0) 
            moveCameraTarget(visibleArea.getDirectionVector(selectorSprite.localPosition) + GameView.get().selectorOverlook * overlookTiles);
        movecamera();
    }

    public void zoomCamera(float delta) {
        if (delta == 0) return;
        camera.orthographicSize = cameraFovRange.clamp(camera.orthographicSize + delta * 2);
        updateCameraBounds(); // visible area size changed, but still need to maintain 3 visible offmap tiles
        updateVisibleArea(); // visible area size changed
    }

    public void moveCameraTarget(Vector2 delta) {
        setCameraTarget(cameraTarget.x + delta.x, cameraTarget.y + delta.y, cameraTarget.z);
    }

    public void moveCameraTargetZ(float dz) {
        setCameraTarget(cameraTarget.x, cameraTarget.y - dz / 4f, cameraTarget.z + dz);
        updateCameraBounds();
    }

    // sets camera target by value (scene)
    private void setCameraTarget(float x, float y, float z) {
        cameraTarget.Set(x, y, z);
        cameraTarget = cameraBounds.putInto(cameraTarget);
        updateVisibleArea(); // camera target changed
    }

    // updates logical bounds of visible tiles
    private void updateVisibleArea() {
        float cameraWidth = this.cameraWidth();
        visibleArea.set((int)(cameraTarget.x - cameraWidth + 1),
            (int)(cameraTarget.y - camera.orthographicSize + 1),
            (int)(cameraTarget.x + cameraWidth - 1),
            (int)(cameraTarget.y + camera.orthographicSize - 1));
    }

    // udates camera bounds to make 3 tiles around map visible
    private void updateCameraBounds() {
        LocalMap map = GameModel.get().localMap;
        cameraBounds.set(0, 0, map.xSize, map.ySize);
        cameraBounds.extendX((int)(overlookTiles - cameraWidth()));
        cameraBounds.extendY((int)(overlookTiles - camera.orthographicSize));
        cameraBounds.move(0, GameModel.get().selector.position.z / 2f);
    }

    private void movecamera() {
        if (camera.transform.localPosition != cameraTarget)
            camera.transform.localPosition = Vector3.SmoothDamp(camera.transform.localPosition, cameraTarget, ref cameraSpeed, 0.05f);
    }

    private float cameraWidth() {
        return camera.orthographicSize * Screen.width / Screen.height;
    }
}