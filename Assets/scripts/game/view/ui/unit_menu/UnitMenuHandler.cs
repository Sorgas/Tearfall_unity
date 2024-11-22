using System.Collections.Generic;
using game.view.system.mouse_tool;
using game.view.ui.util;
using Leopotam.Ecs;
using UnityEngine.UI;

namespace game.view.ui.unit_menu {
// Handler for unit menu. Switches tabs of menu, passes update 
    public class UnitMenuHandler : GameWindow {
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

        public void Start() {
            tabs.Add(generalInfoHandler);
            tabs.Add(healthInfoHandler);
            tabs.Add(equipmentInfoHandler);
            tabs.Add(skillsInfoHandler);
            generalInfoButton.onClick.AddListener(() => showPanel(generalInfoHandler));
            healthInfoButton.onClick.AddListener(() => showPanel(healthInfoHandler));
            equipmentInfoButton.onClick.AddListener(() => showPanel(equipmentInfoHandler));
            skillsInfoButton.onClick.AddListener(() => showPanel(skillsInfoHandler));
        }

        public void showUnit(EcsEntity unit) {
            this.unit = unit;
            showPanel(generalInfoHandler);
        }

        private void showPanel(UnitMenuTab panel) {
            foreach (UnitMenuTab tab in tabs) {
                if (tab.gameObject == panel.gameObject) {
                    panel.showUnit(unit);
                    tab.open();
                    activeTab = tab;
                } else {
                    tab.close();
                }
            }
        }

        public override string getName() {
            return NAME;
        }

        public override void close() {
            base.close();
            MouseToolManager.get().reset();
        }
    }
}