using UnityEngine;
using UnityEngine.UI;

namespace mainMenu.ui {
public class SliderLabelUpdater : MonoBehaviour {
    public Text text;
    public int magnitude = 1;
    public int value;

    public void updateText(float value) {
        this.value = (int)(value * magnitude);
        text.text = (this.value).ToString();
    }
}
}