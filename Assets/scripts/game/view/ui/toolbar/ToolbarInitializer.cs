using System.Collections.Generic;
using game.view.system.mouse_tool;
using game.view.util;
using types;
using types.building;
using UnityEngine;
using util.lang.extension;

namespace game.view.ui.toolbar {
    // creates button in toolbar on startup
    public class ToolbarInitializer {
        private ToolbarWidgetHandler widget;
        private HotKeySequence hotKeySequence = new();

        public void init(ToolbarWidgetHandler widget) {
            this.widget = widget;
            ToolbarPanelHandler ordersPanel = widget.createSubPanel("C: Orders", "toolbar/designation", KeyCode.C);
            ToolbarPanelHandler constructionsPanel = widget.createSubPanel("V: Constructions", "toolbar/building", KeyCode.V);
            ToolbarPanelHandler buildingsPanel = widget.createSubPanel("B: Buildings", "toolbar/building", KeyCode.B);
            ToolbarPanelHandler zonesPanel = widget.createSubPanel("Z: Zones", "toolbar/zone", KeyCode.Z);
            fillOrdersPanel(ordersPanel);
            fillConstructionsPanel(constructionsPanel);
            fillBuildingsPanel(buildingsPanel);
            fillZonesPanel(zonesPanel);
        }

        private void fillOrdersPanel(ToolbarPanelHandler panel) {
            widget.createToolButton(panel, "Z: Chop trees", DesignationTypes.D_CHOP, KeyCode.Z);
            ToolbarPanelHandler diggingPanel = panel.createSubPanel("C: Digging", "toolbar/digging", KeyCode.C);
            diggingPanel.closeAction = () => MouseToolManager.set(DesignationTypes.D_CLEAR);
            widget.createToolButton(diggingPanel, "Z: Dig wall", DesignationTypes.D_DIG, KeyCode.Z);
            widget.createToolButton(diggingPanel, "X: Channel", DesignationTypes.D_CHANNEL, KeyCode.X);
            widget.createToolButton(diggingPanel, "C: Ramp", DesignationTypes.D_RAMP, KeyCode.C);
            widget.createToolButton(diggingPanel, "V: Stairs", DesignationTypes.D_STAIRS, KeyCode.V);
            widget.createToolButton(diggingPanel, "B: Downstairs", DesignationTypes.D_DOWNSTAIRS, KeyCode.B);
            widget.createToolButton(diggingPanel, "N: Clear", DesignationTypes.D_CLEAR, KeyCode.N);
            panel.closeAction = () => MouseToolManager.reset();
        }

        private void fillBuildingsPanel(ToolbarPanelHandler panel) {
            HotKeySequence categorySequence = new();
            Dictionary<string, List<BuildingType>> categoryMap = BuildingTypeMap.get().all().toDictionaryOfLists(type => type.category);
            foreach (KeyValuePair<string, List<BuildingType>> entry in categoryMap) {
                hotKeySequence.reset();
                ToolbarPanelHandler subpanel = panel.createSubPanel(entry.Key, "toolbar/" + entry.Key, categorySequence.getNext());
                foreach (BuildingType type in entry.Value) {
                    widget.createBuildingButton(subpanel, type.name, type, hotKeySequence.getNext()); // TODO use building title instead of name
                }
                subpanel.createButton("rotate", "toolbar/rotate", () => MouseToolManager.get().rotateBuilding(), KeyCode.T, false);
                subpanel.closeAction = () => MouseToolManager.reset();
            }
        }

        private void fillZonesPanel(ToolbarPanelHandler panel) {
            widget.createZoneButton(panel, "Stockpile", "toolbar/zones/stockpile", ZoneTypeEnum.STOCKPILE, KeyCode.Z);
            widget.createZoneButton(panel, "Farm", "toolbar/zones/farm", ZoneTypeEnum.FARM, KeyCode.X);
            // TODO room zones
            widget.createZoneToolButton(panel, "Update", "toolbar/zones/expand", ZoneMouseToolType.UPDATE, KeyCode.B);
            widget.createZoneToolButton(panel, "Clear", "toolbar/zones/clear", ZoneMouseToolType.DELETE, KeyCode.N);
            panel.closeAction = () => MouseToolManager.reset();
        }

        private void fillConstructionsPanel(ToolbarPanelHandler panel) {
            widget.createConstructionButton(panel, "wall", "wall", ConstructionTypeMap.get("wall"), KeyCode.Z);
            widget.createConstructionButton(panel, "floor", "floor", ConstructionTypeMap.get("floor"), KeyCode.X);
            widget.createConstructionButton(panel, "ramp", "ramp", ConstructionTypeMap.get("ramp"), KeyCode.C);
            widget.createConstructionButton(panel, "stairs", "stairs", ConstructionTypeMap.get("stairs"), KeyCode.V);
            widget.createConstructionButton(panel, "downstairs", "downstairs", ConstructionTypeMap.get("downstairs"), KeyCode.B);
            // TODO add clear button
            panel.closeAction = () => MouseToolManager.reset();
        }
    }
}