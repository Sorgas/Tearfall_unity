using System.Collections.Generic;
using System.Linq;
using game.model;
using game.view.system.mouse_tool;
using game.view.tilemaps;
using game.view.util;
using types;
using types.building;
using UnityEngine;
using util.lang.extension;

namespace game.view.ui.toolbar {
    public class ToolbarWidgetHandler : ToolbarPanelHandler {
        private ToolbarPanelHandler openPanel;
        private HotKeySequence hotKeySequence = new();

        private void Start() {
            createSubPanel("C: Orders", "toolbar/designation", KeyCode.C);
            createSubPanel("V: Constructions", "toolbar/building", KeyCode.V);
            createSubPanel("B: Buildings", "toolbar/building", KeyCode.B);
            createSubPanel("Z: Zones", "toolbar/zone", KeyCode.Z);
            fillOrdersPanel(subPanels[KeyCode.C]);
            fillConstructionsPanel(subPanels[KeyCode.V]);
            fillBuildingsPanel(subPanels[KeyCode.B]);
            fillZonesPanel(subPanels[KeyCode.Z]);
        }

        public override void close() { } // main toolbar cannot be closed

        private void fillOrdersPanel(ToolbarPanelHandler panel) {
            createToolButton(panel, "Z: Chop trees", DesignationTypes.D_CHOP, KeyCode.Z);
            ToolbarPanelHandler diggingPanel = panel.createSubPanel("C: Digging", "toolbar/digging", KeyCode.C);
            diggingPanel.closeAction = () => MouseToolManager.set(DesignationTypes.D_CLEAR);
            createToolButton(diggingPanel, "Z: Dig wall", DesignationTypes.D_DIG, KeyCode.Z);
            createToolButton(diggingPanel, "X: Channel", DesignationTypes.D_CHANNEL, KeyCode.X);
            createToolButton(diggingPanel, "C: Ramp", DesignationTypes.D_RAMP, KeyCode.C);
            createToolButton(diggingPanel, "V: Stairs", DesignationTypes.D_STAIRS, KeyCode.V);
            createToolButton(diggingPanel, "B: Downstairs", DesignationTypes.D_DOWNSTAIRS, KeyCode.B);
            createToolButton(diggingPanel, "N: Clear", DesignationTypes.D_CLEAR, KeyCode.N);
            panel.closeAction = () => MouseToolManager.reset();
        }

        private void fillBuildingsPanel(ToolbarPanelHandler panel) {
            HotKeySequence categorySequence = new();
            Dictionary<string, List<BuildingType>> categoryMap = BuildingTypeMap.get().all().toDictionaryOfLists(type => type.category);
            foreach (KeyValuePair<string, List<BuildingType>> entry in categoryMap) {
                hotKeySequence.reset();
                ToolbarPanelHandler subpanel = panel.createSubPanel(entry.Key, "toolbar/" + entry.Key, categorySequence.getNext());
                foreach (BuildingType type in entry.Value) {
                    createBuildingButton(subpanel, type.name, type, hotKeySequence.getNext()); // TODO use building title instead of name
                }
                subpanel.createButton("rotate", "toolbar/rotate", () => MouseToolManager.get().rotateBuilding(), KeyCode.T);
                subpanel.closeAction = () => MouseToolManager.reset();
            }
        }

        private void fillZonesPanel(ToolbarPanelHandler panel) {
            panel.createButton("zone 1", "toolbar/cancel", () => Debug.Log("press Z 1"), KeyCode.Z);
            panel.createButton("zone 2", "toolbar/cancel", () => Debug.Log("press Z 2"), KeyCode.X);
            panel.createButton("zone 3", "toolbar/cancel", () => Debug.Log("press Z 3"), KeyCode.C);
            panel.createButton("zone 4", "toolbar/cancel", () => Debug.Log("press Z 4"), KeyCode.V);
            panel.closeAction = () => MouseToolManager.reset();
        }

        private void fillConstructionsPanel(ToolbarPanelHandler panel) {
            createConstructionButton(panel, "wall", "wall", ConstructionTypeMap.get("wall"), KeyCode.Z);
            createConstructionButton(panel, "floor", "floor", ConstructionTypeMap.get("floor"), KeyCode.X);
            createConstructionButton(panel, "ramp", "ramp", ConstructionTypeMap.get("ramp"), KeyCode.C);
            createConstructionButton(panel, "stairs", "stairs", ConstructionTypeMap.get("stairs"), KeyCode.V);
            createConstructionButton(panel, "downstairs", "downstairs", ConstructionTypeMap.get("downstairs"), KeyCode.B);
            panel.closeAction = () => MouseToolManager.reset();
        }

        private void createToolButton(ToolbarPanelHandler panel, string text, DesignationType designation, KeyCode key) =>
            createToolButton(panel, text, designation.iconName, designation, key);

        private void createConstructionButton(ToolbarPanelHandler panel, string text, string iconName, ConstructionType type,
            KeyCode key) {
            panel.createButton(text, "toolbar/constructions/" + iconName, () => MouseToolManager.set(type),
                () => GameModel.get().currentLocalModel.itemContainer.util.enoughForBuilding(type.variants), key);
        }

        private void createBuildingButton(ToolbarPanelHandler panel, string text, BuildingType type, KeyCode key) {
            panel.createButton(text, BuildingTilesetHolder.get().sprites[type].n, () => MouseToolManager.set(type),
                () => GameModel.get().currentLocalModel.itemContainer.util.enoughForBuilding(type.variants), key);
        }

        private void createToolButton(ToolbarPanelHandler panel, string text, string iconName, DesignationType designation,
            KeyCode key) {
            panel.createButton(text, iconName, () => MouseToolManager.set(designation), key);
        }
    }
}