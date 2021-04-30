using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation;
using Assets.scripts.util.geometry;
using UnityEngine;
using UnityEngine.UI;

namespace Tearfall_unity.Assets.scripts.game {

    // Creates GameView when local map scene is loaded
    // Calls update() for game model
    // Generates local map if none is generated (for testing)
    public class LocalGameRunner : MonoBehaviour {
        public RectTransform mapHolder;
        public Camera mainCamera;
        public RectTransform selector;
        public Text text;
        // public Text debugTextPanel;

        // when scene is loaded, inits game model and view
        public void Start() {
            Debug.Log("starting game");
            ensureLocalMap();
            GameModel.get().init();
            GameView.get().init(this);
        }

        public void Update() {
            GameModel.get().update();
            GameView.get().update();
        }

        // creates mock local map for testing purposes
        private void ensureLocalMap() {
            LocalMap localmap = GameModel.get().localMap;
            if (localmap != null) return;
            GenerationState state = GenerationState.get();
            state.worldGenConfig.size = 10;
            state.generateWorld();
            state.localGenConfig.location = new IntVector2(5,5);
            GameModel.get().localMap = state.generateLocalMap();
        }
    }
}