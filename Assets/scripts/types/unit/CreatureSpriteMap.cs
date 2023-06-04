using System.Collections.Generic;
using game.view.tilemaps;
using UnityEngine;
using util.lang;

namespace types.unit {
public class CreatureSpriteMap : Singleton<CreatureSpriteMap> {
    public Dictionary<string, Dictionary<string, List<Sprite>>> sprites = new(); // type -> bodypart -> sprites

    private const int SIZE = 64;
    private readonly Vector2 pivot = new(0, 0);

    public Sprite getFor(CreatureType type, string bodyPart, int spriteNumber) {
        if (!sprites.ContainsKey(type.name)) loadType(type);
        return sprites[type.name][bodyPart][spriteNumber];
    }

    private void loadType(CreatureType type) {
        Texture2D texture = TextureLoader.get().getSprite(type.appearance.atlas).texture;
        Dictionary<string, List<Sprite>> map = new();
        map.Add("head", loadBodyPart(type.appearance.head, texture));
        map.Add("body", loadBodyPart(type.appearance.body, texture));
        sprites.Add(type.name, map);
    }

    private List<Sprite> loadBodyPart(int[] partConfig, Texture2D texture) {
        int y = partConfig[0];
        int male = partConfig[1];
        int female = partConfig[2];
        List<Sprite> result = new();
        for (int i = 0; i < male + female; i++) {
            Rect rect = new(i * SIZE, texture.height - (y + 1) * SIZE, SIZE, SIZE);
            result.Add(Sprite.Create(texture, rect, pivot, 64));
        }
        return result;
    }
}
}