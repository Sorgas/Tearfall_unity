using game.model;
using game.view.system.mouse_tool;
using game.view.tilemaps;
using game.view.util;
using types;
using types.building;
using UnityEngine;

namespace game.view.ui.toolbar {
    // handler for toolbar's root panel
    public class ToolbarWidgetHandler : ToolbarPanelHandler {
        private ToolbarPanelHandler openPanel;

        private void Start() {
            new ToolbarInitializer().init(this); 
        }
        
        public void createToolButton(ToolbarPanelHandler panel, string text, DesignationType designation, KeyCode key) =>
            createToolButton(panel, text, designation.iconName, designation, key);

        public void createConstructionButton(ToolbarPanelHandler panel, string text, string iconName, ConstructionType type,
            KeyCode key) {
            panel.createButton(text, IconLoader.get("toolbar/constructions/" + iconName), () => MouseToolManager.get().set(type),
                () => GameModel.get().currentLocalModel.itemContainer.util.enoughForBuilding(type.variants), key);
        }

        public void createBuildingButton(ToolbarPanelHandler panel, string text, BuildingType type, KeyCode key) {
            panel.createButton(text, BuildingTilesetHolder.get().sprites[type].n, () => MouseToolManager.get().set(type),
                () => GameModel.get().currentLocalModel.itemContainer.util.enoughForBuilding(type.variants), key);
        }

        public void createToolButton(ToolbarPanelHandler panel, string text, string iconName, DesignationType designation,
            KeyCode key) {
            panel.createButton(text, iconName, () => MouseToolManager.get().set(designation), key);
        }

        public void createZoneButton(ToolbarPanelHandler panel, string text, string iconName, ZoneTypeEnum zoneType, KeyCode key) {
            panel.createButton(text, iconName, () => MouseToolManager.get().set(zoneType), key);
        }

        public void createZoneToolButton(ToolbarPanelHandler panel, string text, string iconName, ZoneMouseToolType zoneType, KeyCode key) {
            panel.createButton(text, iconName, () => MouseToolManager.get().set(zoneType), key);
        }
        
        public override void close() { } // main toolbar cannot be closed
    }
}
