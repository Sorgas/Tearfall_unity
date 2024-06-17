using game.model;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace mainMenu.worldmap_stage {
// reads mouse movement and clicks for world map. updates selectors positions.
[RequireComponent(typeof(WorldmapCameraController))]
public class WorldMapStageHandler : MonoBehaviour {
    // external objects
    public RectTransform mask;
    public Button mapPanel;
    public TextMeshProUGUI nameText;

    // prefab components
    public Transform pointer;
    public Transform locationSelector;
    public Tilemap tilemap;
    public Camera _camera;

    public bool pointerMoved;
    public bool locationChanged;

    // overlay buttons
    public RectTransform overlayButtonsPanel;
    public Button elevationOverlayButton;
    private bool elevationShown;
    
    private int tileSize = 32;
    public WorldMapHandler worldMapHandler;
    private WorldmapCameraController cameraController;
    private WorldmapPointerController pointerController;
    public WorldMap worldMap;

    private bool enabled = false;
    
    public void Start() {
        cameraController = gameObject.GetComponent<WorldmapCameraController>();
        pointerController = new(this);
        elevationOverlayButton.onClick.AddListener(() => worldMapHandler.toggleElevationOverlay());
    }
    
    public void Update() {
        if (!enabled) return;
        pointerController.update();
    }

    // Redraws tilemap with map of given world
    // TODO overlays
    public void setWorld(World world) {
        clear();
        setWorldMap(world.worldModel.worldMap);
        worldMapHandler.draw(world.worldModel.worldMap);
        pointerController.setWorldMap(worldMap);
        
        nameText.text = world.name;
        setCameraToCenter();
        enablePointer();
        overlayButtonsPanel.gameObject.SetActive(true);
        enabled = true;
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
        setWorldMap(null);
        tilemap.ClearAllTiles();
        pointer.gameObject.SetActive(false);
        locationSelector.gameObject.SetActive(false);
        overlayButtonsPanel.gameObject.SetActive(false);
        enabled = false;
    }

    public void setWorldMap(WorldMap map) {
        worldMap = map;
        cameraController.setWorldMap(map);
    }
    
    public Vector3 getPointerPosition() => pointerController.pointer.localPosition;
    
    public Vector3 getLocationPosition() => pointerController.locationSelector.localPosition;
}
}