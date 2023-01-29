using System.Collections.Generic;
using System.Linq;
using types;
using types.material;
using types.plant;
using UnityEngine;
using UnityEngine.Tilemaps;
using util.lang;
using util.lang.extension;

namespace game.view.tilemaps {
    // stores block sprites and tiles
    public class BlockTileSetHolder : Singleton<BlockTileSetHolder> {
        // map of tileset -> tilecode -> tile/sprite
        public readonly Dictionary<string, Dictionary<string, Sprite>> sprites = new();

        // map of <material -> <tilecode -> tile>>
        public readonly Dictionary<string, Dictionary<string, Tile>> tiles = new();
        public readonly Dictionary<int, Dictionary<string, Tile>> substrateTiles = new();

        public readonly Dictionary<ZoneTypeEnum, Tile> zoneTiles = new();
        private BlockTilesetSlicer slicer = new();
        Dictionary<string, List<string>> notFound = new();

        private string logMessage;

        public void loadAll() {
            logMessage = "";
            Debug.Log("loading block tilesets");
            // TODO try use this for all sprites and tilesets
            MaterialMap.get().all
                .Where(material => material.tileset != null)
                .ForEach(material => loadMaterialTilesetFromAtlas(material));
            loadTilesetFromAtlas("selection");
            loadTilesetFromAtlas("template");
            SubstrateTypeMap.get().all()
                .ForEach(type => loadSubstrateTilesetFromAtlas(type));
            createZoneTiles();
            flushNotFound();
            Debug.Log("[BlockTilesetHolder]" + logMessage);
        }

        public Sprite getSprite(string material, string tilecode) {
            if (!tiles.ContainsKey(material)) material = "template";
            return tiles[material][tilecode].sprite;
        }

        private void loadTilesetFromAtlas(string tilesetName) {
            Sprite sprite = TexturePacker.createSpriteFromAtlas(tilesetName);
            Dictionary<string, Sprite> spriteMap = slicer.sliceBlockSpritesheet(sprite);
            log("adding " + tilesetName);
            sprites.Add(tilesetName, spriteMap);
            tiles.Add(tilesetName, createTilesFromSprites(spriteMap, Color.white));
        }

        // looks for sprite of material in atlas. If not present, uses template sprite.
        private void loadMaterialTilesetFromAtlas(Material_ material) {
            log("adding " + material.name);
            Dictionary<string, Sprite> spritesMap = getBlockTileset(material.tileset);
            tiles.Add(material.name, createTilesFromSprites(spritesMap, material.color));
        }

        private Dictionary<string, Tile> createTilesFromSprites(Dictionary<string, Sprite> sprites, Color color) {
            Dictionary<string, Tile> tilesMap = new();
            foreach (string key in sprites.Keys) {
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = sprites[key];
                tile.color = color;
                tilesMap.Add(key, tile);
            }
            return tilesMap;
        }

        private void loadSubstrateTilesetFromAtlas(SubstrateType type) {
            log("adding " + type.name);
            Dictionary<string, Sprite> sprites = getBlockTileset(type.tileset, type.tilesetSize);
            substrateTiles.Add(type.id, createTilesFromSprites(sprites, type.color));
        }

        private Dictionary<string, Sprite> getBlockTileset(string tileset) => getBlockTileset(tileset, 1);

        private Dictionary<string, Sprite> getBlockTileset(string tileset, int tilesetSize) {
            if (!sprites.ContainsKey(tileset)) {
                Sprite sprite = TexturePacker.createSpriteFromAtlas(tileset);
                sprites.Add(tileset, slicer.sliceBlockSpritesheet(sprite, tilesetSize));
            }
            return sprites[tileset];
        }

        private void createZoneTiles() {
            Sprite sprite = TexturePacker.createSpriteFromAtlas("zone_tile");
            foreach (ZoneType zoneType in ZoneTypes.all) {
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = sprite;
                tile.color = zoneType.tileColor;
                zoneTiles.Add(zoneType.value, tile);
            }
        }

        private void flushNotFound() {
            foreach (string tileset in notFound.Keys) {
                log("tileset " + tileset + " not found for materials:" + notFound[tileset].ToString());
            }
            notFound.Clear();
        }

        private void log(string message) => logMessage += message + "\n";
    }
}