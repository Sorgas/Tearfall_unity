using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.mainMenu
{
public class ButtonData {
    public string name;
    public KeyCode hotKey;
    public Action action;
    public Button button;

    public ButtonData(string name, KeyCode hotKey, Action action) : this(hotKey, action) {
        this.name = name;
    }

    public ButtonData(Button button, KeyCode hotKey,    Action action) : this(hotKey, action) {
        this.button = button;
    }

    public ButtonData(KeyCode hotKey, Action action) {
        this.hotKey = hotKey;
        this.action = action;
    }
}
}