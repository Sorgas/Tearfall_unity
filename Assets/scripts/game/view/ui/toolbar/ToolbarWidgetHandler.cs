using System.Collections.Generic;
using game.view.system.mouse_tool;
using game.view.tilemaps;
using game.view.util;
using types;
using types.building;
using UnityEngine;
using util.lang;
using util.lang.extension;

namespace game.view.ui.toolbar {
// handler for toolbar's root panel
public class ToolbarWidgetHandler : ToolbarPanelHandler, Initable {

    public void init() {
        // first-level buttons with panels
        ToolbarPanelHandler ordersPanel = createSubPanel("C: Orders", "toolbar/designation", KeyCode.C, 
            "toolbar orders panel", MouseToolManager.get().reset);
        ToolbarPanelHandler constructionsPanel = createSubPanel("V: Constructions", "toolbar/building", KeyCode.V, 
            "toolbar constructions panel", MouseToolManager.get().reset);
        ToolbarPanelHandler buildingsPanel = createSubPanel("B: Buildings", "toolbar/building", KeyCode.B, 
            "toolbar buildings panel", MouseToolManager.get().reset);
        ToolbarPanelHandler zonesPanel = createSubPanel("Z: Zones", "toolbar/zone", KeyCode.Z, 
            "toolbar zones panel", MouseToolManager.get().reset);
        fillOrdersPanel(ordersPanel);
        fillConstructionsPanel(constructionsPanel);
        fillBuildingsPanel(buildingsPanel);
        fillZonesPanel(zonesPanel);
        toggleAction = MouseToolManager.get().reset;
    }

    private void fillOrdersPanel(ToolbarPanelHandler panel) {
        createToolButton(panel, "Z: Chop trees", DesignationTypes.D_CHOP, KeyCode.Z);
        createToolButton(panel, "X: Harvest plants", DesignationTypes.D_HARVEST_PLANT, KeyCode.X);

        ToolbarPanelHandler diggingPanel = panel.createSubPanel("C: Digging", "toolbar/digging", KeyCode.C, 
            "toolbar digging panel", MouseToolManager.get().reset);
        createToolButton(diggingPanel, "Z: Dig wall", DesignationTypes.D_DIG, KeyCode.Z);
        createToolButton(diggingPanel, "X: Channel", DesignationTypes.D_CHANNEL, KeyCode.X);
        createToolButton(diggingPanel, "C: Ramp", DesignationTypes.D_RAMP, KeyCode.C);
        createToolButton(diggingPanel, "V: Stairs", DesignationTypes.D_STAIRS, KeyCode.V);
        createToolButton(diggingPanel, "B: Downstairs", DesignationTypes.D_DOWNSTAIRS, KeyCode.B);
        createToolButton(diggingPanel, "N: Clear", DesignationTypes.D_CLEAR, KeyCode.N);
        diggingPanel.createButton("Q: Back", "toolbar/cancel", diggingPanel.close, KeyCode.Q);
        panel.createButton("Q: Back", "toolbar/cancel", panel.close, KeyCode.Q);
    }

    // creates buttons for each building category. creates subpanels with building
    private void fillBuildingsPanel(ToolbarPanelHandler panel) {
        ToolbarHotKeySequence categorySequence = new();
        Dictionary<string, List<BuildingType>> categoryMap = BuildingTypeMap.get().all().toDictionaryOfLists(type => type.category);
        foreach (KeyValuePair<string, List<BuildingType>> entry in categoryMap) {
            ToolbarHotKeySequence hotKeySequence = new();
            ToolbarPanelHandler subpanel = panel.createSubPanel(entry.Key, "toolbar/" + entry.Key, categorySequence.getNext(), 
                $"toolbar {entry.Key} panel", MouseToolManager.get().reset);
            foreach (BuildingType type in entry.Value) {
                createBuildingButton(subpanel, type.name, type, hotKeySequence.getNext()); // TODO use building title instead of name
            }
            subpanel.createButton("T: Rotate", "toolbar/rotate", () => MouseToolManager.get().rotate(), KeyCode.T, false);
            subpanel.createButton("Q: Back", "toolbar/cancel", subpanel.close, KeyCode.Q, false);
        }
    }

    private void fillZonesPanel(ToolbarPanelHandler panel) {
        createZoneButton(panel, "Stockpile", "toolbar/zones/stockpile", ZoneTypeEnum.STOCKPILE, KeyCode.Z);
        createZoneButton(panel, "Farm", "toolbar/zones/farm", ZoneTypeEnum.FARM, KeyCode.X);
        createZoneToolButton(panel, "Update", "toolbar/zones/expand", ZoneMouseToolType.UPDATE, KeyCode.B);
        createZoneToolButton(panel, "Clear", "toolbar/zones/clear", ZoneMouseToolType.DELETE, KeyCode.N);
        panel.createButton("Q: Back", "toolbar/cancel", panel.close, KeyCode.Q);
    }

    private void fillConstructionsPanel(ToolbarPanelHandler panel) {
        createConstructionButton(panel, "wall", "wall", ConstructionTypeMap.get("wall"), KeyCode.Z);
        createConstructionButton(panel, "floor", "floor", ConstructionTypeMap.get("floor"), KeyCode.X);
        createConstructionButton(panel, "ramp", "ramp", ConstructionTypeMap.get("ramp"), KeyCode.C);
        createConstructionButton(panel, "stairs", "stairs", ConstructionTypeMap.get("stairs"), KeyCode.V);
        createConstructionButton(panel, "downstairs", "downstairs", ConstructionTypeMap.get("downstairs"), KeyCode.B);
        createToolButton(panel, "N: Clear", DesignationTypes.D_CLEAR, KeyCode.N);
    }

    private void createToolButton(ToolbarPanelHandler panel, string text, DesignationType designation, KeyCode key) =>
        panel.createButton(text, designation.iconName, () => MouseToolManager.get().set(designation), key);

    public void createConstructionButton(ToolbarPanelHandler panel, string text, string iconName, ConstructionType type,
        KeyCode key) {
        panel.createButton(text, IconLoader.get("toolbar/constructions/" + iconName), 
            () => MouseToolManager.get().set(type), key);
    }

    public void createBuildingButton(ToolbarPanelHandler panel, string text, BuildingType type, KeyCode key) {
        panel.createButton(text, BuildingTilesetHolder.get().get(type, Orientations.N, 0), 
            () => MouseToolManager.get().set(type), key);
    }
    
    public void createZoneButton(ToolbarPanelHandler panel, string text, string iconName, ZoneTypeEnum zoneType, KeyCode key) {
        panel.createButton(text, iconName, () => MouseToolManager.get().set(zoneType), key);
    }

    public void createZoneToolButton(ToolbarPanelHandler panel, string text, string iconName, ZoneMouseToolType zoneType, KeyCode key) {
        panel.createButton(text, iconName, () => MouseToolManager.get().set(zoneType), key);
    }

    // main toolbar cannot be closed
    public override void close() {
        log($"closing {name}");
        if (activeSubpanel != null) activeSubpanel.close();
        toggleAction?.Invoke();
        highlightButton(KeyCode.None);
    } 
}
}