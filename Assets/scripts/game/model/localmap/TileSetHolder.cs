using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Assets.scripts.util;
using Tearfall_unity.Assets.scripts.enums.material;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetHolder {
    // map of <material -> <tilecode -> tile>>
    public Dictionary<string, Dictionary<string, Tile>> tilesets = new Dictionary<string, Dictionary<string, Tile>>();

    public void loadAll() {
        BlockTilesetLoader loader = new BlockTilesetLoader();

        string path = "tilesets/local_blocks/";
        Texture2D[] blockTilesets = Resources.LoadAll<Texture2D>(path);
        List<Tearfall_unity.Assets.scripts.enums.material.Material> materials = MaterialMap.get().all;
        Dictionary<string, List<string>> notFound = new Dictionary<string, List<string>>();
        materials.ForEach(material => {
            if(material.tileset != null) {
                Texture2D texture = Resources.Load<Texture2D>(path + material.tileset);
                if(texture == null) {
                    addNotFound(notFound, material.tileset, material.name);
                    texture = Resources.Load<Texture2D>(path + "template");
                }
                Dictionary<string, Tile> tileset = loader.slice(texture);
                tileset.Values.ToList<Tile>().ForEach(tile => tile.color = material.color); // update tile colors
                tilesets.Add(material.name, tileset);
            }
        });
        foreach (string tileset in notFound.Keys) {
            Debug.Log("tileset " + tileset + " not found for materials:" + notFound[tileset]);
        }
    }

    private void addNotFound(Dictionary<string, List<string>> map, string tileset, string material) {
        if(!map.ContainsKey(tileset)) map.Add(tileset, new List<string>());
        map[tileset].Add(material);
    }
}
