using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.lang;

namespace game.view.tilemaps {
    // point to load and cache all tilesets in game (except icons).
    // makes all tilesets in game to be loaded from same texture
    // packs only textures in /tilesets folder. Names should be unique
    public class TextureLoader : Singleton<TextureLoader>, TextureLoader.ITextureLoader {
        private readonly PackingTextureLoader packer = new();
        private readonly SimpleTextureLoader simple = new();
        private readonly ITextureLoader activeLoader;

        public TextureLoader() {
            activeLoader = simple;
            prepareSprites();
        }

        public void prepareSprites() {
            activeLoader.prepareSprites();
        }

        public Sprite getSprite(string name) => activeLoader.getSprite(name);
        
        // just caches sprites
        private class SimpleTextureLoader : ITextureLoader {
            private readonly Dictionary<string, Sprite> spriteCache = new();

            public void prepareSprites() {
                Sprite[] sprites = Resources.LoadAll<Sprite>("tilesets").ToArray();
                for (var i = 0; i < sprites.Length; i++) {
                    spriteCache[sprites[i].name] = sprites[i];
                }
            }

            public Sprite getSprite(string name) {
                if (!spriteCache.ContainsKey(name)) name = "template";
                return spriteCache[name];
            }
        }

        // combines all textures into single one
        private class PackingTextureLoader : ITextureLoader {
            private Texture2D atlasTexture;
            private readonly Dictionary<string, Rect> rectMap = new();
            private readonly Dictionary<string, Sprite> spriteCache = new();

            public void prepareSprites() {
                string message = "Packing textures to atlas: ";
                Texture2D[] textures = Resources.LoadAll<Texture2D>("tilesets").ToArray();
                atlasTexture = new Texture2D(5000, 5000);
                Rect[] rects = atlasTexture.PackTextures(textures, 0, 5000);
                for (var i = 0; i < textures.Length; i++) {
                    message += textures[i].name + " ";
                    rectMap.Add(textures[i].name, rects[i]);
                }
            }

            public Sprite getSprite(string name) {
                if (!spriteCache.ContainsKey(name)) {
                    if (!rectMap.ContainsKey(name)) {
                        name = "template";
                    } else {
                        spriteCache[name] = Sprite.Create(atlasTexture, rectMap[name], new Vector2());
                    }
                }
                return spriteCache[name];
            }
        }

        public interface ITextureLoader {
            public void prepareSprites();

            public Sprite getSprite(string name);
        }
    }
}