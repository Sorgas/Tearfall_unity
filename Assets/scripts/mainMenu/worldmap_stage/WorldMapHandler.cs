using game.model;
using game.view.tilemaps;
using generation.worldgen;
using UnityEngine;
using UnityEngine.Tilemaps;
using util;
using util.geometry.bounds;

namespace mainMenu.worldmap_stage {
// Draws tiles of world on tilemap 
public class WorldMapHandler : MonoBehaviour {
    public Tilemap tilemap;
    public WorldMapTilesetHolder tilesetHolder;

    private WorldMap worldMap;
    private WorldGenConfig config = new();

    private string currentOverlay;
    private Vector3Int cachePosition = new();
    private static float overlayAlpha = 0.5f;
    private Color maxTemperatureColor = new(1, 0, 0, overlayAlpha);
    private Color minTemperatureColor = new(0, 0, 1, overlayAlpha);
    private IntBounds2 bounds;
    
    public static string ELEVATION_OVERLAY = "elevationOverlay";
    public static string TEMPERATURE_OVERLAY = "temperatureOverlay";
    public static string RAINFALL_OVERLAY = "rainfallOverlay";
    
    private void Start() {
        tilesetHolder = new();
    }
    
    public void draw(WorldMap worldMap) {
        this.worldMap = worldMap;
        bounds = new IntBounds2(worldMap.size, worldMap.size);
        bounds.iterate((x, y) => {
            cachePosition.Set(x, y, 0);
            string tileName = getWorldTileName(x, y);
            tilemap.SetTile(new Vector3Int(x, y, 0), tilesetHolder.getTile(tileName));
        });
        if (currentOverlay != null) {
            showOverlay(currentOverlay);
        }
    }

    public void clear() {
        if (worldMap == null) return;
        // if(elevationOverlayShown) toggleElevationOverlay();
        int worldSize = worldMap.size;
        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldSize; y++) {
                cachePosition.Set(x, y, 0);
                tilemap.SetTile(cachePosition, null);
            }
        }
        worldMap = null;
    }

    public void toggleOverlay(string overlayName) {
        if (worldMap == null) return;
        if (currentOverlay == overlayName) { // same button pressed
            clearOverlay();
        } else {
            if (currentOverlay != null) { // another overlay
                clearOverlay();
            }
            showOverlay(overlayName);
        }
    }

    private void showOverlay(string overlayName) {
        if (overlayName == ELEVATION_OVERLAY) {
            showElevationOverlay();
        } else if (overlayName == TEMPERATURE_OVERLAY) {
            showTemperatureOverlay();
        } else if (overlayName == RAINFALL_OVERLAY) {
            showRainfallOverlay();
        } else {
            throw new GameException("Overlay name not supported");
        }
        currentOverlay = overlayName;
    }
    
    private void showElevationOverlay() {
        bounds.iterate((x, y) => {
            cachePosition.Set(x, y, -1);
            Tile tile = tilesetHolder.getTile("overlay");
            float colorValue = (worldMap.elevation[x, y] + config.minElevation) / config.maxElevation - config.minElevation;
            tile.color = new Color(colorValue, colorValue, colorValue, overlayAlpha);
            tilemap.SetTile(cachePosition, tile);
        });
    }

    private void showTemperatureOverlay() {
        bounds.iterate((x, y) => {
            cachePosition.Set(x, y, -1);
            Tile tile = tilesetHolder.getTile("overlay");
            float meanTemperature = (worldMap.summerTemperature[x, y] + worldMap.winterTemperature[x, y]) / 2f;
            tile.color = Color.Lerp(minTemperatureColor, maxTemperatureColor, (meanTemperature - config.minTemperature) / (config.maxTemperature - config.minTemperature));
            tilemap.SetTile(cachePosition, tile);
        });
    } 
    
    private void showRainfallOverlay() {
        bounds.iterate((x, y) => {
            cachePosition.Set(x, y, -1);
            Tile tile = tilesetHolder.getTile("overlay");
            float rainfall = (worldMap.rainfall[x, y] - config.minRainfall) / (config.maxRainfall - config.minRainfall);
            tile.color = new Color(rainfall, rainfall, rainfall, overlayAlpha);
            tilemap.SetTile(cachePosition, tile);
        });
    }

    private void clearOverlay() {
        bounds.iterate((x, y) => {
            cachePosition.Set(x, y, -1);
            tilemap.SetTile(cachePosition, null);
        });
        currentOverlay = null;
    }
    
    private string getWorldTileName(int x, int y) {
        if (worldMap.elevation[x, y] > 0.85f) return "snowMountain";
        if (worldMap.elevation[x, y] > 0.5f) return "mountain";
        if (worldMap.elevation[x, y] > config.seaLevel) return "greenPlain";
        return "sea";
    }
}
}