using System;
using UnityEngine;
using util.geometry;

// controls camera of worldmap.
public class ScrollableCameraController {
    public Camera camera;
    public Rect paneSize;
    private RectTransform imageTransform;
    public int worldSize;
    public Sprite pointer;

    private Vector2Int pointerPosition = new Vector2Int();

    public ValueRange cameraFovRange = new ValueRange(4, 40);
    private Vector3 speed = new Vector3();
    private FloatBounds2 bounds = new FloatBounds2();
    private int padding = 2;
    private Vector2 previousMousePosition = new Vector2();

    public ScrollableCameraController(Rect paneSize, RectTransform image, Camera camera, int worldSize, Transform pointer) {
        this.paneSize = paneSize;
        this.camera = camera;
        this.worldSize = worldSize;
        defineCameraBounds();
    }

    public void handleInput() {
        //if (Input.GetKey(KeyCode.UpArrow)) scrollMinimap(0, 1);
        //if (Input.GetKey(KeyCode.DownArrow)) scrollMinimap(0, -1);
        //if (Input.GetKey(KeyCode.LeftArrow)) scrollMinimap(-1, 0);
        //if (Input.GetKey(KeyCode.RightArrow)) scrollMinimap(1, 0);
        zoomMinimap(Input.GetAxis("Mouse ScrollWheel"));
        ensureCameraBounds();
        updateCameraPosition();
        if (!Input.mousePosition.Equals(previousMousePosition)) {
            updatePointer();
        }
    }

    private void scrollMinimap(int x, int y) {
        Vector3 position = camera.transform.localPosition;
        if (x > 0 && position.x < bounds.maxX ||
            x < 0 && position.x > bounds.minX) speed.x += x * 0.3f;
        if (y > 0 && position.y < bounds.maxY ||
            y < 0 && position.y > bounds.minY) speed.y += y * 0.3f;
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
        if (Math.Round(speed.x) < 0.1f) speed.x = 0;
        if (Math.Round(speed.y) < 0.1f) speed.y = 0;
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
        Debug.Log(hiddenTiles);
        bounds.minY -= hiddenTiles;
        bounds.maxY += hiddenTiles;
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

    private void updatePointer() {
        Vector2 position = Input.mousePosition;
        //imageTransform.TransformPoint

        // to image position == incamera position
        // to tilemap position
        // to tile
        // check tile new

    }
}