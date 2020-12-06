using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scenes.MainMenu.script;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class WorldMapDrawer : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public Camera _camera;
    public TileBase[] tileBases;

    private int worldSize { set { worldSize = value; } }
    private int tileSize = 32;

    public void drawWorld(WorldMap worldMap) {
        
        Vector3 bounds = new Vector3(worldMap.size * tileSize, worldMap.size * tileSize, 0);
        grid.GetComponent<RectTransform>().sizeDelta = new Vector2(bounds.x, bounds.y);
        _camera.transform.Translate(new Vector3(bounds.x / 2, bounds.y /2, 0));
        Vector3Int cachePosition = new Vector3Int();
        for (int x = 0; x < worldMap.size; x++) {
            for (int y = 0; y < worldMap.size; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, tileBases[0]);
            }
        }
    }

    // handles minimap input
    private void Update() {
        
        if(Input.GetKeyDown(KeyCode.UpArrow)) scrollMinimap(0, 1);
        if(Input.GetKeyDown(KeyCode.DownArrow)) scrollMinimap(0, -1);
        if(Input.GetKeyDown(KeyCode.LeftArrow)) scrollMinimap(-1, 0);
        if(Input.GetKeyDown(KeyCode.RightArrow)) scrollMinimap(1, 0);
        // if(Input.mousePosition)
        // if(Input.mouseScrollDelta.y != 0) zoomMinimap()  
    }

    private void scrollMinimap(int x, int y) {
        
    }

    private void zoomMinimap(int delta) {
        
    }
}
