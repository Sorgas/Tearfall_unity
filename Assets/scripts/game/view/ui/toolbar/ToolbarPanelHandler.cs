using System;
using System.Collections.Generic;
using game.view.util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.toolbar {

    // holds and manages sub-panels for toolbar panel
    // only one sub-panel can be enabled at once
    // passes input to sub-panels
    public class ToolbarPanelHandler : MonoBehaviour, IHotKeyAcceptor, ICloseable {
        protected Dictionary<KeyCode, ToolbarPanelHandler> subPanels = new();
        private Dictionary<KeyCode, Action> hotKeyMap = new(); // map to invoke actions
        private ToolbarPanelHandler activeSubpanel;
        private ToolbarPanelHandler parentPanel;
        public Action closeAction;
        private int buttonCount;
        private int level;

        public bool accept(KeyCode key) {
            if (activeSubpanel != null) { // pass to subpanel
                return activeSubpanel.accept(key);
            }
            if (hotKeyMap.ContainsKey(key)) { // press buttons
                hotKeyMap[key].Invoke();
                return true;
            }
            if (key == KeyCode.Q) { // close self
                close();
                return false;
            }
            return false; // not handled
        }

        private void toggleSubpanel(ToolbarPanelHandler panel) {
            if (panel.gameObject.activeSelf) { // close if open
                panel.close();
            } else {
                if(activeSubpanel != null) activeSubpanel.close();
                panel.open();
                activeSubpanel = panel;
            }
        }

        public virtual void close() {
            foreach (var toolbarPanelHandler in subPanels.Values) {
                toolbarPanelHandler.close();
            }
            activeSubpanel = null;
            closeAction?.Invoke();
            gameObject.SetActive(false);
            if(parentPanel != null) parentPanel.activeSubpanel = null;
        }

        public void open() {
            gameObject.SetActive(true);
        }

        public void createButton(string text, string iconName, Action onClick, KeyCode hotKey) {
            GameObject buttonPrefab = PrefabLoader.get("toolbarButton");
            float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
            GameObject button = Instantiate(buttonPrefab, gameObject.transform);
            button.transform.localPosition = new Vector3(buttonWidth * buttonCount++, 0, 0);
            button.GetComponentInChildren<Button>().onClick.AddListener(onClick.Invoke);
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            Sprite icon = IconLoader.get(iconName);
            button.GetComponentsInChildren<Image>()[1].sprite = icon;
            hotKeyMap.Add(hotKey, () => button.GetComponentInChildren<Button>().onClick.Invoke());
        }

        // creates subpanel, button, registers panel to hotkey
        public ToolbarPanelHandler createSubPanel(string text, string icon, KeyCode hotKey) {
            GameObject panelPrefab = PrefabLoader.get("toolbarPanel");
            GameObject panel = Instantiate(panelPrefab, gameObject.transform);
            var handler = panel.GetComponent<ToolbarPanelHandler>();
            // handler.level = level + 1;
            handler.parentPanel = this;
            // Debug.Log("creating panel " + handler.level + " " + (handler.level * panelPrefab.GetComponent<RectTransform>().rect.height));
            panel.transform.localPosition = new Vector3(0, panelPrefab.GetComponent<RectTransform>().rect.height, 0);
            subPanels.Add(hotKey, handler);
            createButton(text, icon, () => toggleSubpanel(handler), hotKey);
            panel.SetActive(false);
            return handler;
        }
    }
}