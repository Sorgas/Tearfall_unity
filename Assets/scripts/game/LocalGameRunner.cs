using game.model;
using game.view;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace game {
    // Entry point from Unity scene to game logic. Creates GameModel and GameView when local map scene is loaded. 
    // Calls update() for game view
    // Generates local map if none is generated (for testing)
    // TODO rename
    public class LocalGameRunner : MonoBehaviour {
        public SceneElementsReferences sceneElementsReferences;
        private const string defaultModelName = "main";

        // TODO make world and local generation independent from gamemodel singleton
        // when scene is loaded, inits game model and view
        public void Start() {
            Debug.unityLogger.logEnabled = true;
            Application.targetFrameRate = 60;
            resolveWorld(); // can generate world
            GameModel.get().init(defaultModelName); // create and init game model
            GameView.get().init(this, GameModel.get().currentLocalModel);
            // InvokeRepeating(nameof(updateModel), 0.2f, GameModelUpdateController.UPDATE_TICK_DELTA);
            Debug.unityLogger.logEnabled = true;
        }

        public void Update() {
            GameView.get().update();
            GameModel.get().update();
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