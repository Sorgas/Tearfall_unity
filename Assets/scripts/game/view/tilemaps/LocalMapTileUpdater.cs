﻿using System.Collections.Generic;
using System.Linq;
using enums;
using enums.material;
using game.model;
using game.model.localmap;
using game.view.util;
using UnityEngine;
using UnityEngine.Tilemaps;
using util.geometry;
using util.lang;
using static enums.BlockTypeEnum;

// stores tilemaps of layers in array
// changes unity tilemaps to be consistent with local map in game model
// tiles are organized into layers: floor tiles, wall tiles, plants & buildings, liquids
namespace game.view.tilemaps {
    public class LocalMapTileUpdater {
        private readonly List<LocalMapLayerHandler> layers = new();
        public readonly TileSetHolder tileSetHolder = new();
        public GameObject layerPrefab;
        private RectTransform mapHolder;
        private LocalMap map;
        public int viewDepth = 6;
        
        // grid z is +0.1
        private const int FLOOR_LAYER = 9;
        private const int WALL_LAYER = 8;
        private const int SELECTION_LAYER = 0;

        public LocalMapTileUpdater(RectTransform mapHolder) {
            this.mapHolder = mapHolder;
            layerPrefab = Resources.Load<GameObject>("prefabs/LocalMapLayer");
            map = GameModel.localMap;
            tileSetHolder.loadAll();
        }

        public void flush() {
            Debug.Log("flushing localMap tiles");
            new Optional<LocalMap>(GameModel.localMap)
                .ifPresent(map => {
                    createLayers();
                    map.bounds.iterate(position => updateTile(position, false)); // no need to update ramps on whole map update
                });
        }

        public void updateTile(Vector3Int position, bool withRamps) => updateTile(position.x, position.y, position.z, withRamps);

        // updates state of visual tilemap to state of model local map
        public void updateTile(int x, int y, int z, bool withRamps) {
            string material = selectMaterial(x, y, z);
            BlockType blockType = get(map.blockType.get(x, y, z));
            Tile wallTile = null;
            Tile floorTile = null;
            // select wall part for non-flat types
            if (!blockType.FLAT) { 
                string wallTileName = blockType == RAMP ? selectRamp(x, y, z) : blockType.PREFIX;
                wallTile = tileSetHolder.tilesets[material][wallTileName]; // draw wall part
            }
            if (blockType != SPACE) {
                string floorTileName = (blockType == STAIRS || blockType == DOWNSTAIRS) ? DOWNSTAIRS.PREFIX : FLOOR.PREFIX;
                floorTile = tileSetHolder.tilesets[material][floorTileName]; // draw wall part
            }
            layers[z].setTile(new Vector3Int(x, y, FLOOR_LAYER), floorTile);
            layers[z].setTile(new Vector3Int(x, y, WALL_LAYER), wallTile);
            if (blockType == SPACE) setToppingForSpace(x, y, z);
            // if tile above is space, topping should be updated
            if (map.inMap(x, y, z + 1) && map.blockType.get(x, y, z + 1) == SPACE.CODE) updateTile(x, y, z + 1, false);
            if (withRamps) updateRampsAround(new Vector3Int(x, y, z));
        }

        private void createLayers() {
            Transform transform = mapHolder.transform;
            for (int i = 0; i <= map.bounds.maxZ; i++) {
                GameObject layer = Object.Instantiate(layerPrefab, new Vector3(0, i / 2f, -i * 2) + transform.position, Quaternion.identity, transform);
                layers.Add(layer.transform.GetComponentInChildren<LocalMapLayerHandler>());
            }
        }

        // when current tile is SPACE, draw special topping tile if lower tile is not SPACE.
        private void setToppingForSpace(int x, int y, int z) {
            if (z <= 0) return;
            BlockType blockType = get(map.blockType.get(x, y, z - 1));
            if (blockType != RAMP) return;
            string material = selectMaterial(x, y, z - 1);
            string type = selectRamp(x, y, z - 1) + "F";
            layers[z].setTile(new Vector3Int(x, y, FLOOR_LAYER), tileSetHolder.tilesets[material][type]); // topping corresponds lower tile
        }

        private string selectMaterial(int x, int y, int z) {
            return MaterialMap.get().material(map.blockType.material[x, y, z]).name;
        }

        //Observes tiles around given one, and updates atlasX for ramps.
        private void updateRampsAround(Vector3Int center) {
            PositionUtil.allNeighbour
                .Select(delta => center + delta) // get absolute position
                .Where(pos => map.inMap(pos))
                .Where(pos => map.blockType.get(pos) == BlockTypeEnum.RAMP.CODE)
                .ToList().ForEach(pos => updateTile(pos, false));
        }

        // Chooses ramp tile by surrounding walls. Don't touch!
        private string selectRamp(int x, int y, int z) {
            uint walls = observeWalls(x, y, z);
            if ((walls & 0b00001010) == 0b00001010) return "SW";
            if ((walls & 0b01010000) == 0b01010000) return "NE";
            if ((walls & 0b00010010) == 0b00010010) return "SE";
            if ((walls & 0b01001000) == 0b01001000) return "NW";
            if ((walls & 0b00010000) != 0) return "E";
            if ((walls & 0b01000000) != 0) return "N";
            if ((walls & 0b00000010) != 0) return "S";
            if ((walls & 0b00001000) != 0) return "W";
            if ((walls & 0b10000000) != 0) return "CNE";
            if ((walls & 0b00000100) != 0) return "CSE";
            if ((walls & 0b00100000) != 0) return "CNW";
            if ((walls & 0b00000001) != 0) return "CSW";
            return "C";
        }

        // Counts walls to choose ramp type and orientation.
        public uint observeWalls(int cx, int cy, int cz) {
            uint bitpos = 1;
            uint walls = 0;
            for (int y = cy - 1; y <= cy + 1; y++) {
                for (int x = cx - 1; x <= cx + 1; x++) {
                    if (x == cx && y == cy) continue;
                    if (map.blockType.get(x, y, cz) == WALL.CODE) walls |= bitpos;
                    bitpos *= 2; // shift to 1 bit
                }
            }
            return walls;
        }

        public void createSelectionTile(int x, int y, int z) {
            Vector3Int vector = new Vector3Int(x, y, SELECTION_LAYER);
            BlockType blockType = get(map.blockType.get(x, y, z));
            Tile tile = null;
            if (blockType != SPACE) {
                string type = blockType == RAMP ? selectRamp(x, y, z) : blockType.PREFIX;
                tile = tileSetHolder.tilesets["selection"][type];
            }
            layers[z].setTile(vector, tile);
            if (blockType == SPACE) setToppingForSpace(x, y, z);
        }

        public void hideSelectionTile(int x, int y, int z) {
            layers[z].setTile(new Vector3Int(x, y, SELECTION_LAYER), null);
        }

        // 
        public void updateLayersVisibility(int oldZ, int newZ) {
            for (int z = 0; z < map.bounds.maxZ; z++) {
                // Debug.Log(z);
                layers[z].setVisible(z > (newZ - viewDepth) && z <= newZ);
            }
        }
    }
}