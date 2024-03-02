using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace game.view.tilemaps {
public class RoomTilesetHolder : Singleton<RoomTilesetHolder> {
    private readonly Dictionary<string, Sprite> sprites = new();
    
    protected override void init() {
        Sprite[] array = Resources.LoadAll<Sprite>("tilesets/rooms/roomTiles");
        foreach (var sprite in array) {
            sprites.Add(sprite.name, sprite);
        }
        Debug.Log("room tileset inited");
    }

    public Sprite getSprite(string name) {
        return sprites[name];
    }
}
}