using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace mainMenu {
// switches stages of main menu. 
// detects buttons on stage by name, assigns listeners, handles hot key presses
public abstract class StageHandler : MonoBehaviour {
    private readonly Dictionary<KeyCode, ButtonData> hotkeyMap = new();
    protected Dictionary<string, Button> buttons = new();

    // subclasses should fill buttons list
    protected abstract List<ButtonData> getButtonsData();

    // Tries to associate button data from subclass with actual child buttons of GO
    public void Start() {
        Dictionary<string, ButtonData> buttonsData = getButtonsData().ToDictionary(data => data.name, data => data);
        foreach (var button in transform.GetComponentsInChildren<Button>(true)) { // all buttons
            if (buttonsData.ContainsKey(button.gameObject.name)) {
                ButtonData data = buttonsData[button.gameObject.name];
                button.onClick.AddListener(data.action.Invoke);
                buttons.Add(data.name, button);
                hotkeyMap.Add(data.hotKey, data);
            }
        }
    }

    void Update() {
        foreach (KeyCode key in hotkeyMap.Keys) {
            if (Input.GetKeyDown(key)) hotkeyMap[key].action.Invoke();
        }
    }

    // disables this GO and enables given GO
    protected void switchTo(GameObject value) {
        if (value == null) return;
        gameObject.SetActive(false);
        value.SetActive(true);
    }
}
}