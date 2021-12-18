using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.toolbar {
    
    // holds and manages sub-panels for toolbar panel
    // only one sub-panel can be enabled at once
    // passes input to sub-panels
    public class ToolbarPanelHandler : MonoBehaviour, IHotKeyAcceptor, ICloseable {
        protected GameObject buttonPrefab;
        protected GameObject panelPrefab;

        protected Dictionary<KeyCode, ToolbarPanelHandler> subPanels = new Dictionary<KeyCode, ToolbarPanelHandler>();
        private Dictionary<KeyCode, Action> hotKeyMap = new Dictionary<KeyCode, Action>(); // map to invoke actions
        private ToolbarPanelHandler activeSubpanel;
        private int level = 0;
        private int buttonCount = 0;

        public bool accept(KeyCode key) {
            if (activeSubpanel != null) {
                return activeSubpanel.accept(key);
            }
            if (hotKeyMap.ContainsKey(key)) {
                hotKeyMap[key].Invoke();
                return true;
            }
            return false;
        }
        
        private void toggleSubpanel(ToolbarPanelHandler panel) {
            if (panel.gameObject.activeSelf) {
                closePanel(panel);
            } else {
                closeAll();
                panel.gameObject.SetActive(true);
            }
        }
        
        private void closeAll() {
            foreach (var panel in subPanels.Values) {
                closePanel(panel);
            }
        }

        private void closePanel(ToolbarPanelHandler panel) {
            panel.gameObject.SetActive(false);
            panel.close();
        }

        
        public void close() {
            closeAllPanels();
            gameObject.SetActive(false);
        }

        public void open() {
            gameObject.SetActive(true);
        }

        public void closeAllPanels() {
            foreach (var toolbarPanelHandler in subPanels.Values) {
                toolbarPanelHandler.close();
            }
        }

        public void createButton(string text, Action onClick, KeyCode hotKey) {
            loadPrefabs();
            float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
            GameObject button = Instantiate(buttonPrefab, gameObject.transform);
            button.transform.localPosition = new Vector3(buttonWidth * buttonCount++, 0, 0);
            button.GetComponentInChildren<Button>().onClick.AddListener(onClick.Invoke);
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;
            hotKeyMap.Add(hotKey, onClick);
        }
        
        // creates subpanel, button, registers panel to hotkey
        public void createSubPanel(string text, KeyCode hotKey) {
            loadPrefabs();
            GameObject panel = Instantiate(panelPrefab, gameObject.transform);
            var handler = panel.GetComponent<ToolbarPanelHandler>();
            handler.level = level + 1;
            panel.transform.localPosition = new Vector3(0,handler.level * panelPrefab.GetComponent<RectTransform>().rect.height,0);
            subPanels.Add(hotKey, handler);
            createButton(text, () => toggleSubpanel(handler), hotKey);
            panel.SetActive(false);
        }

        private void loadPrefabs() {
            if (buttonPrefab != null) return;
            buttonPrefab = Resources.Load<GameObject>("prefabs/toolbar/toolbarButton");
            panelPrefab = Resources.Load<GameObject>("prefabs/toolbar/toolbarPanel");
        }
    }
}