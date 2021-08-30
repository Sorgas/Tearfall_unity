using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.mainMenu {
    // class for initiating button menus. searches buttons in hierarchy, inits them with provided data, 
    // assigns them a click handlers, and handles hotkeys presses.
    public abstract partial class ButtonHandler : MonoBehaviour {
        public List<ButtonData> buttons = new List<ButtonData>();
        private Dictionary<KeyCode, ButtonData> hotkeyMap = new Dictionary<KeyCode, ButtonData>();

        // subclasses should fill buttons listHell
        protected abstract void initButtons();

        // Tries to associate button data from subclass with actual child buttons of GO
        public void Start() {
            initButtons(); // fills buttonData list in subclass

            Dictionary<string, Button> foundButtons = new Dictionary<string, Button>();
            foreach (var button in transform.GetComponentsInChildren<Button>(true)) { // all buttons
                if(!foundButtons.ContainsKey(button.name)) foundButtons.Add(button.name, button);
            }
            foreach (string name in foundButtons.Keys) { Debug.Log(name); }
            buttons.ForEach(data => { // registered buttons
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

        private class ButtonComparer : IEqualityComparer<Button> {
            public bool Equals(Button x, Button y) {
                return x == y;
            }

            public int GetHashCode(Button obj) {
                return obj.GetHashCode();
            }
        }
    }
}