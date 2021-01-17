using Assets.Scenes.MainMenu.script;
using UnityEngine;
using UnityEngine.Tilemaps;
using util;

public class WorldMapDrawer : MonoBehaviour {
    public Tilemap tilemap;
    public Grid grid;
    public Camera _camera;
    public RectTransform mask;
    public TileBase[] tileBases;

    private int minimapPadding = 2;
    public int worldSize;
    private int tileSize = 32;
    private ScrollableCameraController controller;
    
    public void drawWorld(WorldMap worldMap) {
        worldSize = worldMap.size;
        Vector3 bounds = new Vector3(worldSize * tileSize, worldSize * tileSize, 0);
        grid.GetComponent<RectTransform>().sizeDelta = new Vector2(bounds.x, bounds.y);
        controller = new ScrollableCameraController(mask.rect, _camera, worldSize);
        
        Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        random.InitState();
        Vector3Int cachePosition = new Vector3Int();
        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldSize; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, tileBases[random.NextInt(tileBases.Length - 1)]);
            }
        }
    }

    private void Update() {
        if(controller != null) controller.handleInput();
    }
}