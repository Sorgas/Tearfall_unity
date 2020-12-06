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

        public ButtonData(string name, KeyCode hotKey, Action action) {
            this.name = name;
            this.hotKey = hotKey;
            this.action = action;
        }
    }

    public List<ButtonData> buttons = new List<ButtonData>();
    private Dictionary<KeyCode, ButtonData> hotkeyMap = new Dictionary<KeyCode, ButtonData>();

    protected abstract void initButtons();

    // Tries to associate button data from subclass with actual child buttons of GO
    public void Start() {
        initButtons();
        Dictionary<String, Button> foundButtons = transform.GetComponentsInChildren<Button>()
            .ToDictionary(button => button.gameObject.name, button => button);
        buttons.ForEach(data => {
            if (foundButtons.Keys.Contains(data.name)) {
                foundButtons[data.name].onClick.AddListener(data.action.Invoke);
                hotkeyMap.Add(data.hotKey, data);
            } else {
                Debug.Log("Button " + data.name + " not found in " + gameObject.name);
            }
        });
    }

    void Update() {
        foreach(KeyCode key in hotkeyMap.Keys) {
            if(Input.GetKeyDown(key)) hotkeyMap[key].action.Invoke();
        }
    }
}
