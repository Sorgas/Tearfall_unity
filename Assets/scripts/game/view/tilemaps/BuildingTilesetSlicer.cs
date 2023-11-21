using types.building;
using UnityEngine;

namespace game.view.tilemaps {
    public class BuildingTilesetSlicer {
        private const int WIDTH = 64;
        private const int FLOOR_HEIGHT = 64;
        private const int WALL_HEIGHT = 24;
        private Rect cacheRect;
        private readonly Vector2 pivot = new();

        public BuildingSprites slice(BuildingType type, Sprite sprite) {
            return new BuildingSprites(
                createSprites(type, sprite, type.positionN, false),
                createSprites(type, sprite, type.positionS, false),
                createSprites(type, sprite, type.positionE, true),
                createSprites(type, sprite, type.positionW, true));
        }

        private Sprite[] createSprites(BuildingType type, Sprite sprite, int[] position, bool flip) {
            Sprite[] result = new Sprite[type.tileCount];
            int tileWidth = type.tilesetSize;
            int tileHeight = type.tilesetSize + 24 * (type.tilesetSize / 64);
            Texture2D texture = sprite.texture;
            Rect rect = sprite.rect;
            int width = type.size[flip ? 1 : 0];
            int height = type.size[flip ? 0 : 1];
            for (int i = 0; i < type.tileCount; i++) {
                cacheRect.Set(
                    rect.x * texture.width + position[0] * tileWidth + i * width * tileWidth, 
                    rect.y * texture.height + position[1] * tileHeight, 
                    width * tileWidth,
                    height * tileHeight);
                result[i] = Sprite.Create(texture, cacheRect, pivot, 64);
            }
            return result;
        }
    }
}