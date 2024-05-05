using UnityEngine;

namespace game.view.util {

public class UnitSoundPlayer : MonoBehaviour {
    public AudioSource source;
    
    public void playSound(string soundName) {
        source.clip = SoundLoader.getSound(soundName);
        source.Play();
    }
}
}