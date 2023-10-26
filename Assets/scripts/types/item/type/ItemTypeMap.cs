using System.Collections.Generic;
using System.Linq;
using game.view.tilemaps;
using Newtonsoft.Json;
using UnityEngine;
using util.lang;

namespace types.item.type {
// loads and caches types and sprites for items. 
public class ItemTypeMap : Singleton<ItemTypeMap> {
    private Dictionary<string, ItemType> types = new(); // name -> type
    private Dictionary<string, Sprite> sprites = new();
    public readonly MultiValueDictionary<string, ItemType> toolActionsToTypes = new();

    private const int SIZE = 32;
    private Vector2 pivot = new(0, 0);

    private string logMessage;
    private bool debug = false;

    public ItemTypeMap() {
        loadItemTypes();
        applyBaseTypes();
    }

    public static ItemType getItemType(string name) => get().getType(name);

    public ItemType getType(string name) => hasType(name) ? types[name] : null;

    public bool hasType(string name) => types.ContainsKey(name);

    public List<ItemType> getByTags(List<string> tags) => types.Values.Where(type => type.tags.IsSupersetOf(tags)).ToList();

    public static List<ItemType> getAll() {
        return get().types.Values.ToList();
    }

    private void loadItemTypes() {
        log("Loading item types");
        TextAsset[] files = Resources.LoadAll<TextAsset>("data/items");
        foreach (var file in files) {
            loadFromFile(file);
        }
        if (debug) Debug.Log(logMessage);
    }

    private void loadFromFile(TextAsset file) {
        log("   Loading from " + file.name);
        List<RawItemType> raws = JsonConvert.DeserializeObject<List<RawItemType>>(file.text);
        if (raws != null) {
            for (var i = 0; i < raws.Count; i++) {
                ItemType type = new(raws[i]);
                type.atlasName = file.name;
                // processor.process(raws[i], type);
                types.Add(type.name, type);
                addToolMapping(type);
            }
        } else {
            Debug.LogError($"Can't load item types, {file.name} cannot be parsed");
        }
        log($"   {raws.Count} loaded from {file.name}");
    }

    private void applyBaseTypes() {
        foreach (var type in types.Values) {
            if (type.baseItem != null) {
                ItemType baseType = getType(type.baseItem);
                if (baseType.tags != null) {
                    foreach (var tag in baseType.tags) {
                        Debug.Log($"adding {tag} tag to {type.name}");
                        type.tags.Add(tag);
                    }
                }
            }
        }
    }

    private void addToolMapping(ItemType type) {
        if (type.tool != null) toolActionsToTypes.add(type.tool.action, type);
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
        if (debug) logMessage += message + "\n";
    }
}
}