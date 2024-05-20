using game.input;
using game.model;
using game.model.localmap;
using game.view.camera;
using game.view.system;
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
    public CameraAndMouseHandler cameraAndMouseHandler;
    private EcsSystems systems; // systems for updating scene
    public LocalMapTileUpdater tileUpdater; // manager for tilemaps
    public readonly IntBounds2 screenBounds = new(Screen.width, Screen.height);
    public SceneElementsReferences sceneElementsReferences;
    public EntitySelector selector;
    
    public void init(LocalGameRunner runner, LocalModel model) {
        Debug.Log("initializing view");
        sceneElementsReferences = runner.sceneElementsReferences;
        initEcs(GameModel.get().currentLocalModel.ecsWorld);
        tileUpdater = new LocalMapTileUpdater(sceneElementsReferences.mapHolder, model);
        PlayerControls playerControls = new();
        cameraAndMouseHandler = new CameraAndMouseHandler(runner, playerControls);
        KeyInputSystem.get().playerControls = playerControls;
        cameraAndMouseHandler.init();
        selector = new();
        initWindowManager();

        selector.updateBounds();
        selector.zRange.set(0, GameModel.get().currentLocalModel.localMap.bounds.maxZ - 1);
        tileUpdater.flush();
        resetCameraPosition();
        MouseToolManager.get().reset();
        // runner.sceneElementsReferences.init();
        Debug.Log("initialising ui components");
        foreach (Transform o in runner.sceneElementsReferences.transform) {
            Debug.Log(o.gameObject.name);
            o.GetComponent<Initable>()?.init();
        }
        Debug.Log("view initialized");
    }

    public void update() {
        KeyInputSystem.get().update();
        cameraAndMouseHandler?.update();
        systems?.Run();
        // runner.sceneElementsReferences.modelDebugInfoPanel.text += GameModel.get().currentLocalModel.getDebugInfo();
    }

    // create systems which update visual objects when model objects change
    private void initEcs(EcsWorld ecsWorld) {
        systems = new EcsSystems(ecsWorld);
        systems.Add(new UnitVisualSystem())
            .Add(new UnitActionVisualSystem())
            .Add(new ItemVisualSystem())
            .Add(new ItemVisualRemoveSystem())
            .Add(new DesignationVisualSystem())
            .Add(new PlantVisualUpdateSystem())
            .Add(new BuildingVisualSystem())
            .Add(new DoorVisualSystem())
            .Add(new UnitMenuUpdateSystem())
            .Add(new ZoneVisualSystem())
            .Add(new TileUpdateVisualSystem())
            .Add(new RoomVisualSystem())
            .Init();
    }

    // add windows and widgets to manager
    private void initWindowManager() {
        KeyInputSystem system = KeyInputSystem.get();
        system.windowManager.addWindow(sceneElementsReferences.jobsWindow);
        system.windowManager.addWindow(sceneElementsReferences.workbenchWindowHandler);
        system.windowManager.addWindow(sceneElementsReferences.itemMenuHandler);
        system.windowManager.addWindow(sceneElementsReferences.unitMenuHandler);
        system.windowManager.addWindow(sceneElementsReferences.stockpileMenuHandler);
        system.windowManager.addWindow(sceneElementsReferences.farmMenuHandler);
        system.windowManager.addWindow(sceneElementsReferences.plantMenuHandler);
        system.windowManager.closeAll();

        system.widgetManager.addWidget(sceneElementsReferences.gamespeedWidgetHandler);
        system.widgetManager.addWidget(sceneElementsReferences.menuWidget);
        system.widgetManager.addWidget(sceneElementsReferences.toolbarWidget);
    }

    // move selector and camera to ground level in map center
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