using game.model;
using game.view.tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace mainMenu.worldmap_stage {
// Draws tiles of world on tilemap 
public class WorldMapHandler : MonoBehaviour {
    public Tilemap tilemap;
    public WorldMapTilesetHolder tilesetHolder;

    private WorldMap worldMap;
    
    
    private void Start() {
        tilesetHolder = new();
    }

    public void draw(WorldMap worldMap) {
        this.worldMap = worldMap;
        int worldSize = worldMap.size;
        Vector3Int cachePosition = new Vector3Int();
        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldSize; y++) {
                cachePosition.Set(x, y, 0);
                string tileName = getWorldTileName(x, y);
                tilemap.SetTile(new Vector3Int(x, y, 0), tilesetHolder.getTile(tileName));
            }
        }
    }

    public void clear() {
        int worldSize = worldMap.size;
        Vector3Int cachePosition = new Vector3Int();
        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldSize; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, null);
            }
        }
        worldMap = null;
    }

    public void toggleElevationOverlay() {
        
    }

    private string getWorldTileName(int x, int y) {
        if (worldMap.elevation[x,y] > 0.5f) {
            return "greenPlain";
        } else {
            return "sea";
        }
    }
    
    private void setTile(int x, int y, string tileName) {
    }
}
}