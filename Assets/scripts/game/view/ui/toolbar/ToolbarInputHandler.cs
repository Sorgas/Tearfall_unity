using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.toolbar {
    public class ToolbarInputHandler : MonoBehaviour {
        public RectTransform ordersPanel;
        public RectTransform buildingsPanel;
        private const string buttonPath = "prefabs/toolbar/toolbarButton";
        private Dictionary<KeyCode, Action> hotKeyMap = new Dictionary<KeyCode, Action>();
        
        private void Start() {
            createButton("C: Orders", () => openOrders(), KeyCode.C, 0);
            createButton("B: Orders", () => openBuildings(), KeyCode.B, 1);
        }

        public void openOrders() {
            
        }

        public void openBuildings() {
            
        }

        private void createButton(string text, Action onClick, KeyCode hotKey, int index) {
            GameObject prefab = Resources.Load<GameObject>(buttonPath);
            float buttonWidth = prefab.GetComponent<RectTransform>().rect.width;
            GameObject button = Instantiate(prefab, gameObject.transform);
            button.transform.localPosition = new Vector3(buttonWidth * index, 0, 0);
            button.GetComponentInChildren<Button>().onClick.AddListener(onClick.Invoke);
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            hotKeyMap.Add(hotKey, onClick);
        }
    }
}