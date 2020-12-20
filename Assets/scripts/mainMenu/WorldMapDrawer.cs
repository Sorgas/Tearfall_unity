using System;
using Assets.Scenes.MainMenu.script;
using UnityEngine;
using UnityEngine.Tilemaps;
using util.geometry;
using Random = System.Random;

public class WorldMapDrawer : MonoBehaviour {
    public Tilemap tilemap;
    public Grid grid;
    public Camera _camera;
    public RectTransform pane;
    public RectTransform mask;
    public TileBase[] tileBases;
    public ValueRange cameraFovRange = new ValueRange(4, 40);

    private Vector3 speed = new Vector3();
    private FloatBounds2 bounds = new FloatBounds2();
    private const float cameraSpeedChange = 0.1f;
    private const float cameraSpeedLimit = 2f;
    private int minimapPadding = 2;

    public int worldSize;

    private int tileSize = 32;

    public void drawWorld(WorldMap worldMap) {
        worldSize = worldMap.size;
        Vector3 bounds = new Vector3(worldSize * tileSize, worldSize * tileSize, 0);
        grid.GetComponent<RectTransform>().sizeDelta = new Vector2(bounds.x, bounds.y);
        // _camera.transform.Translate(new Vector3(bounds.x / 2, bounds.y /2, 0));
        Vector3Int cachePosition = new Vector3Int();
        defineCameraBounds();
        Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        random.InitState();
        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldSize; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, tileBases[random.NextInt(tileBases.Length - 1)]);
            }
        }
    }

    // handles minimap input
    private void Update() {
        if (Input.GetKey(KeyCode.UpArrow)) scrollMinimap(0, 1);
        if (Input.GetKey(KeyCode.DownArrow)) scrollMinimap(0, -1);
        if (Input.GetKey(KeyCode.LeftArrow)) scrollMinimap(-1, 0);
        if (Input.GetKey(KeyCode.RightArrow)) scrollMinimap(1, 0);
        zoomMinimap(Input.GetAxis("Mouse ScrollWheel"));
        ensureCameraBounds();
        updateCameraPosition();
    }

    private void scrollMinimap(int x, int y) {
        Vector3 position = _camera.transform.localPosition;
        if (x > 0 && position.x < bounds.maxX ||
            x < 0 && position.x > bounds.minX) speed.x += x * 0.3f;
        if (y > 0 && position.y < bounds.maxY ||
            y < 0 && position.y > bounds.minY) speed.y += y * 0.3f;
    }

    private void updateCameraPosition() {
        Vector3 position = _camera.transform.localPosition;
        if (speed.x != 0) {
            if (position.x - bounds.minX < -speed.x) speed.x = bounds.minX - position.x;
            if (bounds.maxX - position.x < speed.x) speed.x = bounds.maxX - position.x;
        }
        if (speed.y != 0) {
            if (position.y - bounds.minY < -speed.y) speed.y = bounds.minY - position.y;
            if (bounds.maxY - position.y < speed.y) speed.y = bounds.maxY - position.y;
        }
        _camera.transform.Translate(speed, Space.Self);
        speed.x *= 0.9f;
        speed.y *= 0.9f;
        if (Math.Round(speed.x) < 0.1f) speed.x = 0;
        if (Math.Round(speed.y) < 0.1f) speed.y = 0;
    }

    /**
     * Defines rectangular bounds where camera can move. Supports fixed padding on world borders.
     */
    private void defineCameraBounds() {
        bounds.set(0, 0, worldSize, worldSize);
        bounds.extend(minimapPadding - _camera.orthographicSize); // add padding to map to clearly show world's borders
        float hiddenTiles = (1f - mask.rect.height / pane.rect.width) * _camera.orthographicSize;
        Debug.Log(hiddenTiles);
        bounds.minY -= hiddenTiles;
        bounds.maxY += hiddenTiles;
    }

    private void ensureCameraBounds() {
        Vector3 position = _camera.transform.localPosition;
        Vector3 translation = new Vector3();
        if (!bounds.isIn(position)) { // scroll into bounds, if camera get out on scrolling
            if (position.x < bounds.minX) translation.x = bounds.minX - position.x;
            if (position.x > bounds.maxX) translation.x = bounds.maxX - position.x;
            if (position.y < bounds.minY) translation.y = bounds.minY - position.y;
            if (position.y > bounds.maxY) translation.y = bounds.maxY - position.y;
            _camera.transform.Translate(translation, Space.Self);
        }
    }
    
    private void zoomMinimap(float delta) {
        if (delta == 0) return;
        float oldZoom = _camera.orthographicSize;
        _camera.orthographicSize = cameraFovRange.clamp(_camera.orthographicSize + delta * 2);
        if (oldZoom != _camera.orthographicSize) defineCameraBounds();
    }
}