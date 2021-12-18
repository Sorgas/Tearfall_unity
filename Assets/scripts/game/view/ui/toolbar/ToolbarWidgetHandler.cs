using UnityEngine;

namespace game.view.ui.toolbar {
    public class ToolbarWidgetHandler : ToolbarPanelHandler {

        private ToolbarPanelHandler openPanel;
        
        private void Start() {
            createSubPanel("C: Orders", KeyCode.C);
            createSubPanel("B: Buildings", KeyCode.B);
            createSubPanel("Z: Zones", KeyCode.Z);
            fillOrdersPanel(subPanels[KeyCode.C]);
            fillBuildingsPanel(subPanels[KeyCode.B]);
            fillZonesPanel(subPanels[KeyCode.Z]);
        }
        
        private void fillOrdersPanel(ToolbarPanelHandler panel) {
            panel.createButton("order 1", () => Debug.Log("press 1"), KeyCode.A);
            panel.createButton("order 2", () => Debug.Log("press 2"), KeyCode.S);
            panel.createButton("order 3", () => Debug.Log("press 3"), KeyCode.D);
            panel.createButton("order 4", () => Debug.Log("press 4"), KeyCode.F);
        }

        private void fillBuildingsPanel(ToolbarPanelHandler panel) {
            panel.createButton("building 1", () => Debug.Log("press 1"), KeyCode.A);
            panel.createButton("building 2", () => Debug.Log("press 2"), KeyCode.S);
            panel.createButton("building 3", () => Debug.Log("press 3"), KeyCode.D);
            panel.createButton("building 4", () => Debug.Log("press 4"), KeyCode.F);
        }
        
        private void fillZonesPanel(ToolbarPanelHandler panel) {
            panel.createButton("zone 1", () => Debug.Log("press 1"), KeyCode.A);
            panel.createButton("zone 2", () => Debug.Log("press 2"), KeyCode.S);
            panel.createButton("zone 3", () => Debug.Log("press 3"), KeyCode.D);
            panel.createButton("zone 4", () => Debug.Log("press 4"), KeyCode.F);
        }
    }
}