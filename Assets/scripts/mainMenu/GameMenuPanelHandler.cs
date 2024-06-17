using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace mainMenu {
// switches stages of main menu. 
// detects buttons on stage by name, assigns listeners, handles hot key presses
// TODO rewrite to simplify to public Button fields
public abstract class GameMenuPanelHandler : MonoBehaviour {
    private readonly Dictionary<KeyCode, ButtonData> hotkeyMap = new();
    private readonly Dictionary<KeyCode, Button> hotKeys = new();
    protected Dictionary<string, Button> buttons = new();

    // TODO to be removed
    // subclasses should fill buttons list
    protected virtual List<ButtonData> getButtonsData() {
        return null;
    }

    // TODO to be removed
    // Tries to associate button data from subclass with actual child buttons of GO
    public void Start() {
        List<ButtonData> list = getButtonsData();
        if (list == null) return;
        Dictionary<string, ButtonData> buttonsData = getButtonsData().ToDictionary(data => data.name, data => data);
        foreach (var button in transform.GetComponentsInChildren<Button>(true)) { // all buttons
            if (buttonsData.ContainsKey(button.gameObject.name)) {
                ButtonData data = buttonsData[button.gameObject.name];
                buttons.Add(data.name, button);
                hotkeyMap.Add(data.hotKey, data);
                createButtonListener(button, data.hotKey, data.action);
            }
        }
    }

    protected void createButtonListener(Button button, KeyCode hotKey, Action action) {
        button.onClick.AddListener(action.Invoke);
        hotKeys.Add(hotKey, button);
    }
    
    public void Update() {
        foreach (var key in hotKeys.Keys) {
            if (Input.GetKeyDown(key) && hotKeys[key].IsActive()) {
                hotKeys[key].onClick.Invoke();
                return;
            }
        }
    }

    // disables this GO and enables given GO
    protected void switchTo(GameMenuPanelHandler value) {
        if (value == null) return;
        gameObject.SetActive(false);
        value.gameObject.SetActive(true);
    }
}
}