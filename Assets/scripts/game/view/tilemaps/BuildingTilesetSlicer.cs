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
                createSprite(type, sprite, type.positionN, false),
                createSprite(type, sprite, type.positionS, false),
                createSprite(type, sprite, type.positionE, true),
                createSprite(type, sprite, type.positionW, true));
        }

        private Sprite createSprite(BuildingType type, Sprite sprite, int[] position, bool flip) {
            Texture2D texture = sprite.texture;
            Rect rect = sprite.rect;
            int width = type.size[flip ? 1 : 0];
            int height = type.size[flip ? 0 : 1];
            cacheRect.Set(
                rect.x * texture.width + position[0] * WIDTH, 
                rect.y * texture.height + position[1] * (FLOOR_HEIGHT + WALL_HEIGHT), 
                width * WIDTH,
                height * FLOOR_HEIGHT + WALL_HEIGHT);
            Sprite tileSprite = Sprite.Create(texture, cacheRect, pivot, 64);
            return tileSprite;
        }
    }
}