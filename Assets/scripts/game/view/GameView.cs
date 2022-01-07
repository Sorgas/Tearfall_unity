using enums;
using game.model;
using game.model.tilemaps;
using game.view.camera;
using game.view.system;
using game.view.system.item;
using game.view.system.unit;
using generation;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;
using util.lang;

namespace game.view {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        private KeyInputSystem keyInputSystem = KeyInputSystem.get();
        public CameraAndMouseHandler cameraAndMouseHandler;
        public LocalMapTileUpdater tileUpdater;
        public RectTransform selector;
        
        private EcsSystems systems; // systems for updating scene
        public Vector2Int selectorOverlook = new Vector2Int();
        public RectTransform mapHolder;
        private readonly ValueRangeInt zRange = new ValueRangeInt(); // range for current z in model units 
        public int currentZ;

        public void init(LocalGameRunner initializer) {
            Debug.Log("initializing view");
            mapHolder = initializer.mapHolder;
            selector = initializer.selector;
            keyInputSystem.windowManager.addWindow(initializer.jobsWindow, KeyCode.J);
            keyInputSystem.widgetManager.addWidget(initializer.menuWidget);
            keyInputSystem.widgetManager.addWidget(initializer.toolbarWidget);
            
            initEcs(GenerationState.get().ecsWorld);
            tileUpdater = new LocalMapTileUpdater(initializer.mapHolder);
            cameraAndMouseHandler = new CameraAndMouseHandler(initializer);
            cameraAndMouseHandler.init();
            zRange.set(0, GameModel.localMap.zSize - 1);
            resetCameraPosition();
            tileUpdater.flush();
            Debug.Log("view initialized");
        }

        public void update() {
            keyInputSystem?.update();
            cameraAndMouseHandler?.update();
            systems?.Run();
        }

        private void initEcs(EcsWorld ecsWorld) {
            systems = new EcsSystems(ecsWorld);
            systems.Add(new UnitVisualSystem());
            systems.Add(new ItemVisualSystem());
            systems.Add(new DesignationVisualSystem());
            systems.Init();
        }

        public int changeLayer(int dz) => setLayer(currentZ + dz);

        public int setLayer(int z) {
            int oldZ = currentZ;
            currentZ = zRange.clamp(z);
            return currentZ - oldZ;
        }
        
        private void resetCameraPosition() {
            Vector3Int cameraPosition = new Vector3Int(GameModel.localMap.xSize / 2, GameModel.localMap.ySize / 2, 0);
            for (int z = GameModel.localMap.zSize - 1; z >=0 ; z--) {
                if (GameModel.localMap.blockType.get(cameraPosition.x, cameraPosition.y, z) != BlockTypeEnum.SPACE.CODE) {
                    cameraPosition.z = z;
                    break;
                }
            }
            cameraAndMouseHandler.cameraMovementSystem.setCameraTarget(cameraPosition);
            cameraAndMouseHandler.mouseMovementSystem.setTargetModel(cameraPosition);
        }
    }
}