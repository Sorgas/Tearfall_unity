using game.model;
using game.view;
using game.view.ui.jobs_widget;
using game.view.ui.menu_widget;
using game.view.ui.toolbar;
using generation;
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
        public Text debugInfoPanel;
        public Text modelDebugInfoPanel;
        private bool started = false;

        // when scene is loaded, inits game model and view
        public void Start() {
            Debug.Log("starting game");
            resolveWorld();
            GameModel.get().init();
            GameView.get().init(this);
            started = true;
        }

        public void Update() {
            if (!started) return;
            GameModel.get().update();
            GameView.get().update();
        }

        // gets world either from worldgen/localgen, savefile, or creates test one
        private void resolveWorld() {
            if (!GenerationState.get().ready) {
                if (true) {
                    new TestLevelInitializer().createTestLocalMap();
                } else {
                    // TODO load save game
                }
            }
        }
    }
}