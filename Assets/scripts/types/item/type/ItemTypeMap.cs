﻿using System.Collections.Generic;
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
        ItemTypeMerger merger = new();
        List<RawItemType> raws = loadRawTypes();
        merger.mergeRawTypes(raws);
        List<ItemType> types = createTypes(raws);
        merger.mergeItemTypes(types);
        addTypesToMap(types);
    }

    public static ItemType getItemType(string name) => get().getType(name);

    public ItemType getType(string name) => hasType(name) ? types[name] : null;

    public bool hasType(string name) => types.ContainsKey(name);

    public List<ItemType> getByTags(List<string> tags) => types.Values.Where(type => type.tags.IsSupersetOf(tags)).ToList();

    public static List<ItemType> getAll() {
        return get().types.Values.ToList();
    }

    private List<RawItemType> loadRawTypes() {
        log("Loading item types");
        TextAsset[] files = Resources.LoadAll<TextAsset>("data/items");
        List<RawItemType> result = new();
        foreach (var file in files) {
            if (debug) log(logMessage);
            log("   Loading from " + file.name);
            List<RawItemType> raws = JsonConvert.DeserializeObject<List<RawItemType>>(file.text);
            if (raws != null) {
                for (var i = 0; i < raws.Count; i++) {
                    raws[i].atlasName = file.name;
                }
            } else {
                Debug.LogError($"Can't load item types, {file.name} cannot be parsed");
            }
            result.AddRange(raws);
            log($"   {raws.Count} loaded from {file.name}");
        }
        return result;
    }

    private List<ItemType> createTypes(List<RawItemType> raws) {
        List<ItemType> result = new();
        for (var i = 0; i < raws.Count; i++) {
            if (raws[i].atlasXY == null && raws[i].spriteName.Equals("defaultItem")) {
                Debug.LogWarning($"Item {raws[i].name} has no atlas tile or sprite specified");
            }
            ItemType type = new(raws[i]);
            result.Add(type);
        }
        return result;
    }

    private void addTypesToMap(List<ItemType> types) {
        foreach (var type in types) {
            // processor.process(raws[i], type);
            this.types.Add(type.name, type);
            addToolMapping(type);
        }
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
                        log($"adding {tag} tag to {type.name}");
                        type.tags.Add(tag);
                    }
                }
            }
        }
    }

    private void addToolMapping(ItemType type) {
        if (type.toolAction != null) toolActionsToTypes.add(type.toolAction, type);
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