using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace game.model.tilemaps {
    // creates block tiles from textures. name example: "NW", "NW_F"
    class BlockTilesetLoader {
        private const int WIDTH = 64;
        private const int DEPTH = 64;
        private const int WALL = 26;
        private const int FLOOR = 6;
        // public static readonly int WALL_HEIGHT = HEIGHT + FLOOR_HEIGHT;
        private readonly int WALL_HEIGHT = DEPTH + WALL;
        private readonly int FLOOR_HEIGHT = DEPTH + FLOOR;
        public static readonly int TILE_Y_HEIGHT = DEPTH + WALL + FLOOR;
        private readonly int DEPTH2 = DEPTH / 2;
        private readonly string[] suffixes = { "WALL", "ST", "N", "S", "W", "E", "NW", "NE", "SW", "SE", "CNW", "CNE", "CSW", "CSE", "C" };
        private Rect cacheRect = new Rect();

        // returns map of <tilecode -> tile>
        public Dictionary<string, Tile> slice(Sprite sprite) {
            Vector2 wallPivot = new Vector2(0.5f, (DEPTH / 2f) / (WALL_HEIGHT));
            Vector2 floorPivot = new Vector2(0.5f, (FLOOR + (DEPTH / 2f)) / (FLOOR_HEIGHT));
            Rect textureRect = getTextureRect(sprite);
            Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
            
            for (int i = 0; i < suffixes.Length; i++) {
                Tile tile = cutTile(i, FLOOR_HEIGHT, WALL_HEIGHT, wallPivot, sprite);
                tiles.Add(suffixes[i], tile);
            }
            for (int i = 0; i < suffixes.Length; i++) {
                Tile tile = cutTile(i, 0, FLOOR_HEIGHT, floorPivot, sprite);
                tiles.Add(suffixes[i] + "F", tile);
            }
            return tiles;
        }

        private Tile cutTile(int i, int y, int height, Vector2 pivot, Sprite sprite) {
            Texture2D texture = sprite.texture;
            cacheRect.Set(sprite.uv[2].x * texture.width + i * WIDTH, sprite.uv[2].y * texture.height + y, WIDTH, height);
            Sprite tileSprite = Sprite.Create(texture, cacheRect, pivot, 64);
            Tile tile = ScriptableObject.CreateInstance<Tile>() as Tile;
            tile.sprite = tileSprite;
            return tile;
        }

        // gets sprite rect as sprite.textureRect not working
        // works for non-rotated rectangular sprites
        private Rect getTextureRect(Sprite sprite) {
            Rect rect = new Rect(sprite.rect);
            rect.x = sprite.uv[2].x * sprite.texture.width;
            rect.y = sprite.uv[2].y * sprite.texture.height;
            return rect;
        }
    }
}
