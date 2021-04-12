using System.Collections.Generic;
using Assets.scripts.enums;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.geometry;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Assets.scripts.enums.BlockTypeEnum;

// updates tilemaps on change of localmap cells
// tiles are organized into layers: floor tiles, wall tiles, plants & buildings, liquids
public class LocalMapTileUpdater : MonoBehaviour {
    private readonly List<Tilemap> layers = new List<Tilemap>();
    private readonly Vector3Int floorPosition = new Vector3Int();
    private readonly Vector3Int wallPosition = new Vector3Int();
    
    public readonly TileSetHolder tileSetHolder = new TileSetHolder();
    public GameObject layerPrefab;
    private GameObject mapHolder;
    private LocalMap map;

    private const int FLOOR_LAYER = 0;
    private const int WALL_LAYER = 1;

    public LocalMapTileUpdater(GameObject mapHolder) {
        this.mapHolder = mapHolder;
        layerPrefab = Resources.Load<GameObject>("prefabs/LocalMapLayer");
        map = GameModel.get().localMap;
        tileSetHolder.loadAll();
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
        Debug.Log("int: " + x);
        wallPosition.Set(x, y, WALL_LAYER);
        floorPosition.Set(x, y, FLOOR_LAYER);
        Debug.Log("vector: " + wallPosition.x);
        string material = "template"; //TODO
        BlockType blockType = BlockTypeEnum.get(map.blockType.get(x, y, z));
        if (blockType == SPACE) { // delete tile
            layers[z].SetTile(floorPosition, null);
            layers[z].SetTile(wallPosition, null);
            setToppingForSpace(x, y, z);
        } else {
            // Debug.Log("placing tile at " + wallPosition.x + " " + wallPosition.y + " " + z);
            layers[z].SetTile(floorPosition, tileSetHolder.tilesets[material]["WALLF"]); // floor is drawn under all tiles
            string type = blockType == RAMP ? selectRamp() : blockType.PREFIX;
            layers[z].SetTile(wallPosition, tileSetHolder.tilesets[material][type]); // draw wall part
        }
    }

    private void createLayers(LocalMap map) {
        Transform transform = mapHolder.transform;
        for (int i = 0; i < map.zSize; i++) {
            GameObject layer = GameObject.Instantiate(layerPrefab, new Vector3(0, i / 2f, -i * 2) + transform.position, Quaternion.identity, transform);
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
