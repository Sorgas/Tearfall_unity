using System.Collections.Generic;
using System.Linq;
using enums;
using enums.material;
using game.model.localmap;
using UnityEngine;
using UnityEngine.Tilemaps;
using util.geometry;
using util.lang;
using static enums.BlockTypeEnum;

// changes unity tilemaps to be consistent with local map in game model
// tiles are organized into layers: floor tiles, wall tiles, plants & buildings, liquids
namespace game.model.tilemaps {
    public class LocalMapTileUpdater {
        private readonly List<Tilemap> layers = new List<Tilemap>();
        // cache tilemap position of a tile
        private Vector3Int floorPosition = new Vector3Int();
        private Vector3Int wallPosition = new Vector3Int();

        public readonly TileSetHolder tileSetHolder = new TileSetHolder();
        public GameObject layerPrefab;
        private RectTransform mapHolder;
        private LocalMap map;

        private const int FLOOR_LAYER = 0;
        private const int WALL_LAYER = 1;

        public LocalMapTileUpdater(RectTransform mapHolder) {
            this.mapHolder = mapHolder;
            layerPrefab = Resources.Load<GameObject>("prefabs/LocalMapLayer");
            map = GameModel.get().localMap;
            tileSetHolder.loadAll();
        }

        public void flush() {
            Debug.Log("flushing localMap tiles");
            new Optional<LocalMap>(GameModel.get().localMap)
                .ifPresent(map => {
                    createLayers(map);
                    map.bounds.iterate(position => updateTile(position, false)); // no need to update ramps on whole map update
                });
        }

        public void updateTile(Vector3Int position, bool withRamps) => updateTile(position.x, position.y, position.z, withRamps);

        public void updateTile(int x, int y, int z, bool withRamps) {
            wallPosition.Set(x, y, WALL_LAYER);
            floorPosition.Set(x, y, FLOOR_LAYER);
            string material = selectMaterial(x, y, z);
            BlockType blockType = BlockTypeEnum.get(map.blockType.get(x, y, z));
            Tile floorTile = null;
            Tile wallTile = null;
            if (blockType != SPACE) {
                string type = blockType == RAMP ? selectRamp(x, y, z) : blockType.PREFIX;
                floorTile = tileSetHolder.tilesets[material]["WALLF"]; // floor is drawn under all tiles
                wallTile = tileSetHolder.tilesets[material][type]; // draw wall part
            }
            layers[z].SetTile(floorPosition, floorTile);
            layers[z].SetTile(wallPosition, wallTile);
            if (blockType == SPACE) setToppingForSpace(x, y, z);
        }

        private void createLayers(LocalMap map) {
            Transform transform = mapHolder.transform;
            for (int i = 0; i < map.zSize; i++) {
                GameObject layer = GameObject.Instantiate(layerPrefab, new Vector3(0, i / 2f, -i * 2) + transform.position, Quaternion.identity, transform);
                layers.Add(layer.transform.GetComponentInChildren<Tilemap>());
            }
        }

        // when current tile is SPACE, draw special topping tile if lower tile is not SPACE.
        private void setToppingForSpace(int x, int y, int z) {
            if (z <= 0) return;
            string material = selectMaterial(x, y, z - 1);
            BlockType blockType = BlockTypeEnum.get(map.blockType.get(x, y, z - 1));
            if (blockType != SPACE && blockType != FLOOR) {
                string type = (blockType == RAMP ? selectRamp(x, y, z - 1) : blockType.PREFIX) + "F";
                layers[z].SetTile(floorPosition, tileSetHolder.tilesets[material][type]); // topping corresponds lower tile
            }
        }

        private string selectMaterial(int x, int y, int z) {
            return MaterialMap.get().material(map.blockType.material[x,y,z]).name;
        }

        //Observes tiles around given one, and updates atlasX for ramps.
        private void updateRampsAround(Vector3Int center) {
            PositionUtil.allNeighbour
                .Select(delta => center + delta) // get absolute position
                .Where(pos => map.inMap(pos))
                .Where(pos => map.blockType.get(pos) == BlockTypeEnum.RAMP.CODE)
                .ToList().ForEach(pos => updateTile(pos, false));
        }

        // Chooses ramp tile by surrounding walls.
        private string selectRamp(int x, int y, int z) {
            uint walls = observeWalls(x, y, z);
            if ((walls & 0b00001010) == 0b00001010) {
                return "SW";
            } else if ((walls & 0b01010000) == 0b01010000) {
                return "NE";
            } else if ((walls & 0b00010010) == 0b00010010) {
                return "SE";
            } else if ((walls & 0b01001000) == 0b01001000) {
                return "NW";
            } else if ((walls & 0b00010000) != 0) {
                return "E";
            } else if ((walls & 0b01000000) != 0) {
                return "N";
            } else if ((walls & 0b00000010) != 0) {
                return "S";
            } else if ((walls & 0b00001000) != 0) {
                return "W";
            } else if ((walls & 0b10000000) != 0) {
                return "CNE";
            } else if ((walls & 0b00000100) != 0) {
                return "CSE";
            } else if ((walls & 0b00100000) != 0) {
                return "CNW";
            } else if ((walls & 0b00000001) != 0) {
                return "CSW";
            } else
                return "C";
        }

        // Counts walls to choose ramp type and orientation.
        public uint observeWalls(int cx, int cy, int cz) {
            uint bitpos = 1;
            uint walls = 0;
            for (int y = cy - 1; y <= cy + 1; y++) {
                for (int x = cx - 1; x <= cx + 1; x++) {
                    if ((x == cx) && (y == cy)) continue;
                    if (map.blockType.get(x, y, cz) == BlockTypeEnum.WALL.CODE) walls |= bitpos;
                    bitpos *= 2; // shift to 1 bit
                }
            }
            return walls;
        }
    }
}
