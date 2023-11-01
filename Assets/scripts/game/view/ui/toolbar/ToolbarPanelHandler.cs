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
    // for buttons initialisation see ToolbarWidgetHandler 
    public class ToolbarPanelHandler : MonoBehaviour, IHotKeyAcceptor, ICloseable {
        public Action closeAction;
        private readonly Dictionary<KeyCode, ToolbarPanelChild> children = new();

        private ToolbarPanelHandler activeSubpanel;
        private ToolbarPanelHandler parentPanel;
        private int buttonCount;

        public bool accept(KeyCode key) {
            if (activeSubpanel != null) return activeSubpanel.accept(key); // pass to subpanel
            if (children.ContainsKey(key)) {
                ExecuteEvents.Execute(children[key].handler.button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
            if (key == KeyCode.Q) close();
            return false; // not handled
        }

        public void open() {
            gameObject.SetActive(true);
            foreach (ToolbarPanelChild child in children.Values) {
                child.handler.button.GetComponentInChildren<Button>().interactable = child.enableFunction.Invoke();
            }
            if (parentPanel != null) parentPanel.activeSubpanel = this;
        }

        // closes current and all subpanels recursively
        public virtual void close() {
            if (activeSubpanel != null) activeSubpanel.close();
            closeAction?.Invoke();
            gameObject.SetActive(false);
            highlightButton(KeyCode.None);
            if (parentPanel == null) return;
            parentPanel.activeSubpanel = null;
            parentPanel.highlightButton(KeyCode.None);
        }

        private void toggleSubpanel(KeyCode key) {
            ToolbarPanelHandler panel = children[key].panel;
            if (panel == null) return;
            if (panel.gameObject.activeSelf) {
                panel.close();
            } else {
                if (activeSubpanel != null) activeSubpanel.close();
                panel.open();
            }
        }
        
        public void createButton(string text, Sprite sprite, Action onClick, Func<bool> enableFunction, KeyCode key) => 
            createButton(text, sprite, onClick, enableFunction, key, null, true);

        public void createButton(string text, string icon, Action onClick, KeyCode key) =>
            createButton(text, IconLoader.get(icon), onClick, null, key, null, true);

        private void createButton(string text, string icon, Action onClick, KeyCode key, ToolbarPanelHandler panel) =>
            createButton(text, IconLoader.get(icon), onClick, null, key, panel, true);

        public void createButton(string text, string icon, Action onClick, KeyCode key, bool buttonHighlight) =>
            createButton(text, IconLoader.get(icon), onClick, null, key, null, buttonHighlight);

        private void createButton(string text, Sprite sprite, Action onClick, Func<bool> enableFunction, KeyCode key, ToolbarPanelHandler panel, bool buttonHighlight) {
            enableFunction ??= () => true;
            GameObject go = PrefabLoader.create("toolbarButton", gameObject.transform);
            ToolbarButtonHandler handler = go.GetComponent<ToolbarButtonHandler>();
            handler.init(text, key.ToString(), sprite, onClick);
            float buttonWidth = go.GetComponent<RectTransform>().rect.width;
            go.transform.localPosition = new Vector3(buttonWidth * buttonCount++, 0, 0);
            if (buttonHighlight) handler.button.onClick.AddListener(() => highlightButton(key));
            children.Add(key, new ToolbarPanelChild(panel, handler, enableFunction));
        }

        public ToolbarPanelHandler createSubPanel(string text, string icon, KeyCode key) {
            GameObject go = PrefabLoader.create("toolbarPanel", gameObject.transform);
            go.transform.localPosition = new Vector3(0, go.GetComponent<RectTransform>().rect.height, 0);
            go.SetActive(false);
            ToolbarPanelHandler panel = go.GetComponent<ToolbarPanelHandler>();
            panel.parentPanel = this;
            createButton(text, icon, () => toggleSubpanel(key), key, panel);
            return panel;
        }

        // update highlighting of all button to make only button of key highlighted
        private void highlightButton(KeyCode key) {
            foreach (KeyValuePair<KeyCode, ToolbarPanelChild> pair in children) {
                pair.Value.handler.setHighlighted(pair.Key == key);
            }
        }

        private class ToolbarPanelChild {
            public ToolbarButtonHandler handler;
            public ToolbarPanelHandler panel;
            public Func<bool> enableFunction;

            public ToolbarPanelChild(ToolbarPanelHandler panel, ToolbarButtonHandler handler, Func<bool> enableFunction) {
                this.panel = panel;
                this.handler = handler;
                this.enableFunction = enableFunction;
            }
        }

    }
}