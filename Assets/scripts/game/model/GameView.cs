using Assets.scripts.util.lang;
using Tearfall_unity.Assets.scripts.game;
using UnityEngine;

namespace Assets.scripts.game.model {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        public LocalMapTileUpdater tileUpdater;
        private LocalMapCameraSystem cameraSystem;

        public void init(LocalGameRunner initializer) {
            Debug.Log("initing game view");
            tileUpdater = new LocalMapTileUpdater(initializer.mapHolder);
            cameraSystem = new LocalMapCameraSystem(initializer.mainCamera, initializer.selector, initializer.mapHolder);
            tileUpdater.flush();
        }

        public void update() {
            if(cameraSystem != null) cameraSystem.update();
        }
    }
}