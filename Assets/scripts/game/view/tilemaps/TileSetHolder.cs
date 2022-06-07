using System.Collections.Generic;
using System.Linq;
using types.material;
using UnityEngine;
using UnityEngine.Tilemaps;
using util.lang;
using util.lang.extension;

namespace game.view.tilemaps {
    // stores block sprites and tiles
    public class TileSetHolder : Singleton<TileSetHolder> {
        private Texture2D atlasTexture;
        private Rect[] rects;
        private Dictionary<string, Rect> rectMap = new();
        
        // map of <material -> <tilecode -> tile>>
        public Dictionary<string, Dictionary<string, Tile>> tiles = new();
        public Dictionary<string, Dictionary<string, Sprite>> sprites = new();
        private BlockTilesetLoader loader = new();
        Dictionary<string, List<string>> notFound = new();

        public void loadAll() {
            // TODO try use this for all sprites and tilesets
            packTextures();
            MaterialMap.get().all
                .Where(material => material.tileset != null)
                .ForEach(material => loadMaterialTilesetFromAtlas(material));
            loadTilesetFromAtlas("selection");
            loadTilesetFromAtlas("template");
            flushNotFound();
        }

        private void packTextures() {
            atlasTexture = new Texture2D(5000, 5000);
            Texture2D[] textures = Resources.LoadAll<Texture2D>("tilesets").ToArray();
            rects = atlasTexture.PackTextures(textures, 0, 5000);
            for (var i = 0; i < textures.Length; i++) {
                rectMap.Add(textures[i].name, rects[i]);
            }
        }

        public Sprite getSprite(string material, string tilecode) => tiles[material][tilecode].sprite;

        private void loadTilesetFromAtlas(string tilesetName) {
            Sprite sprite = createSpriteFromAtlas(tilesetName);
            Dictionary<string, Sprite> spriteMap = loader.sliceBlockSpritesheet(sprite);
            sprites.Add(tilesetName, spriteMap);
            tiles.Add(tilesetName, createTilesFromSprites(spriteMap));
        }

        // looks for sprite of material in atlas. If not present, uses template sprite.
        private void loadMaterialTilesetFromAtlas(Material_ material) {
            Sprite sprite = createSpriteFromAtlas(material.tileset);
            Dictionary<string, Sprite> spritesMap = loader.sliceBlockSpritesheet(sprite);
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

        private Sprite createSpriteFromAtlas(string name) {
            if (!rectMap.ContainsKey(name)) name = "template";
            return Sprite.Create(atlasTexture, rectMap[name], new Vector2());
        }
        
        private void flushNotFound() {
            foreach (string tileset in notFound.Keys) {
                Debug.Log("tileset " + tileset + " not found for materials:" + notFound[tileset].ToString());
            }
            notFound.Clear();
        }
    }
}