using System.Collections.Generic;
using game.view.tilemaps;
using UnityEngine;
using static game.view.util.TilemapLayersConstants;

namespace types.plant {
    public class PlantSpriteMap {
        private Dictionary<string, Sprite[]> sprites = new(); // type name -> sprites
        private const int SIZE_X = 64;
        private const int SIZE_Y = 90;
        private readonly Vector2 pivot = new(0, 0);
        private readonly Vector3 zOffset = new(0, 0, WALL_LAYER * GRID_STEP + GRID_STEP / 2f);

        public void createSprites(PlantType type) {
            Sprite sprite = TextureLoader.get().getSprite(type.atlasName);
            Texture2D texture = sprite.texture;
            int x = type.tileXY[0];
            int y = type.tileXY[1];
            Sprite[] spriteArray = new Sprite[type.tiles];
            for (int i = 0; i < type.tiles; i++) {
                Rect rect = new((x + i) * SIZE_X, texture.height - (y + 1) * SIZE_Y, SIZE_X, SIZE_Y);
                spriteArray[i] = Sprite.Create(texture, rect, pivot, 64);
            }
            sprites.Add(type.name, spriteArray);
        }

        public Sprite getSprite(string type, int number) {
            // Debug.Log(type + " " + number);
            return sprites[type][number];
        }
    }
}