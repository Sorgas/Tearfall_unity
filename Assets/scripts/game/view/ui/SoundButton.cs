using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui {
// adds behaviour to play click and hover sounds to button
[RequireComponent(typeof(Button))]
public class SoundButton : MonoBehaviour, IPointerEnterHandler {
    public AudioClip clickSound;
    public AudioClip hoverSound;
    private AudioSource source;

    public void Start() {
        source = !gameObject.hasComponent<AudioSource>()
            ? gameObject.AddComponent<AudioSource>()
            : gameObject.GetComponent<AudioSource>();
        gameObject.GetComponent<Button>().onClick.AddListener(() => source.PlayOneShot(clickSound));
    }

    public void OnPointerEnter(PointerEventData eventData) {
        source.PlayOneShot(hoverSound);
    }
}
}