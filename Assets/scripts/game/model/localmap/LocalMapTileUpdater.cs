using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts.enums;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util;
using Assets.scripts.util.geometry;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Assets.scripts.enums.BlockTypeEnum;

// updates tilemaps on change of localmap cells
// tiles are organized into layers: floor tiles, wall tiles, plants & buildings, liquids
public class LocalMapTileUpdater : MonoBehaviour {
    private List<Tilemap> layers = new List<Tilemap>();
    private Vector3Int cachePosition = new Vector3Int();
    private Vector3Int cachePosition2 = new Vector3Int();
    public TileSetHolder tileSetHolder;
    public GameObject layerPrefab;
    private LocalMap map;

    private const int FLOOR_LAYER = 0;
    private const int WALL_LAYER = 1;
    void Start() {
        flush();
    }

    public void flush() {
        map = GameModel.get().localMap;
        if(map == null) {
            Debug.Log("map is null");
            return;
        }
        createLayers(map);
        map.bounds.iterate(position => updateTile(position));
    }

    public void updateTile(IntVector3 position) => updateTile(position.x, position.y, position.z);

    public void updateTile(int x, int y, int z) {
        cachePosition.Set(x, y, FLOOR_LAYER);
        cachePosition2.Set(x, y, WALL_LAYER);
        string material = "template"; //TODO
        BlockType blockType = BlockTypeEnum.get(map.blockType.get(x, y, z));
        if (blockType == SPACE) { // delete tile
            layers[z].SetTile(cachePosition, null);
            layers[z].SetTile(cachePosition2, null);
            setToppingForSpace(x, y, z);
        } else {
            layers[z].SetTile(cachePosition, tileSetHolder.tilesets[material]["F"]); // floor is drawn under all tiles
            string type = blockType == RAMP ? selectRamp() : blockType.PREFIX;
            layers[z].SetTile(cachePosition2, tileSetHolder.tilesets[material][type]); // draw wall part
        }
    }

    private void createLayers(LocalMap map) {
        Debug.Log("map size = " + map.zSize);
        for (int i = 0; i < map.zSize; i++) {
            GameObject layer = Instantiate(layerPrefab, new Vector3(0, 0, i * BlockTilesetLoader.WALL_HEIGHT), Quaternion.identity, gameObject.transform);
            layers.Add(layer.transform.GetComponentInChildren<Tilemap>());
        }
    }
    private void setToppingForSpace(int x, int y, int z) {
        if (z <= 0) return;
        string material = "template"; //TODO
        BlockType blockType = BlockTypeEnum.get(map.blockType.get(x, y, z - 1));
        if (blockType == SPACE) { // no topping 
            layers[z].SetTile(cachePosition, null);
        } else {
            string type = "F" + (blockType == RAMP ? selectRamp() : blockType.PREFIX);
            layers[z].SetTile(cachePosition, tileSetHolder.tilesets[material][type]); // topping corresponds lower tile
        }
    }

    private string selectRamp() {
        return "E"; //TODO select ramp direction by surrounding walls
    }
}
