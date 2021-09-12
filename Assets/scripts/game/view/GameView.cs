using Assets.scripts.generation;
using Assets.scripts.util.lang;
using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.game;
using Tearfall_unity.Assets.scripts.game.view.system.unit;
using UnityEngine;

namespace Assets.scripts.game.model {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        public LocalMapTileUpdater tileUpdater;
        public EntitySelectorInputSystem entitySelectorInputSystem;
        public EntitySelectorVisualMovementSystem entitySelectorVisualMovementSystem;
        public CameraMovementSystem cameraMovementSystem;
        public EcsSystems systems; // systems for updating scene
        public Vector2Int selectorOverlook = new Vector2Int();

        public void init(LocalGameRunner initializer) {
            Debug.Log("initializing view");
            initEcs(GenerationState.get().ecsWorld);
            tileUpdater = new LocalMapTileUpdater(initializer.mapHolder);
            entitySelectorInputSystem = new EntitySelectorInputSystem(initializer);
            entitySelectorInputSystem.init();
            entitySelectorVisualMovementSystem = new EntitySelectorVisualMovementSystem(initializer);
            cameraMovementSystem = new CameraMovementSystem(initializer.mainCamera, initializer.selector);
            tileUpdater.flush();
            Debug.Log("view initialized");
        }

        public void update() {
            if (entitySelectorInputSystem != null) entitySelectorInputSystem.update();
            if (entitySelectorVisualMovementSystem != null) entitySelectorVisualMovementSystem.update();
            if (cameraMovementSystem != null) cameraMovementSystem.update();
            if (systems != null) systems.Run();
        }

        private void initEcs(EcsWorld ecsWorld) {
            systems = new EcsSystems(ecsWorld);
            systems.Add(new MovementSystem())
            .Add(new UnitVisualSystem())
            .Add(new UnitOrientationSystem());
            systems.Init();
        }
    }
}