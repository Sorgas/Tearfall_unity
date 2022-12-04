using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.lang;

namespace game.view.tilemaps {
    
    // makes all tilesets in game to be loaded from same texture
    public class TexturePacker : Singleton<TexturePacker> {
        private Texture2D atlasTexture;
        private Rect[] rects;
        private Dictionary<string, Rect> rectMap = new();
        private Dictionary<string, Sprite> spriteCache = new();

        public TexturePacker() {
            packTextures();
        }

        public static Sprite createSpriteFromAtlas(string name) => get().createSpriteFromAtlas_(name);

        public Sprite createSpriteFromAtlas_(string name) {
            log("getting sprite " + name);
            if (!rectMap.ContainsKey(name)) name = "template";
            if (!spriteCache.ContainsKey(name)) {
                spriteCache[name] = Sprite.Create(atlasTexture, rectMap[name], new Vector2());
            }
            return spriteCache[name];
        }

        private void packTextures() {
            string message = "Packing textures to atlas: ";
            atlasTexture = new Texture2D(5000, 5000);
            Texture2D[] textures = Resources.LoadAll<Texture2D>("tilesets").ToArray();
            rects = atlasTexture.PackTextures(textures, 0, 5000);
            for (var i = 0; i < textures.Length; i++) {
                message += textures[i].name + " ";
                rectMap.Add(textures[i].name, rects[i]);
            }
        }

        private void log(string message) {
            Debug.Log("[TexturePacker]: " + message);
        }
    }
}