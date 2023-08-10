using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.util {
    public class ProgressBarHandler : MonoBehaviour {
        public TextMeshProUGUI text;
        public Image barImage;

        public void setValue(float value) {
            barImage.fillAmount = value;
            text.text = String.Format("{0:P0}", value);
        }
    }
}