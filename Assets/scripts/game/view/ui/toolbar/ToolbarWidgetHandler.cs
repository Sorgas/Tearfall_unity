using game.view.system;
using game.view.system.mouse_tool;
using UnityEngine;

namespace game.view.ui.toolbar {
    public class ToolbarWidgetHandler : ToolbarPanelHandler {
        private ToolbarPanelHandler openPanel;

        private void Start() {
            createSubPanel("C: Orders", "toolbar/designation", KeyCode.C);
            createSubPanel("B: Buildings", "toolbar/building", KeyCode.B);
            createSubPanel("Z: Zones", "toolbar/zone", KeyCode.Z);
            fillOrdersPanel(subPanels[KeyCode.C]);
            fillBuildingsPanel(subPanels[KeyCode.B]);
            fillZonesPanel(subPanels[KeyCode.Z]);
        }

        public override void close() { } // main toolbar cannot be closed

        private void fillOrdersPanel(ToolbarPanelHandler panel) {
            createToolButton(panel, "Z: Chop trees", MouseToolEnum.CHOP, KeyCode.Z);
            
            ToolbarPanelHandler diggingPanel = panel.createSubPanel("C: Digging", "toolbar/digging", KeyCode.C);
            diggingPanel.closeAction = () => MouseToolManager.set(MouseToolEnum.NONE);
            createToolButton(diggingPanel, "Z: Dig wall", MouseToolEnum.DIG, KeyCode.Z);
            createToolButton(diggingPanel, "X: Channel", MouseToolEnum.CHANNEL, KeyCode.X);
            createToolButton(diggingPanel, "C: Ramp", MouseToolEnum.RAMP, KeyCode.C);
            createToolButton(diggingPanel, "V: Stairs", MouseToolEnum.STAIRS, KeyCode.V);
            createToolButton(diggingPanel, "B: Downstairs", MouseToolEnum.DOWNSTAIRS, KeyCode.B);
            createToolButton(diggingPanel, "N: Clear", MouseToolEnum.CLEAR, KeyCode.N);
        }

        private void fillBuildingsPanel(ToolbarPanelHandler panel) {
            panel.createButton("building 1", "toolbar/cancel", () => Debug.Log("press B 1"), KeyCode.Z);
            panel.createButton("building 2", "toolbar/cancel", () => Debug.Log("press B 2"), KeyCode.X);
            panel.createButton("building 3", "toolbar/cancel", () => Debug.Log("press B 3"), KeyCode.C);
            panel.createButton("building 4", "toolbar/cancel", () => Debug.Log("press B 4"), KeyCode.V);
        }

        private void fillZonesPanel(ToolbarPanelHandler panel) {
            panel.createButton("zone 1", "toolbar/cancel", () => Debug.Log("press Z 1"), KeyCode.Z);
            panel.createButton("zone 2", "toolbar/cancel", () => Debug.Log("press Z 2"), KeyCode.X);
            panel.createButton("zone 3", "toolbar/cancel", () => Debug.Log("press Z 3"), KeyCode.C);
            panel.createButton("zone 4", "toolbar/cancel", () => Debug.Log("press Z 4"), KeyCode.V);
        }

        private void createToolButton(ToolbarPanelHandler panel, string text, string iconName, MouseToolType tool, KeyCode key) {
            panel.createButton(text, iconName, () => MouseToolManager.set(tool), key);
        }

        private void createToolButton(ToolbarPanelHandler panel, string text, MouseToolType tool, KeyCode key) =>
            createToolButton(panel, text, tool.iconPath, tool, key);
    }
}