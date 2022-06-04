using System.Collections.Generic;
using System.Linq;
using enums.material;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using util.lang;
using util.lang.extension;
using util.TexturePacker.AssetPacker;

namespace game.view.tilemaps {
    
    // stores block sprites and tiles
    public class TileSetHolder : Singleton<TileSetHolder> {
        // map of <material -> <tilecode -> tile>>
        public Dictionary<string, Dictionary<string, Tile>> tiles = new();
        public Dictionary<string, Dictionary<string, Sprite>> sprites = new();
        private BlockTilesetLoader loader = new();
        Dictionary<string, List<string>> notFound = new();

        public void loadAll() {
            // TODO try use this for all sprites and tilesets
            // AssetPacker packer = new();
            // packer.AddTexturesToPack();
            // packer.Process();
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>("tilesets/local_blocks/blockSpriteAtlas");
            MaterialMap.get().all
                .Where(material => material.tileset != null)
                .ForEach(material => loadMaterialTilesetFromAtlas(material, atlas));
            loadTilesetFromAtlas("selection", atlas);
            loadTilesetFromAtlas("template", atlas);
            flushNotFound();
        }

        public Sprite getSprite(string material, string tilecode) => tiles[material][tilecode].sprite;

        private void loadTilesetFromAtlas(string tilesetName, SpriteAtlas atlas) {
            Sprite selection = atlas.GetSprite(tilesetName);
            Dictionary<string, Sprite> spriteMap = loader.sliceBlockSpritesheet(selection);
            sprites.Add(tilesetName, spriteMap);
            tiles.Add(tilesetName, createTilesFromSprites(spriteMap));
        }

        // looks for sprite of material in atlas. If not present, uses template sprite.
        private void loadMaterialTilesetFromAtlas(Material_ material, SpriteAtlas atlas) {
            Sprite sprite = atlas.GetSprite(material.tileset);
            if (sprite == null) {
                addNotFound(notFound, material.tileset, material.name);
                sprite = atlas.GetSprite("template");
            }
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
        
        private void flushNotFound() {
            foreach (string tileset in notFound.Keys) {
                Debug.Log("tileset " + tileset + " not found for materials:" + notFound[tileset].ToString());
            }
            notFound.Clear();
        }
    }
}
