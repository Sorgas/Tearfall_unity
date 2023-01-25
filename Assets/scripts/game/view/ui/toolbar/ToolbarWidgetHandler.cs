using game.model;
using game.view.system.mouse_tool;
using game.view.tilemaps;
using types;
using types.building;
using UnityEngine;

namespace game.view.ui.toolbar {
    public class ToolbarWidgetHandler : ToolbarPanelHandler {
        private ToolbarPanelHandler openPanel;

        private void Start() {
            new ToolbarInitializer().init(this); 
        }

        public override void close() { } // main toolbar cannot be closed
        
        public void createToolButton(ToolbarPanelHandler panel, string text, DesignationType designation, KeyCode key) =>
            createToolButton(panel, text, designation.iconName, designation, key);

        public void createConstructionButton(ToolbarPanelHandler panel, string text, string iconName, ConstructionType type,
            KeyCode key) {
            panel.createButton(text, "toolbar/constructions/" + iconName, () => MouseToolManager.set(type),
                () => GameModel.get().currentLocalModel.itemContainer.util.enoughForBuilding(type.variants), key);
        }

        public void createBuildingButton(ToolbarPanelHandler panel, string text, BuildingType type, KeyCode key) {
            panel.createButton(text, BuildingTilesetHolder.get().sprites[type].n, () => MouseToolManager.set(type),
                () => GameModel.get().currentLocalModel.itemContainer.util.enoughForBuilding(type.variants), key);
        }

        public void createToolButton(ToolbarPanelHandler panel, string text, string iconName, DesignationType designation,
            KeyCode key) {
            panel.createButton(text, iconName, () => MouseToolManager.set(designation), key);
        }
    }
}