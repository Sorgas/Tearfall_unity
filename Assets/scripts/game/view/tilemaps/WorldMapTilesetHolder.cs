using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace game.view.tilemaps {
public class WorldMapTilesetHolder {
    private Dictionary<string, Tile> tiles = new();

    private Rect cacheRect = new();
    private const int TILESIZE = 32;

    public WorldMapTilesetHolder() {
        sliceWorldTiles();
    }

    public Tile getTile(string name) {
        return tiles[name];
    }

    private void sliceWorldTiles() {
        Sprite sprite = Resources.Load<Sprite>("tilesets/world_tiles/worldMap");
        tiles.Add("greenPlain", createTile(cutSprite(0, 0, sprite), Color.white));
        tiles.Add("greenForest", createTile(cutSprite(1, 0, sprite), Color.white));
        tiles.Add("mountain", createTile(cutSprite(2, 0, sprite), Color.white));
        tiles.Add("snowPlain", createTile(cutSprite(0, 1, sprite), Color.white));
        tiles.Add("snowForest", createTile(cutSprite(1, 1, sprite), Color.white));
        tiles.Add("snowMountain", createTile(cutSprite(2, 1, sprite), Color.white));
        tiles.Add("swampPlain", createTile(cutSprite(0, 2, sprite), Color.white));
        tiles.Add("swampForest", createTile(cutSprite(1, 2, sprite), Color.white));
        tiles.Add("desert", createTile(cutSprite(0, 3, sprite), Color.white));
        tiles.Add("sea", createTile(cutSprite(0, 4, sprite), Color.white));
        tiles.Add("ocean", createTile(cutSprite(1, 4, sprite), Color.white));
        tiles.Add("river", createTile(cutSprite(0, 5, sprite), Color.white));
        tiles.Add("overlay", createTile(cutSprite(0, 6, sprite), Color.white));
    }
    
    // creates tile by applying color to sprite
    private Tile createTile(Sprite sprite, Color color) {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.color = color;
        return tile;
    }
    
    private Sprite cutSprite(int x, int y, Sprite sprite) {
        Texture2D texture = sprite.texture;
        Rect rect = sprite.rect;
        cacheRect.Set(rect.x * texture.width + x * TILESIZE, rect.y * texture.height + y * TILESIZE, TILESIZE, TILESIZE);
        Sprite tileSprite = Sprite.Create(texture, cacheRect, Vector2.zero, TILESIZE);
        return tileSprite;
    }
}
}