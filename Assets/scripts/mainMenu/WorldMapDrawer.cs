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

    private Vector3 cacheVector = new Vector3();
    private Vector3 cameraSpeed = new Vector3();
    private const float cameraSpeedLimit = 2f;
    private const float cameraSpeedChange = 0.1f;
    private Int2dBounds cameraBounds = new Int2dBounds();
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
        updateCameraPosition();
    }

    private void scrollMinimap(int x, int y) {
        Vector3 position = _camera.transform.localPosition;
        if(x > 0 && position.x < cameraBounds.maxX ||
           x < 0 && position.x > cameraBounds.minX) cameraSpeed.x += x * 0.3f;
        if(y > 0 && position.y < cameraBounds.maxY ||
           y < 0 && position.y > cameraBounds.minY) cameraSpeed.y += y * 0.3f;
    }

    private void updateCameraPosition() {
        Vector3 position = _camera.transform.localPosition;
        if (!cameraBounds.isIn(position)) { // scroll into bounds, if camera get out on scrolling
            if (position.x < cameraBounds.minX) scrollMinimap(1, 0);
            if (position.x > cameraBounds.maxX) scrollMinimap(-1, 0);
            if (position.y < cameraBounds.minY) scrollMinimap(0, 1);
            if (position.y > cameraBounds.maxY) scrollMinimap(0, -1);
        }
        _camera.transform.Translate(cameraSpeed, Space.Self);
        cameraSpeed.x *= 0.9f;
        cameraSpeed.y *= 0.9f;
        if (Math.Round(cameraSpeed.x) < 0.1f) cameraSpeed.x = 0;
        if (Math.Round(cameraSpeed.y) < 0.1f) cameraSpeed.y = 0;
    }

    /**
     * Defines rectangular bounds where camera can move. Supports fixed padding on world borders.
     */
    private void defineCameraBounds() {
        cameraBounds.set(0, 0, worldSize, worldSize);
        cameraBounds.extend(
            (int) Math.Round(minimapPadding -
                             _camera.orthographicSize)); // add padding to map to clearly show world's borders
        int hiddenTiles = (int) Math.Round((1f - mask.rect.height / pane.rect.width) * _camera.orthographicSize);
        cameraBounds.minY -= hiddenTiles;
        cameraBounds.maxY += hiddenTiles;
    }

    private void zoomMinimap(float delta) {
        float newZoom = cameraFovRange.clamp(_camera.orthographicSize + delta);
        Debug.Log(newZoom);
        if (newZoom != _camera.orthographicSize) {
            _camera.orthographicSize = newZoom;
            defineCameraBounds();
        }
    }
}