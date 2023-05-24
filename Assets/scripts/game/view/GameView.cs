using game.input;
using game.model;
using game.model.container;
using game.model.localmap;
using game.view.camera;
using game.view.system.building;
using game.view.system.designation;
using game.view.system.item;
using game.view.system.mouse_tool;
using game.view.system.plant;
using game.view.system.unit;
using game.view.system.zone;
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
            initEcs(GameModel.get().currentLocalModel.ecsWorld);
            tileUpdater = new LocalMapTileUpdater(sceneObjectsContainer.mapHolder, model);
            PlayerControls playerControls = new();
            cameraAndMouseHandler = new CameraAndMouseHandler(sceneObjectsContainer, playerControls);
            KeyInputSystem.get().playerControls = playerControls;
            cameraAndMouseHandler.init();
            selector = new();
            initWindowManager();

            selector.updateBounds();
            selector.zRange.set(0, GameModel.get().currentLocalModel.localMap.bounds.maxZ - 1);
            tileUpdater.flush();
            resetCameraPosition();
            MouseToolManager.get().reset();
            sceneObjectsContainer.gamespeedWidgetHandler.updateVisual();
            sceneObjectsContainer.prioritySelectionWidgetHandler.init();
            Debug.Log("view initialized");
        }

        public void update() {
            KeyInputSystem.get().update();
            cameraAndMouseHandler?.update();
            systems?.Run();
            sceneObjectsContainer.modelDebugInfoPanel.text += GameModel.get().currentLocalModel.getDebugInfo();
        }

        private void initEcs(EcsWorld ecsWorld) {
            systems = new EcsSystems(ecsWorld);
            systems.Add(new UnitVisualSystem())
                .Add(new UnitActionProgressBarUpdateSystem())
                .Add(new ItemVisualSystem())
                .Add(new ItemVisualRemoveSystem())
                .Add(new DesignationVisualSystem())
                .Add(new PlantVisualUpdateSystem())
                .Add(new BuildingVisualSystem())
                .Add(new WorkbenchWindowUpdateSystem())
                .Add(new UnitMenuUpdateSystem())
                .Add(new ZoneVisualSystem())
                .Add(new TileUpdateVisualSystem())
                .Init();
        }

        private void initWindowManager() {
            KeyInputSystem system = KeyInputSystem.get();
            system.windowManager.addWindow(sceneObjectsContainer.jobsWindow);
            system.windowManager.addWindow(sceneObjectsContainer.workbenchWindowHandler);
            system.windowManager.addWindow(sceneObjectsContainer.itemMenuHandler);
            system.windowManager.addWindow(sceneObjectsContainer.unitMenuHandler);
            system.windowManager.addWindow(sceneObjectsContainer.stockpileMenuHandler);
            system.windowManager.addWindow(sceneObjectsContainer.farmMenuHandler);
            system.windowManager.addWindow(sceneObjectsContainer.plantMenuHandler);
            system.windowManager.closeAll();
            
            system.widgetManager.addWidget(sceneObjectsContainer.gamespeedWidgetHandler);
            system.widgetManager.addWidget(sceneObjectsContainer.menuWidget);
            system.widgetManager.addWidget(sceneObjectsContainer.toolbarWidget);
            
        }

        private void resetCameraPosition() {
            LocalMap map = GameModel.get().currentLocalModel.localMap;
            Vector3Int cameraPosition = new(map.bounds.maxX / 2, map.bounds.maxY / 2, 0);
            for (int z = map.bounds.maxZ - 1; z >= 0; z--) {
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
    }
}