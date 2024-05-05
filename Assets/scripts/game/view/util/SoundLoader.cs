using System.Collections.Generic;
using UnityEngine;

namespace game.view.util {
public class SoundLoader {
    public static Dictionary<string, AudioClip> soundMap = new();

    public static AudioClip getSound(string name) {
        if (!soundMap.ContainsKey(name)) {
            soundMap.Add(name, Resources.Load<AudioClip>($"sounds/{name}"));
        }
        return soundMap[name];
    }
}
}