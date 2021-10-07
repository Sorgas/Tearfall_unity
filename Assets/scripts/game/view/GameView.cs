using game.model.tilemaps;
using game.view.system.unit;
using game.view.with_entity_selector;
using generation;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;

namespace game.view {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        public LocalMapTileUpdater tileUpdater;
        public EntitySelectorInputSystem entitySelectorInputSystem;
        public EntitySelectorVisualMovementSystem entitySelectorVisualMovementSystem;
        public CameraMovementSystem cameraMovementSystem;
        public EcsSystems systems; // systems for updating scene
        public Vector2Int selectorOverlook = new Vector2Int();
        public RectTransform mapHolder;

        public void init(LocalGameRunner initializer) {
            Debug.Log("initializing view");
            mapHolder = initializer.mapHolder;
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
            systems.Add(new UnitVisualSystem());
            systems.Init();
        }
    }
}