using System.Diagnostics;
using game.model;
using game.view;
using game.view.ui;
using game.view.ui.gamespeed_widget;
using game.view.ui.jobs_widget;
using game.view.ui.menu_widget;
using game.view.ui.stockpileMenu;
using game.view.ui.toolbar;
using game.view.ui.unit_menu;
using game.view.ui.workbench;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace game {
    // Entry point from Unity scene to game logic. Creates GameModel and GameView when local map scene is loaded. 
    // Calls update() for game model
    // Generates local map if none is generated (for testing)
    // Contains links to common scene objects
    // TODO rename
    public class LocalGameRunner : MonoBehaviour {
        public RectTransform mapHolder;
        public Camera mainCamera;
        public RectTransform selector;
        public JobsWindowHandler jobsWindow;
        public MenuWidgetHandler menuWidget;
        public ToolbarWidgetHandler toolbarWidget;
        public MaterialSelectionWidgetHandler materialSelectionWidgetHandler;
        public GamespeedWidgetHandler gamespeedWidgetHandler;
        public WorkbenchWindowHandler workbenchWindowHandler;
        public ItemMenuHandler itemMenuHandler;
        public UnitMenuHandler unitMenuHandler;
        public StockpileMenuHandler stockpileMenuHandler;
        public FarmMenuHandler farmMenuHandler;
        public Text debugInfoPanel;
        public Text modelDebugInfoPanel;

        private bool started = false;
        private string defaultModelName = "main";
        
        // TODO make world and local generation independent from gamemodel singleton
        // when scene is loaded, inits game model and view
        public void Start() {
            Stopwatch stopwatch = new();
            Debug.unityLogger.logEnabled = false;
            System.DateTime.Now.ToString();
            stopwatch.Start();
            resolveWorld();
            long generationTime = stopwatch.ElapsedMilliseconds;
            GameModel.get().init(defaultModelName);
            long modelInitTime = stopwatch.ElapsedMilliseconds - generationTime;
            GameView.get().init(this, GameModel.get().currentLocalModel);
            stopwatch.Stop();
            long viewInitTime = stopwatch.ElapsedMilliseconds - modelInitTime;
            started = true;
            InvokeRepeating("updateModel", 0.2f, GameModelUpdateController.UPDATE_TICK_DELTA);
            Debug.unityLogger.logEnabled = true;
            // Debug.Log("generation: " + generationTime + "\n model: " + modelInitTime + "\n view:" + viewInitTime);
        }

        public void Update() {
            if (!started) return;
            GameView.get().update();
        }

        private void updateModel() => GameModel.get().update();

        // gets world either from worldgen/localgen, savefile, or creates test one
        private void resolveWorld() {
            if(GameModel.get().world == null) {
                if (true) {
                    new TestLevelInitializer().createTestLocalMap();
                    // new TestLevelInitializer2().createTestLocalMap(defaultModelName);
                } else {
                    // TODO load save game
                }
            }
        }
    }
}