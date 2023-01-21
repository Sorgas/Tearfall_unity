using System.Collections.Generic;
using game.input;
using game.view.system.mouse_tool;
using game.view.ui.util;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.unit_menu { // TODO generalize item building and unit menus to EntityWindow
    public class UnitMenuHandler : MbWindow, IHotKeyAcceptor {
        public const string NAME = "unit";
        public Image portrait;
        public UnitMenuGeneralInfoHandler generalInfoHandler;
        public UnitMenuHealthInfoHandler healthInfoHandler;
        private UnitMenuTab activeTab;

        public Button generalInfoButton;
        public Button healthInfoButton;
        public List<UnitMenuTab> tabs = new();

        public EcsEntity unit;
        private bool inited = false;

        public void initFor(EcsEntity unit) {
            if(!inited) init();
            this.unit = unit;
            showPanel(generalInfoHandler);
        }

        public void updateFor(EcsEntity unit) {
            activeTab.initFor(unit);
        }

        private void showPanel(UnitMenuTab panel) {
            Debug.Log("showing panel " + panel);
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
            MouseToolManager.reset();
            return true;
        }

        private void init() {
            tabs.Add(generalInfoHandler);
            tabs.Add(healthInfoHandler);
            generalInfoButton.onClick.AddListener(() => showPanel(generalInfoHandler));
            healthInfoButton.onClick.AddListener(() => showPanel(healthInfoHandler));
        }
    }

    public interface IUnitMenuTab {
        public void initFor(EcsEntity unit);
    }
}