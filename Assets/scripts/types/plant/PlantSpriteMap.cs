using System.Collections.Generic;
using game.view.tilemaps;
using UnityEngine;
using static game.view.util.TilemapLayersConstants;

namespace types.plant {
    public class PlantSpriteMap {
        private Dictionary<string, Sprite[]> sprites = new(); // type name -> sprites
        private readonly Vector2 pivot = new(0, 0);

        public void createSprites(PlantType type) {
            int width = type.atlasSize;
            int height = width == 64 ? 90 : 45;
            Sprite sprite = TextureLoader.get().getSprite(type.atlasName);
            Texture2D texture = sprite.texture;
            int x = type.tileXY[0];
            int y = type.tileXY[1];
            Sprite[] spriteArray = new Sprite[type.tileCount];
            for (int i = 0; i < type.tileCount; i++) {
                Rect rect = new((x + i) * width, texture.height - (y + 1) * height, width, height);
                spriteArray[i] = Sprite.Create(texture, rect, pivot, width);
            }
            sprites.Add(type.name, spriteArray);
        }

        public Sprite getSprite(string type, int number) {
            // Debug.Log(type + " " + number);
            return sprites[type][number];
        }
    }
}