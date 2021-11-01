using game.model;
using game.model.tilemaps;
using game.view.system.unit;
using game.view.ui.jobs_widget;
using generation;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry;
using util.lang;

namespace game.view {
    // component for binding GameModel and GameObjects in scene. 
    public class GameView : Singleton<GameView> {
        private KeyInputSystem keyInputSystem = KeyInputSystem.get();
        public CameraHandler cameraHandler;
        
        public LocalMapTileUpdater tileUpdater;

        private EcsSystems systems; // systems for updating scene
        public Vector2Int selectorOverlook = new Vector2Int();
        public RectTransform mapHolder;
        public RectTransform jobsMenu;
        public bool useSelector = false;
        private readonly ValueRangeInt zRange = new ValueRangeInt(); // range for current z in model units 
        public int currentZ;

        public void init(LocalGameRunner initializer) {
            Debug.Log("initializing view");
            mapHolder = initializer.mapHolder;
            jobsMenu = initializer.jobsMenu;
            initEcs(GenerationState.get().ecsWorld);
            tileUpdater = new LocalMapTileUpdater(initializer.mapHolder);
            cameraHandler = new CameraHandler(initializer, false);
            zRange.set(0, GameModel.get().localMap.zSize - 1);
            tileUpdater.flush();
            Debug.Log("view initialized");
        }

        public void update() {
            cameraHandler.update();
            // hotKeyInputSystem?.update();
            systems?.Run();
        }

        private void initEcs(EcsWorld ecsWorld) {
            systems = new EcsSystems(ecsWorld);
            systems.Add(new UnitVisualSystem());
            systems.Init();
        }

        public void toggleJobsMenu() {
            toggleMenu(jobsMenu.gameObject);
        }

        private void toggleMenu(GameObject menu) {
            if (!menu.activeSelf) {
                hideAllMenus();
                cameraHandler.enabled = false;
                menu.GetComponent<JobsWidgetHandler>().refill();
                menu.SetActive(true);
            } else {
                hideAllMenus();
            }
        }

        public void hideAllMenus() {
            cameraHandler.enabled = true;
            jobsMenu.gameObject.SetActive(false);
        }

        public int changeLayer(int dz) {
            int oldZ = currentZ;
            currentZ = zRange.clamp(currentZ + dz);
            return currentZ - oldZ;
        }
    }
}