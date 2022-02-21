using game.model;
using game.model.localmap;
using game.view;
using game.view.ui.jobs_widget;
using game.view.ui.menu_widget;
using game.view.ui.toolbar;
using generation;
using UnityEngine;
using UnityEngine.UI;
using util.geometry;

namespace game {
    // Entry point from Unity scene to game logic. Creates GameModel and GameView when local map scene is loaded. 
    // Calls update() for game model
    // Generates local map if none is generated (for testing)
    public class LocalGameRunner : MonoBehaviour {
        public RectTransform mapHolder;
        public Camera mainCamera;
        public RectTransform selector;
        public Text text;
        public JobsWindowHandler jobsWindow;
        public MenuWidgetHandler menuWidget;
        public ToolbarWidgetHandler toolbarWidget;
        // public Text debugTextPanel;
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
                    createTestLocalMap();
                } else {
                    // TODO load save game
                }
            }
        }
        
        private void createTestLocalMap() {
            if(GameModel.get().world != null && GameModel.get().world.localMap != null) return;
            GenerationState state = GenerationState.get();
            state.worldGenConfig.size = 10;
            state.generateWorld();
            createTestSettler();
            createTestItem();
            state.localGenConfig.location = new IntVector2(5, 5);
            state.generateLocalMap();
        }

        private void createTestSettler() {
            SettlerData settler = new SettlerData();
            settler.name = "test settler";
            settler.age = 30;
            settler.type = "human";
            GenerationState.get().preparationState.settlers.Add(settler);
        }

        private void createTestItem() {
            ItemData item = new ItemData() {material = "iron", type = "pickaxe", quantity = 1};
            GenerationState.get().preparationState.items.Add(item);
        }
    }
}