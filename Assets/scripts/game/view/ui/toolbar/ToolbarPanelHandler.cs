using System;
using System.Collections.Generic;
using game.view.ui.util;
using game.view.util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.view.ui.toolbar {
    // holds and manages sub-panels for toolbar panel
    // only one sub-panel can be enabled at once
    // passes input to sub-panels
    public class ToolbarPanelHandler : MonoBehaviour, IHotKeyAcceptor, ICloseable {
        public Action closeAction;
        private Dictionary<KeyCode, Button> buttons = new();
        private Dictionary<KeyCode, Func<bool>> enableFunctions = new();
        protected Dictionary<KeyCode, ToolbarPanelHandler> subPanels = new(); // map to open/close subpanels

        private Dictionary<KeyCode, Action> hotKeyMap = new(); // map to invoke actions

        private ToolbarPanelHandler activeSubpanel;
        private ToolbarPanelHandler parentPanel;
        private int buttonCount;

        public bool accept(KeyCode key) {
            if (activeSubpanel != null) return activeSubpanel.accept(key); // pass to subpanel
            if (buttons.ContainsKey(key)) {
                ExecuteEvents.Execute(buttons[key].gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                return true;
            }
            if (key == KeyCode.Q) {
                // close self
                close();
                return false;
            }
            return false; // not handled
        }

        private void toggleSubpanel(ToolbarPanelHandler panel) {
            if (panel.gameObject.activeSelf) {
                // close if open
                panel.close();
            } else {
                if (activeSubpanel != null) activeSubpanel.close();
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
            if (parentPanel != null) parentPanel.activeSubpanel = null;
        }

        public void open() {
            gameObject.SetActive(true);
            foreach (KeyCode key in buttons.Keys) {
                buttons[key].GetComponentInChildren<Button>().interactable = enableFunctions[key].Invoke();
            }
        }

        public void createButton(string text, string iconName, Action onClick, KeyCode hotKey) =>
            createButton(text, iconName, onClick, () => true, hotKey);

        public void createButton(string text, string iconName, Action onClick, Func<bool> enableFunction, KeyCode hotKey) =>
            createButton(text, IconLoader.get(iconName), onClick, () => true, hotKey);

        public void createButton(string text, Sprite sprite, Action onClick, Func<bool> enableFunction, KeyCode hotKey) {
            GameObject go = PrefabLoader.create("toolbarButton", gameObject.transform);
            float buttonWidth = go.GetComponent<RectTransform>().rect.width;
            go.transform.localPosition = new Vector3(buttonWidth * buttonCount++, 0, 0);
            Button button = go.GetComponentInChildren<Button>();
            button.onClick.AddListener(onClick.Invoke);
            go.GetComponentInChildren<TextMeshProUGUI>().text = text;
            go.GetComponentsInChildren<Image>()[1].sprite = sprite;
            hotKeyMap.Add(hotKey, () => button.onClick.Invoke());
            enableFunctions.Add(hotKey, enableFunction);
            buttons.Add(hotKey, button);
        }

        // creates subpanel, button, registers panel to hotkey
        public ToolbarPanelHandler createSubPanel(string text, string icon, KeyCode hotKey) {
            GameObject panel = PrefabLoader.create("toolbarPanel", gameObject.transform);
            panel.transform.localPosition = new Vector3(0, panel.GetComponent<RectTransform>().rect.height, 0);
            panel.SetActive(false);

            ToolbarPanelHandler handler = panel.GetComponent<ToolbarPanelHandler>();
            handler.parentPanel = this;
            subPanels.Add(hotKey, handler);

            createButton(text, icon, () => toggleSubpanel(handler), hotKey);
            return handler;
        }
    }
}