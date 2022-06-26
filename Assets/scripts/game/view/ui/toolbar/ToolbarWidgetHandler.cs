using game.model;
using game.view.system.mouse_tool;
using game.view.tilemaps;
using game.view.util;
using types.building;
using UnityEngine;

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
            createToolButton(panel, "Z: Chop trees", MouseToolTypes.CHOP, KeyCode.Z);
            ToolbarPanelHandler diggingPanel = panel.createSubPanel("C: Digging", "toolbar/digging", KeyCode.C);
            diggingPanel.closeAction = () => MouseToolManager.set(MouseToolTypes.NONE);
            createToolButton(diggingPanel, "Z: Dig wall", MouseToolTypes.DIG, KeyCode.Z);
            createToolButton(diggingPanel, "X: Channel", MouseToolTypes.CHANNEL, KeyCode.X);
            createToolButton(diggingPanel, "C: Ramp", MouseToolTypes.RAMP, KeyCode.C);
            createToolButton(diggingPanel, "V: Stairs", MouseToolTypes.STAIRS, KeyCode.V);
            createToolButton(diggingPanel, "B: Downstairs", MouseToolTypes.DOWNSTAIRS, KeyCode.B);
            createToolButton(diggingPanel, "N: Clear", MouseToolTypes.CLEAR, KeyCode.N);
            panel.closeAction = () => MouseToolManager.set(MouseToolTypes.NONE);
        }

        private void fillBuildingsPanel(ToolbarPanelHandler panel) {
            hotKeySequence.reset();
            foreach (BuildingType type in BuildingTypeMap.get().all()) {
                createBuildingButton(panel, type.name, type, hotKeySequence.getNext()); // TODO use building title instead of name
            }
        }

        private void fillZonesPanel(ToolbarPanelHandler panel) {
            panel.createButton("zone 1", "toolbar/cancel", () => Debug.Log("press Z 1"), KeyCode.Z);
            panel.createButton("zone 2", "toolbar/cancel", () => Debug.Log("press Z 2"), KeyCode.X);
            panel.createButton("zone 3", "toolbar/cancel", () => Debug.Log("press Z 3"), KeyCode.C);
            panel.createButton("zone 4", "toolbar/cancel", () => Debug.Log("press Z 4"), KeyCode.V);
        }

        private void fillConstructionsPanel(ToolbarPanelHandler panel) {
            createConstructionButton(panel, "wall", "wall", ConstructionTypeMap.get("wall"), KeyCode.Z);
            createConstructionButton(panel, "floor", "floor", ConstructionTypeMap.get("floor"), KeyCode.X);
            createConstructionButton(panel, "ramp", "ramp", ConstructionTypeMap.get("ramp"), KeyCode.C);
            createConstructionButton(panel, "stairs", "stairs", ConstructionTypeMap.get("stairs"), KeyCode.V);
            createConstructionButton(panel, "downstairs", "downstairs", ConstructionTypeMap.get("downstairs"), KeyCode.B);
            panel.closeAction = () => MouseToolManager.set(MouseToolTypes.NONE);
        }

        private void createToolButton(ToolbarPanelHandler panel, string text, string iconName, MouseToolType tool, KeyCode key) {
            panel.createButton(text, iconName, () => MouseToolManager.set(tool), key);
        }

        private void createToolButton(ToolbarPanelHandler panel, string text, MouseToolType tool, KeyCode key) =>
            createToolButton(panel, text, tool.iconPath, tool, key);

        private void createConstructionButton(ToolbarPanelHandler panel, string text, string iconName, ConstructionType type,
            KeyCode key) {
            panel.createButton(text, "toolbar/constructions/" + iconName, () => MouseToolManager.set(type),
                () => GameModel.get().itemContainer.util.enoughForConstructionType(type), key);
        }

        private void createBuildingButton(ToolbarPanelHandler panel, string text, BuildingType type, KeyCode key) {
            panel.createButton(text, BuildingTilesetHolder.get().sprites[type].n, () => { }, 
                () => GameModel.get().itemContainer.util.enoughForBuilding(type), key);
        }
    }
}