using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static game.view.ui.UiColorsEnum;

namespace game.view.ui.toolbar {
// references components of Toolbar button.
public class ToolbarButtonHandler : MonoBehaviour {
    public TextMeshProUGUI text;
    public TextMeshProUGUI hotKey;
    public Image image;
    public Button button;

    public void init(string text, string hotKey, Sprite sprite, Action buttonAction) {
        this.text.text = text;
        this.hotKey.text = hotKey;
        image.sprite = sprite;
        button.onClick.AddListener(buttonAction.Invoke);
    }

    public void setHighlighted(bool value) {
        ColorBlock block = button.GetComponent<Button>().colors;
        block.normalColor = value ? BUTTON_HIGHLIGHTED : BUTTON_NORMAL;
        button.GetComponent<Button>().colors = block;
    }
}
}