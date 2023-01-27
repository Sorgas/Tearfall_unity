using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.lang;

namespace game.view.tilemaps {

    // makes all tilesets in game to be loaded from same texture
    // packs only textures in tilesets folder. Names should be unique
    public class TexturePacker : Singleton<TexturePacker> {
        private Texture2D atlasTexture;
        private Dictionary<string, Rect> rectMap = new();
        private Dictionary<string, Sprite> spriteCache = new();
        private bool pack = false;

        public TexturePacker() {
            packTextures();
        }

        public static Sprite createSpriteFromAtlas(string name) => get().createSpriteFromAtlas_(name);

        public Sprite createSpriteFromAtlas_(string name) {
            if (!spriteCache.ContainsKey(name)) {
                if (pack) {
                    if (!rectMap.ContainsKey(name)) {
                        name = "template";
                    } else {
                        spriteCache[name] = Sprite.Create(atlasTexture, rectMap[name], new Vector2());
                    }
                } else {
                    name = "template";
                }
            }
            return spriteCache[name];
        }

        private void packTextures() {
            if (pack) {
                string message = "Packing textures to atlas: ";
                Texture2D[] textures = Resources.LoadAll<Texture2D>("tilesets").ToArray();
                atlasTexture = new Texture2D(5000, 5000);
                Rect[] rects = atlasTexture.PackTextures(textures, 0, 5000);
                for (var i = 0; i < textures.Length; i++) {
                    message += textures[i].name + " ";
                    rectMap.Add(textures[i].name, rects[i]);
                }
                log(message);
            } else {
                Sprite[] sprites = Resources.LoadAll<Sprite>("tilesets").ToArray();
                for (var i = 0; i < sprites.Length; i++) {
                    spriteCache[sprites[i].name] = sprites[i];
                }
            }
        }

        private void log(string message) {
            Debug.Log("[TexturePacker]: " + message);
        }
    }
}