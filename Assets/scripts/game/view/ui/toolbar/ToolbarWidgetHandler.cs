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
            ToolbarPanelHandler diggingPanel = panel.createSubPanel("C: Digging", "toolbar/digging", KeyCode.C);
            diggingPanel.closeAction = () => MouseToolManager.set(MouseToolEnum.NONE);
            createDigButton(diggingPanel, "Z: Dig wall", "orders/dig", MouseToolEnum.DIG, KeyCode.Z);
            createDigButton(diggingPanel, "X: Channel", "orders/channel", MouseToolEnum.CHANNEL, KeyCode.X);
            createDigButton(diggingPanel, "C: Ramp", "orders/ramp", MouseToolEnum.RAMP, KeyCode.C);
            createDigButton(diggingPanel, "V: Stairs", "orders/stairs", MouseToolEnum.STAIRS, KeyCode.V);
            createDigButton(diggingPanel, "B: Downstairs", "orders/downstairs", MouseToolEnum.DOWNSTAIRS, KeyCode.B);
            createDigButton(diggingPanel, "N: Clear", "orders/cancel", MouseToolEnum.CLEAR, KeyCode.N);

            panel.createButton("order 1", "toolbar/cancel", () => Debug.Log("press C 1"), KeyCode.Z);
            panel.createButton("order 2", "toolbar/cancel", () => Debug.Log("press C 2"), KeyCode.X);
            panel.createButton("order 4", "toolbar/cancel", () => Debug.Log("press C 4"), KeyCode.V);
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

        private void createDigButton(ToolbarPanelHandler panel, string text, string iconName, MouseToolType tool, KeyCode key) {
            panel.createButton(text, iconName, () => MouseToolManager.set(tool), key);
        }
    }
}