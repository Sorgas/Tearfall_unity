using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation;
using Assets.scripts.util.geometry;
using UnityEngine;
namespace Tearfall_unity.Assets.scripts.game {

    // Creates GameView when local map scene is loaded
    // Calls update() for game model
    // Generates local map if none is generated (for testing)
    public class LocalGameRunner : MonoBehaviour {
        public RectTransform mapHolder;
        public Camera mainCamera;
        public RectTransform selector;

        public void Start() {
            Debug.Log("starting game");
            ensureLocalMap();
            GameModel.get().init();
            GameView.get().init(this); // TODO wrap into object if new parameters added
        }

        public void Update() {
            GameModel.get().update();
            GameView.get().update();
        }

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