using System.Collections.Generic;
using game.input;
using game.view.system.mouse_tool;
using game.view.ui.util;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.unit_menu {
    public class UnitMenuHandler : WindowManagerMenu {
        public const string NAME = "unit";
        public Image portrait;
        // tabs
        public UnitMenuGeneralInfoHandler generalInfoHandler;
        public UnitMenuHealthInfoHandler healthInfoHandler;
        public UnitMenuEquipmentInfoHandler equipmentInfoHandler;
        public UnitSkillsInfoHandler skillsInfoHandler;
        private UnitMenuTab activeTab;
        // buttons
        public Button generalInfoButton;
        public Button healthInfoButton;
        public Button equipmentInfoButton;
        public Button skillsInfoButton;
        private List<UnitMenuTab> tabs = new();

        public EcsEntity unit;
        private bool inited = false;

        public void initFor(EcsEntity unit) {
            if (!inited) init();
            this.unit = unit;
            showPanel(generalInfoHandler);
        }

        public void updateFor(EcsEntity unit) {
            activeTab.initFor(unit);
        }

        private void showPanel(UnitMenuTab panel) {
            foreach (UnitMenuTab tab in tabs) {
                if (tab.gameObject == panel.gameObject) {
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

        private void init() {
            tabs.Add(generalInfoHandler);
            tabs.Add(healthInfoHandler);
            tabs.Add(equipmentInfoHandler);
            tabs.Add(skillsInfoHandler);
            generalInfoButton.onClick.AddListener(() => showPanel(generalInfoHandler));
            healthInfoButton.onClick.AddListener(() => showPanel(healthInfoHandler));
            equipmentInfoButton.onClick.AddListener(() => showPanel(equipmentInfoHandler));
            skillsInfoButton.onClick.AddListener(() => showPanel(skillsInfoHandler));
        }
    }

    public interface IUnitMenuTab {
        public void initFor(EcsEntity unit);
    }
}