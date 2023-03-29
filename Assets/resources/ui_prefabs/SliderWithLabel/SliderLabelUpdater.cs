using UnityEngine;
using UnityEngine.UI;

namespace resources.ui_prefabs.SliderWithLabel {
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