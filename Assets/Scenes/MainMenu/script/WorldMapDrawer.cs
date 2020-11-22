using System.Collections;
using System.Collections.Generic;
using Assets.Scenes.MainMenu.script;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMapDrawer : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public Camera _camera;
    public TileBase[] tileBases;

    private int worldSize { set { worldSize = value; } }
    private int tileSize = 32;

    public void drawWorld(WorldMap worldMap) {
        Vector3 bounds = new Vector3(worldMap.width * tileSize, worldMap.height * tileSize, 0);
        grid.GetComponent<RectTransform>().sizeDelta = new Vector2(bounds.x, bounds.y);
        _camera.transform.Translate(new Vector3(bounds.x / 2, bounds.y /2, 0));
        Vector3Int cachePosition = new Vector3Int();
        for (int x = 0; x < worldMap.width; x++) {
            for (int y = 0; y < worldMap.height; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, tileBases[0]);
            }
        }
    }
}
