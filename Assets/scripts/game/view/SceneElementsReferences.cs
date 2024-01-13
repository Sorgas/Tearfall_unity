using game.view.ui;
using game.view.ui.debug_tools;
using game.view.ui.gamespeed_widget;
using game.view.ui.jobs_widget;
using game.view.ui.menu_widget;
using game.view.ui.stockpileMenu;
using game.view.ui.toolbar;
using game.view.ui.unit_menu;
using game.view.ui.workbench;
using TMPro;
using UnityEngine;

namespace game.view {
public class SceneElementsReferences : MonoBehaviour {
    public RectTransform mapHolder;
    public Camera mainCamera;
    public RectTransform selector;
    public JobsWindowHandler jobsWindow;
    public MenuWidgetHandler menuWidget;
    public ToolbarWidgetHandler toolbarWidget;
    public MaterialSelectionWidgetHandler materialSelectionWidgetHandler;
    public PrioritySelectionWidgetHandler prioritySelectionWidgetHandler;
    public GamespeedWidgetHandler gamespeedWidgetHandler;
    public WorkbenchWindowHandler workbenchWindowHandler;
    public ItemMenuHandler itemMenuHandler;
    public UnitMenuHandler unitMenuHandler;
    public StockpileMenuHandler stockpileMenuHandler;
    public FarmMenuHandler farmMenuHandler;
    public PlantMenuHandler plantMenuHandler;
    public TextMeshProUGUI modelDebugInfoPanel;
    public DebugToolsHandler debugToolsHandler;
}
}