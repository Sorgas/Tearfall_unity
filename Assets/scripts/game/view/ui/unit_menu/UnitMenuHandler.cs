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

    public Button generalInfoButton;
    public List<UnitMenuTab> tabs;

    private EcsEntity unit;

    public void Start() {
        tabs.Add(generalInfoHandler);
        generalInfoButton.onClick.AddListener(() => {
            showPanel(generalInfoHandler);
        });
    }

    public void initFor(EcsEntity unit) {
        this.unit = unit;
        showPanel(generalInfoHandler);
        generalInfoHandler.initFor(unit);
    }

    private void showPanel(UnitMenuTab panel) {
        foreach(UnitMenuTab tab in tabs) {
            if(tab.gameObject == panel) {
                tab.open();
                tab.initFor(unit);
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