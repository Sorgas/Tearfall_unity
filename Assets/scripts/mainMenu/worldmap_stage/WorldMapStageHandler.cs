using System;
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

    public bool pointerMoved;
    public bool locationChanged;

    public TextMeshProUGUI tileInfoText;
    
    // overlay buttons
    public RectTransform overlayButtonsPanel;
    public Button elevationOverlayButton;
    private bool elevationShown;
    
    private int tileSize = 32;
    public WorldMapHandler worldMapHandler;
    private WorldmapCameraController cameraController;
    private WorldmapPointerController pointerController;
    private WorldMap worldMap;

    private Vector3Int pointerPosition;
    
    private bool enabled = false;
    
    public void Start() {
        cameraController = gameObject.GetComponent<WorldmapCameraController>();
        pointerController = gameObject.GetComponent<WorldmapPointerController>();
        elevationOverlayButton.onClick.AddListener(() => worldMapHandler.toggleElevationOverlay());
    }

    public void Update() {
        if (pointerPosition != pointerController.pointerPosition) {
            pointerPosition = pointerController.pointerPosition;
            updateTileInfoText();
        }
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

    private void setWorldMap(WorldMap map) {
        worldMap = map;
        cameraController.setWorldMap(map);
        pointerController.setWorldMap(map);
    }

    private void updateTileInfoText() {
        tileInfoText.text = $"Position: [{pointerPosition.x:###}.{pointerPosition.y:###}] \n" +
                            $"    elevation: {worldMap.elevation[pointerPosition.x, pointerPosition.y]}";
    }
    
    public Vector3 getPointerPosition() => pointerController.pointer.localPosition;
    
    public Vector3 getLocationPosition() => pointerController.locationSelector.localPosition;
}
}