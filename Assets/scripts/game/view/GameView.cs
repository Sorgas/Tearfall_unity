using game.model;
using game.model.localmap;
using game.view.camera;
using game.view.system.building;
using game.view.system.designation;
using game.view.system.item;
using game.view.system.mouse_tool;
using game.view.system.plant;
using game.view.system.unit;
using game.view.tilemaps;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;
using util.lang;

namespace game.view {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        public LocalGameRunner sceneObjectsContainer;
        public CameraAndMouseHandler cameraAndMouseHandler;
        public LocalMapTileUpdater tileUpdater;
        private EcsSystems systems; // systems for updating scene
        public readonly IntBounds2 screenBounds = new(Screen.width, Screen.height);

        public EntitySelector selector;

        public void init(LocalGameRunner sceneObjectsContainer, LocalModel model) {
            Debug.Log("initializing view");
            this.sceneObjectsContainer = sceneObjectsContainer;
            initWindowManager();
            initEcs(GameModel.get().currentLocalModel.ecsWorld);
            tileUpdater = new LocalMapTileUpdater(sceneObjectsContainer.mapHolder, model);
            cameraAndMouseHandler = new CameraAndMouseHandler(sceneObjectsContainer);
            cameraAndMouseHandler.init();
            selector = new();
            
            selector.updateBounds();
            selector.zRange.set(0, GameModel.get().currentLocalModel.localMap.bounds.maxZ - 1);
            tileUpdater.flush();
            resetCameraPosition();
            MouseToolManager.reset();
            sceneObjectsContainer.gamespeedWidgetHandler.updateVisual();
            Debug.Log("view initialized");
        }
        
        public void update() {
            KeyInputSystem.get().update();
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
            systems.Add(new BuildingVisualSystem());
            systems.Init();
        }

        private void initWindowManager() {
            KeyInputSystem system = KeyInputSystem.get();
            system.windowManager.addWindow(sceneObjectsContainer.jobsWindow);
            system.windowManager.addWindow(sceneObjectsContainer.workbenchWindowHandler);
            system.widgetManager.addWidget(sceneObjectsContainer.gamespeedWidgetHandler);
            system.widgetManager.addWidget(sceneObjectsContainer.menuWidget);
            system.widgetManager.addWidget(sceneObjectsContainer.toolbarWidget);

        }
        
        private void resetCameraPosition() {
            LocalMap map = GameModel.get().currentLocalModel.localMap;
            Vector3Int cameraPosition = new(map.bounds.maxX / 2, map.bounds.maxY / 2, 0);
            for (int z = map.bounds.maxZ - 1; z >=0 ; z--) {
                if (map.blockType.get(cameraPosition.x, cameraPosition.y, z) != BlockTypes.SPACE.CODE) {
                    cameraPosition.z = z;
                    break;
                }
            }
            selector.updatePosition(cameraPosition);
            selector.setLayer(cameraPosition.z + 1); // hack to disable unseen levels renderers
            selector.setLayer(cameraPosition.z);
            cameraAndMouseHandler.cameraMovementSystem.setTargetModel(cameraPosition);
            cameraAndMouseHandler.mouseMovementSystem.updateTargetAndSprite(cameraPosition);
        }
        
        public Vector3 screenToScenePosition(Vector3 screenPosition) {
            Vector3 worldPosition = sceneObjectsContainer.mainCamera.ScreenToWorldPoint(screenPosition);
            return sceneObjectsContainer.mapHolder.InverseTransformPoint(worldPosition); // position relative to mapHolder
        }
    }
}