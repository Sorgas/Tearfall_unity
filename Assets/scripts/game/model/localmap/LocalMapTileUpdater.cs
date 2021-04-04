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
    private Vector3Int floorPosition = new Vector3Int();
    private Vector3Int wallPosition = new Vector3Int();
    public TileSetHolder tileSetHolder = new TileSetHolder();
    public GameObject layerPrefab;
    private LocalMap map;

    private const int FLOOR_LAYER = 0;
    private const int WALL_LAYER = 1;
    void Start() {
        tileSetHolder.loadAll();
        flush();
    }

    public void flush() {
        map = GameModel.get().localMap;
        if (map == null) {
            Debug.Log("map is null");
            return;
        }
        createLayers(map);
        map.bounds.iterate(position => updateTile(position));
    }

    public void updateTile(IntVector3 position) => updateTile(position.x, position.y, position.z);

    public void updateTile(int x, int y, int z) {
        floorPosition.Set(x, y, FLOOR_LAYER);
        wallPosition.Set(x, y, WALL_LAYER);
        string material = "template"; //TODO
        BlockType blockType = BlockTypeEnum.get(map.blockType.get(x, y, z));
        // if ((x + y + z) % 2 == 0) {
        //     blockType = FLOOR;
        // } else {
        //     blockType = WALL;
        // }
        // Debug.Log(blockType.NAME);
        if (blockType == SPACE) { // delete tile
            layers[z].SetTile(floorPosition, null);
            layers[z].SetTile(wallPosition, null);
            setToppingForSpace(x, y, z);
        } else {
            layers[z].SetTile(floorPosition, tileSetHolder.tilesets[material]["WALLF"]); // floor is drawn under all tiles
            string type = blockType == RAMP ? selectRamp() : blockType.PREFIX;
            layers[z].SetTile(wallPosition, tileSetHolder.tilesets[material][type]); // draw wall part
        }
    }

    private void createLayers(LocalMap map) {
        Debug.Log("map size = " + map.zSize);
        Transform transform = gameObject.transform;
        for (int i = 0; i < map.zSize; i++) {
            GameObject layer = Instantiate(layerPrefab, new Vector3(0, i / 2f, -i * 2) + transform.position, Quaternion.identity, transform);
            layers.Add(layer.transform.GetComponentInChildren<Tilemap>());
        }
    }
    private void setToppingForSpace(int x, int y, int z) {
        if (z <= 0) return;
        string material = "template"; //TODO
        BlockType blockType = BlockTypeEnum.get(map.blockType.get(x, y, z - 1));
        if (blockType == SPACE || blockType == FLOOR) { // no topping 
            layers[z].SetTile(floorPosition, null);
        } else {
            string type = (blockType == RAMP ? selectRamp() : blockType.PREFIX) + "F";
            Debug.Log(type);
            layers[z].SetTile(floorPosition, tileSetHolder.tilesets[material][type]); // topping corresponds lower tile
        }
    }

    private string selectRamp() {
        return "E"; //TODO select ramp direction by surrounding walls
    }
}
