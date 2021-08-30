using Assets.scripts.generation;
using Assets.scripts.util.lang;
using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.game;
using UnityEngine;

namespace Assets.scripts.game.model {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        public LocalMapTileUpdater tileUpdater;
        public EntitySelectorInputSystem cameraSystem;
        public EcsSystems systems; // systems for updating scene

        public void init(LocalGameRunner initializer) {
            Debug.Log("initializing view");
            initEcs(GenerationState.get().ecsWorld);
            tileUpdater = new LocalMapTileUpdater(initializer.mapHolder);
            cameraSystem = new EntitySelectorInputSystem(initializer);
            cameraSystem.init();
            tileUpdater.flush();

            Debug.Log("view initialized");
        }

        public void update() {
            if(cameraSystem != null) cameraSystem.update();
            systems.Run();
        }

        private void initEcs(EcsWorld ecsWorld) {
            systems = new EcsSystems(ecsWorld);
            systems.Init();
        }
    }
}