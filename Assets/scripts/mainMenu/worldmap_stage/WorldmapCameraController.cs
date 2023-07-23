using System;
using mainMenu.worldmap_stage;
using UnityEngine;
using util.geometry;
using util.geometry.bounds;

namespace mainMenu.worldmap {
public class WorldmapCameraController {
    public Camera camera;
    public int worldSize;

    private readonly FloatBounds2 cameraBounds = new(); // camera bounds
    private ValueRange cameraSizeRange = new(4, 40); // zoom
    private Vector3 speed;
    private const int padding = 2; // blank tiles on map sides
    private FloatBounds2 visibleArea = new();
    private PlayerControls playerControls;
    private Rect viewportSize;
    private float cameraMaxSpeed;
    private const float defaultCameraZoom = 20;
    private const float cameraRelativeMaxSpeed = 0.05f; // relative to camera size

    public WorldmapCameraController(WorldmapController worldmapController) {
        camera = worldmapController._camera;
        playerControls = worldmapController.playerControls;
        worldSize = worldmapController.worldMap.size;
        viewportSize = worldmapController.mask.rect;
        updateCameraBounds();
        camera.orthographicSize = defaultCameraZoom;
        setCameraMaxSpeed();
    }

    public void update() {
        zoomMinimap(Input.GetAxis("Mouse ScrollWheel"));
        updateCameraPosition();
        ensureCameraBounds();
    }

    public void setCameraToCenter() {
        camera.transform.localPosition = new Vector3(worldSize / 2f, worldSize / 2f, -2);
    }

    private void updateCameraPosition() {
        Vector2 vector = playerControls.Player.CameraMove.ReadValue<Vector2>();
        speed.x += vector.x;
        speed.y += vector.y;
        float speedValue = speed.magnitude;
        if (speedValue > cameraMaxSpeed) {
            speed *= cameraMaxSpeed / speedValue;
        }
        camera.transform.Translate(speed, Space.Self);
        speed.x *= 0.5f;
        speed.y *= 0.5f;
        if (Math.Abs(speed.x) < 0.1f) speed.x = 0;
        if (Math.Abs(speed.y) < 0.1f) speed.y = 0;
    }

    //Defines rectangular bounds where camera can move. Supports fixed padding on world borders.
    private void updateCameraBounds() {
        cameraBounds.set(0, 0, worldSize, worldSize);
        float cameraH = camera.orthographicSize;
        float cameraW = camera.aspect * cameraH;
        // add padding to map to clearly show world's borders
        cameraBounds.extendY(padding - cameraH);
        cameraBounds.extendX(padding - cameraW);
        if (cameraBounds.minX > cameraBounds.maxX) {
            cameraBounds.minX = (worldSize + padding) / 2f;
            cameraBounds.maxX = cameraBounds.minX;
        }
        if (cameraBounds.minY > cameraBounds.maxY) {
            cameraBounds.minY = (worldSize + padding) / 2f;
            cameraBounds.maxY = cameraBounds.minY;
        }

        float hiddenTiles = (1f - viewportSize.height / viewportSize.width) * cameraH;
        cameraBounds.minY -= hiddenTiles;
        cameraBounds.maxY += hiddenTiles;
    }

    private void ensureCameraBounds() {
        Vector3 position = camera.transform.localPosition;
        Vector3 translation = new Vector3();
        if (!cameraBounds.isIn(position)) { // scroll into bounds, if camera get out on scrolling
            if (position.x < cameraBounds.minX) translation.x = cameraBounds.minX - position.x;
            if (position.x > cameraBounds.maxX) translation.x = cameraBounds.maxX - position.x;
            if (position.y < cameraBounds.minY) translation.y = cameraBounds.minY - position.y;
            if (position.y > cameraBounds.maxY) translation.y = cameraBounds.maxY - position.y;
            camera.transform.Translate(translation, Space.Self);
        }
    }

    private void zoomMinimap(float delta) {
        if (delta == 0) return;
        float oldZoom = camera.orthographicSize;
        camera.orthographicSize = cameraSizeRange.clamp(oldZoom + delta * 2);
        setCameraMaxSpeed();
        if (Math.Abs(oldZoom - camera.orthographicSize) > 0.01f) updateCameraBounds();
    }

    private void setCameraMaxSpeed() {
        cameraMaxSpeed = camera.orthographicSize * cameraRelativeMaxSpeed;
    }
}
}