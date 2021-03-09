using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.scripts.util {
    // creates tiles from textures. 
    class BlockTilesetLoader {
        private const int WIDTH = 64;
        private const int DEPTH = 64;
        private const int HEIGHT = 26;
        private const int FLOOR_HEIGHT = 6;
        private readonly int WALL_HEIGHprepaT = HEIGHT + FLOOR_HEIGHT;
        private readonly int WALL_TILE_HEIGHT = DEPTH + HEIGHT + FLOOR_HEIGHT;
        private readonly int FLOOR_TILE_HEIGHT = DEPTH + FLOOR_HEIGHT;
        private readonly int DEPTH2 = DEPTH / 2;
        private readonly string[] suffixes = {"", "ST", "N", "S", "W", "E", "NW", "NE", "SW", "SE", "CNW", "CNE", "CSW", "CSE", "C"};
        private Rect cacheRect = new Rect();


        public Dictionary<string, Tile> slice(Texture2D texture) {
            Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
            for(int i = 0; i < suffixes.Length; i++) {
                Tile tile = cutTile(i, FLOOR_TILE_HEIGHT, WALL_TILE_HEIGHT, new Vector2(0.5f, DEPTH2 / WALL_TILE_HEIGHT), texture);
                tiles.Add(texture.name + "_" + suffixes[i], tile);
            }
            for(int i = 0; i < suffixes.Length; i++) {
                Tile tile = cutTile(i, 0, FLOOR_TILE_HEIGHT, new Vector2(0.5f, (FLOOR_TILE_HEIGHT - DEPTH2) / FLOOR_TILE_HEIGHT), texture);
                tiles.Add(texture.name + "_" +  suffixes[i] + "F", tile);
            }
            return tiles;
        }

        private Tile cutTile(int i, int y, int height, Vector2 pivot, Texture2D texture) {
            cacheRect.Set(i * WIDTH, y, WIDTH, height);
            Sprite sprite = Sprite.Create(texture, cacheRect, pivot);
            Tile tile = ScriptableObject.CreateInstance<Tile>() as Tile;
            tile.sprite = sprite;
            return tile;
        }
    }
}
