using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarHandler : MonoBehaviour {
    public TextMeshProUGUI text;
    public Image maskImage;

    public void setValue(float value) {
        maskImage.fillAmount = value;
        text.text = String.Format("{0:P0}", value);
    }
}