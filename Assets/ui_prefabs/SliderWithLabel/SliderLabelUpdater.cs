using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLabelUpdater : MonoBehaviour
{
    public Text text;
    public int magnitude = 1;
    public int value;

    public void updateText(float value) {
        this.value = (int)(value * magnitude);
        text.text = (this.value).ToString();
    }
}
