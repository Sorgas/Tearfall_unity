using game.model;
using mainMenu.worldmap;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace mainMenu.worldmap_stage {
// reads mouse movement and clicks for world map. updates selectors positions.
public class WorldmapController : MonoBehaviour {
    // external objects
    public RectTransform mask;
    public Button mapPanel;
    public TileBase[] tileBases;

    // prefab components
    public Transform pointer;
    public Transform locationSelector;
    public Tilemap tilemap;
    public Camera _camera;

    public bool pointerMoved;
    public bool locationChanged;

    private int tileSize = 32;
    private WorldmapCameraController cameraController;
    private WorldmapPointerController pointerController;
    public WorldMap worldMap;
    public PlayerControls playerControls;

    public void Start() {
        playerControls = new();
        playerControls.Enable();
    }

    private void Update() {
        if (cameraController != null) cameraController.update();
        if (pointerController != null) pointerController.update();
    }

    public void drawWorld(WorldMap worldMap2) {
        clear();
        worldMap = worldMap2;
        int worldSize = worldMap2.size;
        pointerController = new WorldmapPointerController(this);
        cameraController = new WorldmapCameraController(this);

        Vector3Int cachePosition = new Vector3Int();
        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldSize; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, tileBases[Random.Range(0, tileBases.Length - 1)]);
            }
        }
    }

    public void enablePointer() {
        pointer.gameObject.SetActive(true);
    }

    public void enableLocationSelector() {
        locationSelector.gameObject.SetActive(true);
    }

    public void setCameraToCenter() {
        cameraController.setCameraToCenter();
    }
    
    public void clear() {
        tilemap.ClearAllTiles();
        pointer.gameObject.SetActive(false);
        locationSelector.gameObject.SetActive(false);
    }

    public Vector3 getPointerPosition() => pointerController.pointer.localPosition;

    public Vector3 getLocationPosition() => pointerController.locationSelector.localPosition;

    public void resetFlags() {
        pointerMoved = false;
        locationChanged = false;
    }
}
}