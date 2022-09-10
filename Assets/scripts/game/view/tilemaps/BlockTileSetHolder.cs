using System.Collections.Generic;
using System.Linq;
using types.material;
using UnityEngine;
using UnityEngine.Tilemaps;
using util.lang;
using util.lang.extension;

namespace game.view.tilemaps {
    // stores block sprites and tiles
    public class BlockTileSetHolder : Singleton<BlockTileSetHolder> {
        // map of <material -> <tilecode -> tile>>
        public readonly Dictionary<string, Dictionary<string, Tile>> tiles = new();
        public readonly Dictionary<string, Dictionary<string, Sprite>> sprites = new();
        private BlockTilesetSlicer slicer = new();
        Dictionary<string, List<string>> notFound = new();

        public void loadAll() {
            Debug.Log("loading block tilesets");
            // TODO try use this for all sprites and tilesets
            MaterialMap.get().all
                .Where(material => material.tileset != null)
                .ForEach(material => loadMaterialTilesetFromAtlas(material));
            loadTilesetFromAtlas("selection");
            loadTilesetFromAtlas("template");
            flushNotFound();
        }
        
        public Sprite getSprite(string material, string tilecode) {
            if (!tiles.ContainsKey(material)) material = "template";
            return tiles[material][tilecode].sprite;
        }

        private void loadTilesetFromAtlas(string tilesetName) {
            Sprite sprite = TexturePacker.createSpriteFromAtlas(tilesetName);
            Dictionary<string, Sprite> spriteMap = slicer.sliceBlockSpritesheet(sprite);
            Debug.Log("adding " + tilesetName);
            sprites.Add(tilesetName, spriteMap);
            tiles.Add(tilesetName, createTilesFromSprites(spriteMap));
        }

        // looks for sprite of material in atlas. If not present, uses template sprite.
        private void loadMaterialTilesetFromAtlas(Material_ material) {
            Sprite sprite = TexturePacker.createSpriteFromAtlas(material.tileset);
            Dictionary<string, Sprite> spritesMap = slicer.sliceBlockSpritesheet(sprite);
            Debug.Log("adding " + material.name);
            sprites.Add(material.name, spritesMap);
            tiles.Add(material.name, createTilesFromSprites(spritesMap, material.color));
        }

        private void addNotFound(Dictionary<string, List<string>> map, string tileset, string material) {
            if (!map.ContainsKey(tileset)) map.Add(tileset, new List<string>());
            map[tileset].Add(material);
        }

        private Dictionary<string, Tile> createTilesFromSprites(Dictionary<string, Sprite> sprites) =>
            createTilesFromSprites(sprites, Color.white);

        private Dictionary<string, Tile> createTilesFromSprites(Dictionary<string, Sprite> sprites, Color color) {
            Dictionary<string, Tile> tiles = new();
            foreach (string key in sprites.Keys) {
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = sprites[key];
                tile.color = color;
                tiles.Add(key, tile);
            }
            return tiles;
        }

        private void flushNotFound() {
            foreach (string tileset in notFound.Keys) {
                Debug.Log("tileset " + tileset + " not found for materials:" + notFound[tileset].ToString());
            }
            notFound.Clear();
        }
    }
}