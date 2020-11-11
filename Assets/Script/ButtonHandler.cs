using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
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

    public GameObject toCreate;
    public GameObject canvas;
    public int qwer;

    // Start is called before the first frame update
    public void Start() {
        buttons.ForEach(data => {
            Button button= transform.Find(data.name).GetComponent<Button>();
            button.onClick.AddListener(data.action.Invoke);
            hotkeyMap.Add(data.hotKey, data);
        });
    }

    // Update is called once per frame
    void Update() {
        foreach(KeyCode key in hotkeyMap.Keys) {
            if(Input.GetKeyDown(key)) hotkeyMap[key].action.Invoke();
        }
    }
}
