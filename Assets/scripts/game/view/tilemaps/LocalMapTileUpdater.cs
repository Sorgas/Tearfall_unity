using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.localmap;
using game.view.util;
using Leopotam.Ecs;
using types;
using types.material;
using types.plant;
using UnityEngine;
using UnityEngine.Tilemaps;
using util.geometry;
using util.lang;
using util.lang.extension;
using static types.BlockTypes;
using static game.view.util.TilemapLayersConstants;

// stores tilemaps of layers in array
// changes unity tilemaps to be consistent with local map in game model
// tiles are organized into layers: floor tiles, wall tiles, plants & buildings, liquids
namespace game.view.tilemaps {
    public class LocalMapTileUpdater {
        private RampUtil rampUtil;
        private readonly List<LocalMapLayerHandler> layers = new();
        public readonly BlockTileSetHolder blockTileSetHolder = BlockTileSetHolder.get();
        public GameObject layerPrefab;
        private RectTransform mapHolder;
        private LocalModel model;
        private LocalMap map;
        private ToggleableLogger logger = new("LocalMapTileUpdater");
        private bool substratesEnabled = true;
        
        public LocalMapTileUpdater(RectTransform mapHolder, LocalModel model) {
            this.mapHolder = mapHolder;
            this.model = model;
            layerPrefab = Resources.Load<GameObject>("prefabs/LocalMapLayer");
            map = model.localMap;
            rampUtil = new(map);
            blockTileSetHolder.loadAll();
        }

        public void flush() {
            logger.log("flushing localMap tiles");
            createLayers();
            layers.ForEach(layer => layer.setLock());
            map.bounds.iterate(position => updateTile(position, false)); // no need to update ramps on whole map update
            layers.ForEach(layer => layer.unlock());
        }

        public void updateTile(Vector3Int position, bool withRamps) => updateTile(position.x, position.y, position.z, withRamps);

        // updates state of visual tilemap to state of model local map
        public void updateTile(int x, int y, int z, bool withRamps) {
            string material = selectMaterial(x, y, z);
            BlockType blockType = get(map.blockType.get(x, y, z));
            Vector3Int position = new(x, y, z);
            Tile wallTile = null, floorTile = null, substrateFloorTile = null, substrateWallTile = null;
            if (!blockType.flat) { // tile has wall part
                string wallTileName = blockType == RAMP ? rampUtil.selectRampPrefix(x, y, z) : blockType.PREFIX;
                wallTile = blockTileSetHolder.tiles[material][wallTileName]; // draw wall part
                if (map.substrateMap.has(position)) {
                    int id = map.substrateMap.get(position).type;
                    SubstrateType type = SubstrateTypeMap.get().get(id);
                    wallTileName += Random.Range(0, type.tilesetSize);
                    substrateWallTile = blockTileSetHolder.tiles[type.name][wallTileName];
                }
            }
            if (blockType != SPACE) { // tile has floor part
                string floorTileName = (blockType == STAIRS || blockType == DOWNSTAIRS) ? DOWNSTAIRS.PREFIX : FLOOR.PREFIX;
                floorTile = blockTileSetHolder.tiles[material][floorTileName]; // draw wall part
                if (blockType == FLOOR || blockType == DOWNSTAIRS) {
                    if (map.substrateMap.has(position)) {
                        int id = map.substrateMap.get(position).type;
                        SubstrateType type = SubstrateTypeMap.get().get(id);
                        floorTileName += Random.Range(0, type.tilesetSize);
                        substrateFloorTile = blockTileSetHolder.tiles[type.name][floorTileName];
                    } else if (model.farmContainer.isFarm(position)) {
                        substrateFloorTile = blockTileSetHolder.getFarmTile(material);
                    }
                }
            }
            LocalMapLayerHandler layerHandler = layers[z];
            bool wasLocked = layerHandler.locked;
            if (!wasLocked) layerHandler.setLock();
            layerHandler.setTile(new Vector3Int(x, y, FLOOR_LAYER), floorTile);
            layerHandler.setTile(new Vector3Int(x, y, WALL_LAYER), wallTile);
            layerHandler.setTile(new Vector3Int(x, y, ZONE_FLOOR_LAYER), getZoneTile(position));
            if (substratesEnabled) {
                layerHandler.setTile(new Vector3Int(x, y, SUBSTRATE_FLOOR_LAYER), substrateFloorTile);
                layerHandler.setTile(new Vector3Int(x, y, SUBSTRATE_WALL_LAYER), substrateWallTile);
            }
            if (blockType == SPACE) setToppingForSpace(x, y, z);

            // if tile above is space, topping should be updated
            if (map.inMap(x, y, z + 1) && map.blockType.get(x, y, z + 1) == SPACE.CODE) updateTile(x, y, z + 1, false);
            if (withRamps) updateRampsAround(new Vector3Int(x, y, z));
            
            if (!wasLocked) layerHandler.unlock();
        }

