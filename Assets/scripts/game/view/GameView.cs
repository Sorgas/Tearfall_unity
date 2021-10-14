using game.model;
using game.model.tilemaps;
using game.view.no_es;
using game.view.system.unit;
using game.view.with_entity_selector;
using generation;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;
using util.lang;
using EntitySelectorVisualMovementSystem = game.view.with_entity_selector.EntitySelectorVisualMovementSystem;

namespace game.view {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        public LocalMapTileUpdater tileUpdater;
        private EntitySelectorInputSystem entitySelectorInputSystem;
        private EntitySelectorVisualMovementSystem entitySelectorVisualMovementSystem;
        public CameraMovementSystem cameraMovementSystem;

        private CameraInputSystem cameraInputSystem;
        public CameraMovementSystem2 cameraMovementSystem2;
        private MouseInputSystem mouseInputSystem;
        public MouseMovementSystem mouseMovementSystem;
        private EcsSystems systems; // systems for updating scene
        public Vector2Int selectorOverlook = new Vector2Int();
        public RectTransform mapHolder;
        public bool useSelector = false;
        private readonly ValueRangeInt zRange = new ValueRangeInt(); // range for current z in model units 
        public int currentZ;

        public void init(LocalGameRunner initializer) {
            Debug.Log("initializing view");
            mapHolder = initializer.mapHolder;
            initEcs(GenerationState.get().ecsWorld);
            tileUpdater = new LocalMapTileUpdater(initializer.mapHolder);
            zRange.set(0, GameModel.get().localMap.zSize - 1);
            if (useSelector) {
                entitySelectorInputSystem = new EntitySelectorInputSystem(initializer);
                entitySelectorInputSystem.init();
                entitySelectorVisualMovementSystem = new EntitySelectorVisualMovementSystem(initializer);
                cameraMovementSystem = new CameraMovementSystem(initializer.mainCamera, initializer.selector);
            } else {
                mouseMovementSystem = new MouseMovementSystem(initializer);
                cameraMovementSystem2 = new CameraMovementSystem2(initializer.mainCamera);
                mouseInputSystem = new MouseInputSystem(initializer);
                cameraInputSystem = new CameraInputSystem();
            }
            tileUpdater.flush();
            Debug.Log("view initialized");
        }

        public void update() {
            if (!useSelector) {
                mouseInputSystem.update();
                mouseMovementSystem.update();
                cameraInputSystem.update();
                cameraMovementSystem2.update();
            }
            if (entitySelectorInputSystem != null) entitySelectorInputSystem.update();
            if (entitySelectorVisualMovementSystem != null) entitySelectorVisualMovementSystem.update();
            if (cameraMovementSystem != null) cameraMovementSystem.update();
            if (cameraInputSystem != null) cameraInputSystem.update();
            if (systems != null) systems.Run();
        }

        private void initEcs(EcsWorld ecsWorld) {
            systems = new EcsSystems(ecsWorld);
            systems.Add(new UnitVisualSystem());
            systems.Init();
        }

        public int changeLayer(int dz) {
            int oldZ = currentZ;
            currentZ = zRange.clamp(currentZ + dz);
            return currentZ - oldZ;
        }
    }
}