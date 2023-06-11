using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace game.view.util {
// loads and caches icons. All icons should be in icons/
public class IconLoader : Singleton<IconLoader> {
    private Dictionary<string, Sprite> cache = new();

    public static Sprite get(string name) => get().getSprite(name);

    public Sprite getSprite(string name) {
        if (!cache.ContainsKey(name)) {
            cache.Add(name, Resources.Load<Sprite>("icons/" + name));
        }
        return cache[name];
    }
}
}