        private void createLayers() {
            Transform transform = mapHolder.transform;
            for (int i = 0; i <= map.bounds.maxZ; i++) {
                GameObject layer = Object.Instantiate(layerPrefab, new Vector3(0, i / 2f, -i * 2) + transform.position,
                    Quaternion.identity, transform);
                layer.name = "local map layer " + i;
                layers.Add(layer.transform.GetComponentInChildren<LocalMapLayerHandler>());
                if (GlobalSettings.USE_SPRITE_SORTING_LAYERS) {
                    layers[i].tilemap.GetComponent<TilemapRenderer>().sortingOrder = i;
                    layers[i].planeRenderer.sortingOrder = i;
                }
            }
        }

        // when current tile is SPACE, draw special topping tile if lower tile is not SPACE.
        private void setToppingForSpace(int x, int y, int z) {
            if (z <= 0) return;
            BlockType blockType = get(map.blockType.get(x, y, z - 1));
            if (blockType != RAMP) return;
            string material = selectMaterial(x, y, z - 1);
            string toppingTileName = rampUtil.selectRampPrefix(x, y, z - 1) + "F";
            layers[z].setTile(new Vector3Int(x, y, FLOOR_LAYER), blockTileSetHolder.tiles[material][toppingTileName]);
            Vector3Int lowerPosition = new(x, y, z - 1);
            if (substratesEnabled && map.substrateMap.has(lowerPosition)) {
                int id = map.substrateMap.get(lowerPosition).type;
                SubstrateType type = SubstrateTypeMap.get().get(id);
                toppingTileName += Random.Range(0, type.tilesetSize);
                Tile substrateTile = blockTileSetHolder.tiles[type.name][toppingTileName];
                layers[z].setTile(new Vector3Int(x, y, SUBSTRATE_FLOOR_LAYER), substrateTile);
            }
        }

        private string selectMaterial(int x, int y, int z) {
            return MaterialMap.get().material(map.blockType.material[x, y, z]).name;
        }

        //Observes tiles around given one, and updates atlasX for ramps.
        private void updateRampsAround(Vector3Int center) {
            PositionUtil.allNeighbour
                .Select(delta => center + delta) // get absolute position
                .Where(pos => map.inMap(pos))
                .Where(pos => map.blockType.get(pos) == RAMP.CODE)
                .ToList().ForEach(pos => updateTile(pos, false));
        }

        private Tile getZoneTile(Vector3Int position) {
            EcsEntity zone = model.zoneContainer.getZone(position);
            return zone == EcsEntity.Null ? null : blockTileSetHolder.zoneTiles[zone.take<ZoneComponent>().type];
        }

        public void createSelectionTile(int x, int y, int z) {
            Vector3Int vector = new(x, y, SELECTION_LAYER);
            BlockType blockType = get(map.blockType.get(x, y, z));
            Tile tile = null;
            if (blockType != SPACE) {
                string type = blockType == RAMP ? rampUtil.selectRampPrefix(x, y, z) : blockType.PREFIX;
                tile = blockTileSetHolder.tiles["selection"][type];
            }
            layers[z].setTile(vector, tile);
            if (blockType == SPACE) setToppingForSpace(x, y, z);
        }

        public void hideSelectionTile(int x, int y, int z) {
            layers[z].setTile(new Vector3Int(x, y, SELECTION_LAYER), null);
        }
        
        public void updateLayersVisibility(int newZ) {
            for (int z = 0; z < map.bounds.maxZ; z++) {
                bool visible = z > (newZ - GlobalSettings.cameraLayerDepth) && z <= newZ;
                layers[z].setVisible(visible);
                if (visible) {
                    // layers[z].tilemap.color.
                }
            }
        }
    }
}