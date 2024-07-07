using System.Collections.Generic;
using UnityEngine;

namespace game.view.tilemaps {
// creates block tiles from textures. name example: "NW", "NWF"
// tileset layout is fixed and hardcoded: row of walls, row of floors
class BlockTilesetSlicer {
    private const int WIDTH = 64;
    private const int DEPTH = 64;
    private const int WALL = 26;
    private const int FLOOR = 6;
    private const int WALL_HEIGHT = 90;
    private const int FLOOR_HEIGHT = 70;
    private const int TILESET_HEIGHT = WALL_HEIGHT + FLOOR_HEIGHT;
    private readonly string[] suffixes = { "WALL", "ST", "N", "S", "W", "E", "NW", "NE", "SW", "SE", "CNW", "CNE", "CSW", "CSE", "C" };
    private Rect cacheRect;
    // private readonly Vector2 wallPivot = new(0.5f, DEPTH / 2f / (DEPTH + WALL));
    // private readonly Vector2 floorPivot = new(0.5f, (FLOOR + DEPTH / 2f) / (DEPTH + FLOOR));
    private readonly Vector2 wallPivot = new(0, 0);
    private readonly Vector2 floorPivot = new(0, (float)FLOOR / (DEPTH + FLOOR));

    public Dictionary<string, Sprite> sliceBlockSpritesheet(Sprite sprite) => sliceBlockSpritesheet(sprite, 1);

    public Dictionary<string, Sprite> sliceBlockSpritesheet(Sprite sprite, int count) {
        Dictionary<string, Sprite> sprites = new();
        for (int i = 0; i < count; i++) {
            int y = i * TILESET_HEIGHT;
            string suffix = count != 1 ? i.ToString() : "";
            for (int j = 0; j < suffixes.Length; j++) {
                Sprite wall = cutSprite(j, y + FLOOR_HEIGHT, WALL_HEIGHT, wallPivot, sprite);
                sprites.Add(suffixes[j] + suffix, wall);
                Sprite floor = cutSprite(j, y, FLOOR_HEIGHT, floorPivot, sprite);
                sprites.Add(suffixes[j] + "F" + suffix, floor);
            }
        }
        return sprites;
    }

    private Sprite cutSprite(int i, int y, int height, Vector2 pivot, Sprite sprite) {
        Texture2D texture = sprite.texture;
        Rect rect = sprite.rect;
        cacheRect.Set(rect.x * texture.width + i * WIDTH, rect.y * texture.height + y, WIDTH, height);
        Sprite tileSprite = Sprite.Create(texture, cacheRect, pivot, 64);
        return tileSprite;
    }
}
}