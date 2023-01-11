using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.util {
    public class ProgressBarHandler : MonoBehaviour {
        public TextMeshProUGUI text;
        public Image maskImage;

        public void setValue(float value) {
            maskImage.fillAmount = value;
            text.text = String.Format("{0:P0}", value);
        }
    }
}