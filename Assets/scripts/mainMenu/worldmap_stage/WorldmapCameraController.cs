using System;
using game.model;
using UnityEngine;
using util.geometry;
using util.geometry.bounds;

namespace mainMenu.worldmap_stage {
// Moves and zooms camera of world map.
public class WorldmapCameraController : MonoBehaviour {
    public new Camera camera; // camera to move
    public RectTransform mask; // viewport object on screen 

    private int worldSize;
    private PlayerControls playerControls;

    private readonly FloatBounds2 cameraBounds = new(); // camera bounds
    private readonly ValueRange cameraSizeRange = new(4, 40); // zoom
    private Vector3 speed;
    private const int padding = 2; // blank tiles on map sides
    private Rect viewportSize;
    private float cameraMaxSpeed;

    public void Start() {
        playerControls = new PlayerControls();
        playerControls.Enable();
        viewportSize = mask.rect;
    }

    public void Update() {
        zoomMinimap(playerControls.UI.ScrollWheel.ReadValue<Vector2>().y);
        updateCameraPosition();       
        camera.transform.localPosition = cameraBounds.putInto(camera.transform.localPosition);
    }

    // nullable
    public void setWorldMap(WorldMap worldMap) {
        worldSize = worldMap != null ? worldMap.size : 0;
        resetCamera();
        updateCameraBounds();
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
        if (Math.Abs(speed.x) < 0.05f) speed.x = 0;
        if (Math.Abs(speed.y) < 0.05f) speed.y = 0;
    }

    // Defines rectangular bounds where camera can move. Supports fixed padding on world borders.
    private void updateCameraBounds() {
        if (worldSize == 0) return;
        cameraBounds.set(0, 0, worldSize, worldSize);
        float cameraWidth = camera.orthographicSize;
        cameraBounds.extendX(padding - cameraWidth);
        cameraBounds.extendY(padding - cameraWidth * viewportSize.height / viewportSize.width);
    }

    private void zoomMinimap(float delta) {
        if (delta == 0) return;
        float oldZoom = camera.orthographicSize;
        setCameraSize(cameraSizeRange.clamp(oldZoom + delta / 120));
        if (Math.Abs(oldZoom - camera.orthographicSize) > 0.01f) updateCameraBounds();
    }

    private void resetCamera() {
        camera.transform.localPosition = new Vector3(worldSize / 2f, worldSize / 2f, -2);
        float targetSize = (worldSize / 2f + padding) / viewportSize.height * viewportSize.width;
        cameraSizeRange.set(Math.Min(4, worldSize / 2f), targetSize);
        setCameraSize(targetSize);
    }

    // maxSpeed should always be updated
    private void setCameraSize(float value) {
        camera.orthographicSize = value;
        cameraMaxSpeed = camera.orthographicSize * 0.05f;
    }
}
}