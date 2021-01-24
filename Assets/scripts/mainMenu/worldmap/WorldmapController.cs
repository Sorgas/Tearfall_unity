using Assets.Scenes.MainMenu.script;
using UnityEngine;
using UnityEngine.Tilemaps;
using util;

public class WorldmapController : MonoBehaviour {
    public RectTransform mask;
    public RectTransform image;
    public TileBase[] tileBases;

    private Transform pointer;
    private Tilemap tilemap;
    private Grid grid;
    private Camera _camera;

    public int worldSize;
    private int tileSize = 32;
    private ScrollableCameraController controller;
    private WorldmapPointerController pointerController;

    public void Start() {
        tilemap = gameObject.transform.GetComponentInChildren<Tilemap>();
        grid = gameObject.GetComponent<Grid>();
        _camera = gameObject.transform.GetComponentInChildren<Camera>();
        pointer = gameObject.transform.GetComponentInChildren<SpriteRenderer>().transform;
    }

    public void drawWorld(WorldMap worldMap) {
        worldSize = worldMap.size;
        Vector3 bounds = new Vector3(worldSize * tileSize, worldSize * tileSize, 0);
        grid.GetComponent<RectTransform>().sizeDelta = new Vector2(bounds.x, bounds.y);
        
        controller = new ScrollableCameraController(mask.rect, image, _camera, worldSize, pointer);
        pointerController = new WorldmapPointerController(worldSize, pointer);
        Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        random.InitState();
        Vector3Int cachePosition = new Vector3Int();
        for (int x = 0; x <= worldSize; x++) {
            for (int y = 0; y <= worldSize; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, tileBases[random.NextInt(tileBases.Length - 1)]);
            }
        }
    }

    public void handleClick() {

    }

    public void clear() {
        tilemap.ClearAllTiles();
    }

    private void Update() {
        if(controller != null) controller.handleInput();
        if (pointerController != null) pointerController.update();
    }
}