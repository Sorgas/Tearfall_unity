﻿using System.Linq;
using System.Collections.Generic;
using Assets.scripts.util;
using Tearfall_unity.Assets.scripts.enums.material;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class TileSetHolder {
    // map of <material -> <tilecode -> tile>>
    public Dictionary<string, Dictionary<string, Tile>> tilesets = new Dictionary<string, Dictionary<string, Tile>>();
    private BlockTilesetLoader loader = new BlockTilesetLoader();
    Dictionary<string, List<string>> notFound = new Dictionary<string, List<string>>();

    public void loadAll() {
        SpriteAtlas atlas = Resources.Load<SpriteAtlas>("tilesets/local_blocks/blockSpriteAtlas");
        List<Material_> materials = MaterialMap.get().all;
        foreach(Material_ material in materials) {
            if (material.tileset != null) {
                Sprite sprite = getSprite(atlas, material);
                Dictionary<string, Tile> tileset = loader.slice(sprite);
                tileset.Values.ToList<Tile>().ForEach(tile => tile.color = material.color); // update tile colors
                tilesets.Add(material.name, tileset);
            }
        }
        materials.ForEach(material => {
            
        });
        flushNotFound();
        
    }

    private Sprite getSprite(SpriteAtlas atlas, Material_ material) {
        Sprite sprite = atlas.GetSprite(material.tileset);
        if (sprite == null) {
            addNotFound(notFound, material.tileset, material.name);
            sprite = atlas.GetSprite("template");
        }
        Debug.Log("getting sprite " + material.tileset + " " + sprite);
        Debug.Log(sprite.uv[0] + " " + sprite.uv[1] + " " + sprite.uv[2] + " " + sprite.uv[3]);
        Debug.Log(sprite.textureRect);
        return sprite;
    }

    private void addNotFound(Dictionary<string, List<string>> map, string tileset, string material) {
        if (!map.ContainsKey(tileset)) map.Add(tileset, new List<string>());
        map[tileset].Add(material);
    }

    private void flushNotFound() {
        foreach (string tileset in notFound.Keys) {
            Debug.Log("tileset " + tileset + " not found for materials:" + notFound[tileset].ToString());
        }
        notFound.Clear();
    }
}
