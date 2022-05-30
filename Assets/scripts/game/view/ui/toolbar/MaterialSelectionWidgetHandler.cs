using System;
using System.Collections.Generic;
using game.view.util;
using TMPro;
using types.building;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace game.view.ui.toolbar {
    public class MaterialSelectionWidgetHandler : MonoBehaviour {
        private List<GameObject> buttons = new();
        
        public void addMaterials(ConstructionType type) {
            
        }
        
        private int buttonCount;
        
        public virtual void close() {
            gameObject.SetActive(false);
        }

        public void open() {
            gameObject.SetActive(true);
        }

        public void init() {
            createButton("material 1", () => { } );
            createButton("material 2", () => { } );
            createButton("material 3", () => { } );
        }

        public void clear() {
            foreach (GameObject button in buttons) {
                Destroy(button);
            }
            buttons.Clear();

        }
        
        public void createButton(string text, Action onClick) {
            GameObject buttonPrefab = PrefabLoader.get("materialButton");
            float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
            GameObject button = Instantiate(buttonPrefab, gameObject.transform);
            button.transform.localPosition = new Vector3(buttonWidth * buttonCount++, 0, 0);
            button.GetComponentInChildren<Button>().onClick.AddListener(onClick.Invoke);
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            // button.GetComponentsInChildren<Image>()[1].sprite = IconLoader.get(iconName);
            buttons.Add(button);
        }
    }
}