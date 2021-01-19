using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonHandler : MonoBehaviour {
    public class ButtonData {
        public string name;
        public KeyCode hotKey;
        public Action action;
        public Button button;

        public ButtonData(string name, KeyCode hotKey, Action action) : this(hotKey, action) {
            this.name = name;
        }

        public ButtonData(Button button, KeyCode hotKey, Action action) : this(hotKey, action) {
            this.button = button;
        }

        public ButtonData(KeyCode hotKey, Action action) {
            this.hotKey = hotKey;
            this.action = action;
        }
    }

    public List<ButtonData> buttons = new List<ButtonData>();
    private Dictionary<KeyCode, ButtonData> hotkeyMap = new Dictionary<KeyCode, ButtonData>();

    protected abstract void initButtons();

    // Tries to associate button data from subclass with actual child buttons of GO
    public void Start() {
        initButtons(); // fills buttonData list in subclass
        Dictionary<String, Button> foundButtons = transform.GetComponentsInChildren<Button>(true)
            .ToDictionary(button => button.gameObject.name, button => button);
        foreach (String name in foundButtons.Keys) {
            Debug.Log(name);
        }
        buttons.ForEach(data => {
            if (foundButtons.Keys.Contains(data.name)) {
                if (data.button == null) data.button = foundButtons[data.name];
                if (data.button != null) {
                    registerButton(data);
                } else {
                    Debug.Log("Button " + data.name + " not found in " + gameObject.name);
                }
            }
        });
    }

    private void registerButton(ButtonData data) {
        data.button.onClick.AddListener(data.action.Invoke);
        hotkeyMap.Add(data.hotKey, data);
    }

    void Update() {
        foreach (KeyCode key in hotkeyMap.Keys) {
            if (Input.GetKeyDown(key)) hotkeyMap[key].action.Invoke();
        }
    }

    // disables parent GO and enables given GO
    public void switchTo(GameObject value) {
        if (value == null) return;
        gameObject.SetActive(false);
        value.SetActive(true);
    }
}
