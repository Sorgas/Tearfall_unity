using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace mainMenu.ui {
public class SliderWithLabelHandler : MonoBehaviour {
    public TextMeshProUGUI text;
    public Slider slider;
    public int magnitude = 1;
    private int value;

    public void Start() {
        updateText(slider.value);
    }
    
    public void updateText(float newValue) {
        value = (int)(newValue * magnitude);
        text.text = value.ToString();
    }
}
}