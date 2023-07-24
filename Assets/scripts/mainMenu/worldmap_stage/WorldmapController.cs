using game.model;
using mainMenu.worldmap;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace mainMenu.worldmap_stage {
// reads mouse movement and clicks for world map. updates selectors positions.
public class WorldmapController : MonoBehaviour {
    // external objects
    public RectTransform mask;
    public Button mapPanel;
    public TileBase[] tileBases; // TODO replace with actual world tiles
    public TextMeshProUGUI nameText;

    // prefab components
    public Transform pointer;
    public Transform locationSelector;
    public Tilemap tilemap;
    public Camera _camera;

    public bool pointerMoved;
    public bool locationChanged;

    private int tileSize = 32;
    private WorldmapCameraController cameraController = new();
    private WorldmapPointerController pointerController= new();
    public WorldMap worldMap;
    public PlayerControls playerControls;

    private void Update() {
        cameraController?.update();
        pointerController?.update();
    }

    public void setWorldMap(WorldMap newWorldMap) {
        clear();
        playerControls = new();
        playerControls.Enable();
        pointerController.init(this);
        cameraController.init(this);
        worldMap = newWorldMap;
        pointerController.setWorldMap(worldMap);
        cameraController.setWorldMap(worldMap);
        int worldSize = newWorldMap.size;
        Vector3Int cachePosition = new Vector3Int();
        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldSize; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, tileBases[Random.Range(0, tileBases.Length - 1)]);
            }
        }
    }

    public void setWorldName(string name) {
        nameText.text = name;
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