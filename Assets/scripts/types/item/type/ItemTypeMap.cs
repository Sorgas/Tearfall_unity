using System.Collections.Generic;
using System.Linq;
using game.view.tilemaps;
using Leopotam.Ecs;
using Newtonsoft.Json;
using types.item.type.raw;
using UnityEngine;
using util.lang;

namespace types.item.type {
    // loads and caches types and sprites for items. 
    public class ItemTypeMap : Singleton<ItemTypeMap> {
        private Dictionary<string, ItemType> types = new();
        private Dictionary<string, Sprite> sprites = new();

        private const int SIZE = 32;
        private Vector2 pivot = new(0, 0);

        private string logMessage;
        EcsWorld world = new();

        public ItemTypeMap() {
            loadItemTypes();
        }

        public static ItemType getItemType(string name) {
            return get().types[name];
        }

        public static bool contains(string title) {
            return get().types.ContainsKey(title);
        }

        public static List<ItemType> getAll() {
            return get().types.Values.ToList();
        }
        
        private void loadItemTypes() {
            log("Loading item types");
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/items");
            RawItemTypeProcessor processor = new();
            foreach (var file in files) {
                loadFromFile(file, processor);
            }
            Debug.Log(logMessage);
        }

        private void loadFromFile(TextAsset file, RawItemTypeProcessor processor) {
            log("   Loading from " + file.name);
            List<RawItemType> raws = JsonConvert.DeserializeObject<List<RawItemType>>(file.text);
            if (raws != null) {
                for (var i = 0; i < raws.Count; i++) {
                    ItemType type = new(raws[i]);
                    type.atlasName = file.name;
                    // processor.process(raws[i], type);
                    types.Add(type.name, type);
                }
            } else {
                Debug.LogError("Can't load item types, " + file.name + " cannot be parsed");
            }
            log("   " + raws.Count + " loaded from " + file.name);
        }

        // TODO add material arg to tint sprite with material color
        public Sprite getSprite(string type) {
            if (!sprites.ContainsKey(type)) sprites.Add(type, createSprite(type));
            return sprites[type];
        }

        private Sprite createSprite(string typeName) {
            ItemType type = types[typeName];
            Sprite sprite = TextureLoader.get().getSprite(type.atlasName);
            Texture2D texture = sprite.texture;
            int x = type.atlasXY[0];
            int y = type.atlasXY[1];
            Rect rect = new(x * SIZE, texture.height - (y + 1) * SIZE, SIZE, SIZE);
            return Sprite.Create(texture, rect, pivot, 32);
        }

        private void log(string message) {
            logMessage += message + "\n";
        }
    }
}