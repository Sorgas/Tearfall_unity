using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.geometry;
using Tearfall_unity.Assets.scripts.game.model.entity_selector;
using UnityEngine;

// moves camera to be on same z-level with entity selector and see ES on screen
public class CameraMovementSystem {
    // common
    public Camera camera;
    public RectTransform selectorSprite;
    // camera
    private Vector3 cameraTarget = new Vector3(0, 0, -1); // target in scene coordinates
    private ValueRange cameraFovRange = new ValueRange(4, 15);
    private FloatBounds2 visibleArea = new FloatBounds2(); // visible tiles area around !cameraTarget!
    private FloatBounds2 cameraBounds = new FloatBounds2(); // bounds for camera target
    private Vector3 cameraSpeed = new Vector3();

    Vector3Int cacheVector = new Vector3Int();

    public CameraMovementSystem(Camera camera, RectTransform selectorSprite) {
        this.camera = camera;
        this.selectorSprite = selectorSprite;
        updateCameraBounds();
        updateVisibleArea();
    }

    public void update() {
        // move camera target to see selector
        if (!visibleArea.isIn(selectorSprite.localPosition)) {
            Vector2 vector = visibleArea.getDirectionVector(selectorSprite.localPosition);
            moveCameraTarget(vector.x, vector.y, 0);
        }
        // move camera smoothly
        if (camera.transform.localPosition != cameraTarget)
            camera.transform.localPosition = Vector3.SmoothDamp(camera.transform.localPosition, cameraTarget, ref cameraSpeed, 0.05f);
    }

    public void zoomCamera(float delta) {
        if (delta == 0) return;
        camera.orthographicSize = cameraFovRange.clamp(camera.orthographicSize + delta * 2);
        updateCameraBounds(); // visible area size changed, but still need to maintain 3 visible offmap tiles
        updateVisibleArea(); // visible area size changed
    }

    // sets camera to model position
    public void setCameraPosition(Vector3Int position) {
        Debug.Log("setting camera position to " + position);
        Vector3 scenePosition = new Vector3(position.x, position.y - position.z / 2f, position.z * -2f - 1);
        camera.transform.Translate(scenePosition - camera.transform.localPosition, Space.Self);
        setCameraTarget(scenePosition.x, scenePosition.y, scenePosition.z);
    }

    // changes camera target by value
    public void moveCameraTarget(float dx, float dy, float dz) {
        setCameraTarget(cameraTarget.x + dx, cameraTarget.y + dy + dz / 2f, cameraTarget.z -dz * 2);
    }

    // sets camera target by value
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
        cameraBounds.extendX((int)(3 - cameraWidth()));
        cameraBounds.extendY((int)(3 - camera.orthographicSize));
        cameraBounds.move(0, GameModel.get().selector.position.z / 2f);
    }

    private float cameraWidth() {
        return camera.orthographicSize * Screen.width / Screen.height;
    }
}