using Assets.scripts.util.lang;
using Tearfall_unity.Assets.scripts.game;
using UnityEngine;

namespace Assets.scripts.game.model {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        public LocalMapTileUpdater tileUpdater;
        public LocalMapCameraInputSystem cameraSystem;

        public void init(LocalGameRunner initializer) {
            Debug.Log("initializing view");
            tileUpdater = new LocalMapTileUpdater(initializer.mapHolder);
            cameraSystem = new LocalMapCameraInputSystem(initializer);
            cameraSystem.init();
            tileUpdater.flush();
            Debug.Log("view initialized");
        }

        public void update() {
            if(cameraSystem != null) cameraSystem.update();
        }
    }
}