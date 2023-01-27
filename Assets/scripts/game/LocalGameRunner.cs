using game.model;
using game.view;
using game.view.ui;
using game.view.ui.gamespeed_widget;
using game.view.ui.jobs_widget;
using game.view.ui.menu_widget;
using game.view.ui.toolbar;
using game.view.ui.unit_menu;
using game.view.ui.workbench;
using UnityEngine;
using UnityEngine.UI;

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
        public Text debugInfoPanel;
        public Text modelDebugInfoPanel;

        private bool started = false;
        private string defaultModelName = "main";
        
        // TODO make world and local generation independent from gamemodel singleton
        // when scene is loaded, inits game model and view
        public void Start() {
            Debug.unityLogger.logEnabled = false;
            Debug.Log("starting game");
            resolveWorld();
            GameModel.get().init(defaultModelName);
            GameView.get().init(this, GameModel.get().currentLocalModel);
            started = true;
            InvokeRepeating("updateModel", 0, GameModelUpdateController.updateTickDelta);
            Debug.unityLogger.logEnabled = true;
        }

        public void Update() {
            if (!started) return;
            GameView.get().update();
        }

        private void updateModel() {
            GameModel.get().updateController.update(GameModelUpdateController.updateTickDelta);
        }

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