using game.model;
using game.model.localmap;
using game.view;
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
        public RectTransform jobsMenu;
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
            GameModel.get().setWorld(GenerationState.get().world, GenerationState.get().ecsWorld);
        }
        
        private void createTestLocalMap() {
            LocalMap localmap = GameModel.get().localMap;
            if (localmap != null) return;
            GenerationState state = GenerationState.get();
            state.worldGenConfig.size = 10;
            state.generateWorld();
            SettlerData settler = new SettlerData();
            settler.name = "test settler";
            settler.age = 30;
            GenerationState.get().preparationState.settlers.Add(settler);
            state.localGenConfig.location = new IntVector2(5, 5);
            state.generateLocalMap();
        }
    }
}