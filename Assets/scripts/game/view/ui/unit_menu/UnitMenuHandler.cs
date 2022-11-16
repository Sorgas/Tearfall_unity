using System.Collections.Generic;
using game.view.ui;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

// TODO generalize item building and unit menus to EntityWindow
public class UnitMenuHandler : MbWindow, IHotKeyAcceptor {
    public const string NAME = "unit";
    public Image portrait;
    public UnitMenuGeneralInfoHandler generalInfoHandler;
    public UnitMenuHealthInfoHandler healthInfoHandler;
    private UnitMenuTab activeTab;

    public Button generalInfoButton;
    public Button healthInfoButton;
    public List<UnitMenuTab> tabs;

    public EcsEntity unit;

    public void Start() {
        Debug.Log("starting unit menu");
        tabs.Add(generalInfoHandler);
        tabs.Add(healthInfoHandler);
        generalInfoButton.onClick.AddListener(() => showPanel(generalInfoHandler));
        healthInfoButton.onClick.AddListener(() => showPanel(healthInfoHandler));
    }

    public void initFor(EcsEntity unit) {
        this.unit = unit;
        showPanel(generalInfoHandler);
    }

    public void updateFor(EcsEntity unit) {
        activeTab.initFor(unit);
    }

    private void showPanel(UnitMenuTab panel) {
        // Debug.Log("showing panel");
        foreach(UnitMenuTab tab in tabs) {
            if(tab.gameObject == panel.gameObject) {
                tab.open();
                tab.initFor(unit);
                activeTab = tab;
            } else {
                tab.close();
            }
        }
    }

    public override string getName() {
        return NAME;
    }

    public bool accept(KeyCode key) {
        if(key == KeyCode.Q) WindowManager.get().closeWindow(NAME);
        return true;
    }
}

public interface IUnitMenuTab {
    public void initFor(EcsEntity unit);
}