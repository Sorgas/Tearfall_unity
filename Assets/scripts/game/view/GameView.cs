using enums;
using game.model;
using game.view.camera;
using game.view.system;
using game.view.system.item;
using game.view.system.plant;
using game.view.system.unit;
using game.view.tilemaps;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;
using util.lang;

namespace game.view {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        public LocalGameRunner sceneObjectsContainer;
        private KeyInputSystem keyInputSystem = KeyInputSystem.get();
        public CameraAndMouseHandler cameraAndMouseHandler;
        public LocalMapTileUpdater tileUpdater;
        private EcsSystems systems; // systems for updating scene
        private readonly ValueRangeInt zRange = new(); // range for current z in model units
        public int currentZ;
        
        public Vector2Int selectorOverlook = new(); // used for navigating with entity selector

        public void init(LocalGameRunner sceneObjectsContainer) {
            Debug.Log("initializing view");
            this.sceneObjectsContainer = sceneObjectsContainer;
            initWindowManager();
            initEcs(GameModel.ecsWorld);
            tileUpdater = new LocalMapTileUpdater(sceneObjectsContainer.mapHolder);
            cameraAndMouseHandler = new CameraAndMouseHandler(sceneObjectsContainer);
            cameraAndMouseHandler.init();
            zRange.set(0, GameModel.localMap.bounds.maxZ - 1);
            tileUpdater.flush();
            resetCameraPosition();
            Debug.Log("view initialized");
        }

        public void update() {
            keyInputSystem?.update();
            cameraAndMouseHandler?.update();
            systems?.Run();
            sceneObjectsContainer.modelDebugInfoPanel.text = GameModel.get().getDebugInfo();
        }

        private void initEcs(EcsWorld ecsWorld) {
            systems = new EcsSystems(ecsWorld);
            systems.Add(new UnitVisualSystem());
            systems.Add(new ItemVisualSystem());
            systems.Add(new ItemVisualRemoveSystem());
            systems.Add(new DesignationVisualSystem());
            systems.Add(new PlantVisualSystem());
            systems.Init();
        }

        private void initWindowManager() {
            keyInputSystem.windowManager.addWindow(sceneObjectsContainer.jobsWindow, KeyCode.J);
            keyInputSystem.widgetManager.addWidget(sceneObjectsContainer.menuWidget);
            keyInputSystem.widgetManager.addWidget(sceneObjectsContainer.toolbarWidget);
        }
        
        public int changeLayer(int dz) => setLayer(currentZ + dz);

        public int setLayer(int z) {
            int oldZ = currentZ;
            currentZ = zRange.clamp(z);
            if (oldZ != currentZ) {
                tileUpdater.updateLayersVisibility(oldZ,currentZ);
            }
            return currentZ - oldZ;
        }
        
        private void resetCameraPosition() {
            Vector3Int cameraPosition = new(GameModel.localMap.bounds.maxX / 2, GameModel.localMap.bounds.maxY / 2, 0);
            for (int z = GameModel.localMap.bounds.maxZ - 1; z >=0 ; z--) {
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