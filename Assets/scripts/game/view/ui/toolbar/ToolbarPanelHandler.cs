using System;
using System.Collections.Generic;
using game.view.ui.util;
using game.view.util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace game.view.ui.toolbar {
// holds and manages sub-panels for toolbar panel
// only one sub-panel can be enabled at once
// passes input to sub-panels
// for buttons initialisation see ToolbarWidgetHandler 
public class ToolbarPanelHandler : GameWidget {
    public Action toggleAction;
    private readonly Dictionary<KeyCode, ToolbarPanelChild> children = new();

    protected ToolbarPanelHandler activeSubpanel;
    private ToolbarPanelHandler parentPanel;
    private int buttonCount;
    private string name = "toolbar panel";
    private const bool debug = true;
    
    public override bool accept(KeyCode key) {
        log($"handling {key} in {name}");
        if (activeSubpanel != null) return activeSubpanel.accept(key); // pass to subpanel
        if (children.ContainsKey(key)) {
            ExecuteEvents.Execute(children[key].button.button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
        if (key == KeyCode.Q) close();
        return false; // not handled
    }

    public override string getName() {
        return "toolbar_panel_widget";
    }

    public override void open() {
        gameObject.SetActive(true);
        if (parentPanel != null) parentPanel.activeSubpanel = this;
        toggleAction?.Invoke();
    }

    // closes current and all subpanels recursively
    public new virtual void close() {
        log($"closing {name}");
        if (activeSubpanel != null) activeSubpanel.close();
        toggleAction?.Invoke();
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
    
    public void createButton(string text, Sprite sprite, Action onClick, KeyCode key) => 
        createButton(text, sprite, onClick, key, null, true);

    public void createButton(string text, string icon, Action onClick, KeyCode key) =>
        createButton(text, IconLoader.get(icon), onClick, key, null, true);

    private void createButton(string text, string icon, Action onClick, KeyCode key, ToolbarPanelHandler subPanel) =>
        createButton(text, IconLoader.get(icon), onClick, key, subPanel, true);

    public void createButton(string text, string icon, Action onClick, KeyCode key, bool buttonHighlight) =>
        createButton(text, IconLoader.get(icon), onClick, key, null, buttonHighlight);
    
    // creates button in current panel. Button can open subPanel when pressed.
    private void createButton(string text, Sprite sprite, Action onClick, KeyCode key, ToolbarPanelHandler subPanel, bool buttonHighlight) {
        GameObject go = PrefabLoader.create("toolbarButton", gameObject.transform);
        ToolbarButtonHandler button = go.GetComponent<ToolbarButtonHandler>();
        button.init(text, key.ToString(), sprite, onClick);
        float buttonWidth = go.GetComponent<RectTransform>().rect.width;
        go.transform.localPosition = new Vector3(buttonWidth * buttonCount++, 0, 0);
        if (buttonHighlight) button.button.onClick.AddListener(() => highlightButton(key));
        children.Add(key, new ToolbarPanelChild(subPanel, button));
    }

    // creates subpanel and button which opens it
    public ToolbarPanelHandler createSubPanel(string text, string icon, KeyCode key, string panelName, Action toggleAction) {
        GameObject go = PrefabLoader.create("toolbarPanel", gameObject.transform);
        go.transform.localPosition = new Vector3(0, go.GetComponent<RectTransform>().rect.height, 0);
        go.SetActive(false);
        ToolbarPanelHandler panel = go.GetComponent<ToolbarPanelHandler>();
        panel.name = panelName;
        panel.parentPanel = this;
        panel.toggleAction = toggleAction;
        createButton(text, icon, () => toggleSubpanel(key), key, panel);
        return panel;
    }

    // update highlighting of all button to make only button of key highlighted
    protected void highlightButton(KeyCode key) {
        foreach (KeyValuePair<KeyCode, ToolbarPanelChild> pair in children) {
            pair.Value.button.setHighlighted(pair.Key == key);
        }
    }

    private class ToolbarPanelChild {
        public ToolbarButtonHandler button;
        public ToolbarPanelHandler panel;

        public ToolbarPanelChild(ToolbarPanelHandler panel, ToolbarButtonHandler button) {
            this.panel = panel;
            this.button = button;
        }
    }

    protected void log(string message) {
        if(debug) Debug.Log($"[ToolbarPanelHandler]: {message}");
    }
}
}