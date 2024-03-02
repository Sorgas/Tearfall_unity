using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using types;
using types.material;
using types.plant;
using UnityEngine;
using UnityEngine.Tilemaps;
using util.lang;
using util.lang.extension;

namespace game.view.tilemaps {
    // stores block sprites and tiles. Some tilesets share same texture.
    public class BlockTileSetHolder : Singleton<BlockTileSetHolder> {
        // tileset -> tilecode -> tile/sprite
        public readonly Dictionary<string, Dictionary<string, Sprite>> sprites = new();
        // name -> tilecode -> tile
        public readonly Dictionary<string, Dictionary<string, Tile>> tiles = new();

        public readonly Dictionary<ZoneTypeEnum, Tile> zoneTiles = new();
        public readonly Dictionary<string, Tile> farmTiles = new();
        public readonly Dictionary<string, Sprite> roomSprites = new();
        private BlockTilesetSlicer slicer = new();
        Dictionary<string, List<string>> notFound = new();
        private string logMessage;
        private bool debug = false;

        public void loadAll() {
            logMessage = "";
            log("[BlockTilesetHolder] loading block tilesets");
            // TODO try use this for all sprites and tilesets
            MaterialMap.get().all
                .Where(material => material.tileset != null)
                .ForEach(loadMaterialTilesetFromAtlas); // sets color
            loadTilesetFromAtlas("selection");
            loadTilesetFromAtlas("template");
            SubstrateTypeMap.get().all()
                .ForEach(loadSubstrateTilesetFromAtlas);

            createZoneTiles();
            flushLog();
        }

        public Sprite getSprite(string material, string tilecode) {
            if (!tiles.ContainsKey(material)) material = "template";
            return tiles[material][tilecode].sprite;
        }

        public Tile getFarmTile(string materialName) {
            if (!farmTiles.ContainsKey(materialName)) {
                Sprite sprite = TextureLoader.get().getSprite("farm_tile");
                Material_ material = MaterialMap.get().material(materialName);
                farmTiles.Add(materialName, createTile(sprite, material.color));
            }
            return farmTiles[materialName];
        }

        private void loadMaterialTilesetFromAtlas(Material_ material) => loadTilesetFromAtlas(material.name, material.tileset, 1, material.color);

        private void loadTilesetFromAtlas(string tileset) => loadTilesetFromAtlas(tileset, tileset, 1, Color.white);

        private void loadSubstrateTilesetFromAtlas(SubstrateType type) => loadTilesetFromAtlas(type.name, type.tileset, type.tilesetSize, type.color);

        private void loadRoomTiles() {
            
        }
        
        private void loadTilesetFromAtlas(string tilesName, string tileset, int tilesetSize, Color color) {
            log("adding " + tileset);
            loadSpriteSet(tileset, tilesetSize);
            createTilesFromSprites(tilesName, tileset, color);
        }

        // lazy-loads tileset sprite from atlas and slices it to tile sprites
        private void loadSpriteSet(string tileset, int tilesetSize) {
            if (sprites.ContainsKey(tileset)) return;
            Sprite sprite = TextureLoader.get().getSprite(tileset);
            sprites.Add(tileset, slicer.sliceBlockSpritesheet(sprite, tilesetSize));
        }

        // creates tileset and applies color
        private void createTilesFromSprites(string tilesName, string tileset, Color color) {
            Dictionary<string, Tile> tilesMap = new();
            Dictionary<string, Sprite> tilesetSprites = sprites[tileset];
            foreach (string key in tilesetSprites.Keys) {
                tilesMap.Add(key, createTile(tilesetSprites[key], color));
            }
            tiles.Add(tilesName, tilesMap);
        }

        private void createZoneTiles() {
            Sprite sprite = TextureLoader.get().getSprite("zone_tile");
            foreach (ZoneType zoneType in ZoneTypes.all) {
                zoneTiles.Add(zoneType.value, createTile(sprite, zoneType.tileColor));
            }
        }


        // creates tile by applying color to sprite
        private Tile createTile(Sprite sprite, Color color) {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            tile.color = color;
            return tile;
        }


        private void log(string message) {
            if (debug) logMessage += message + "\n";
        }

        private void flushLog() {
            if (!debug) return;
            foreach (string tileset in notFound.Keys) {
                log($"tileset {tileset} not found for type: {notFound[tileset]}");
            }
            notFound.Clear();
            Debug.Log(logMessage);
        }
    }
}