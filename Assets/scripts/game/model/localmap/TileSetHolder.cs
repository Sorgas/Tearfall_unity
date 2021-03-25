using System.Collections;
using System.Collections.Generic;
using Assets.scripts.util;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetHolder
{
    public Dictionary<string, Dictionary<string, Tile>> tilesets = new Dictionary<string, Dictionary<string, Tile>>();

    public void loadAll() {
        BlockTilesetLoader loader = new BlockTilesetLoader();

        string path = "tilesets/local_blocks/";
        string name = "template";
        
        foreach (var obj in Resources.LoadAll(".")) {
            Debug.Log(obj);
        }
        tilesets.Add(name, loader.slice(Resources.Load<Texture2D>(path + name)));
    }
}
