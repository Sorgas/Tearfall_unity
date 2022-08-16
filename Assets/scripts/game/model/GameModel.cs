using game.model.container;
using game.model.container.item;
using game.model.entity_selector;
using game.model.localmap;
using game.model.system.plant;
using game.model.system.task;
using game.model.system.task.designation;
using game.model.system.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;

namespace game.model {
    public class GameModel : Singleton<GameModel> {
        public World world;
        public EcsWorld _ecsWorld;
        public EcsSystems systems; // model systems
        public EntitySelectorSystem selectorSystem = new();
        public readonly UnitContainer unitContainer = new();
        public readonly DesignationContainer designationContainer = new();
        public readonly TaskContainer taskContainer = new();
        public readonly ItemContainer itemContainer = new();
        public readonly PlantContainer plantContainer = new();
        public readonly BuildingContainer buildingContainer = new();
        private int count = 0;

        public static EcsWorld ecsWorld => get()._ecsWorld;
        public static LocalMap localMap => get().world.localMap;

        // init with entities generated on new game or loaded from savegame
        public void update() {
            systems?.Run();
        }
        
        public new void init() {
            Debug.Log("initializing model");
            initEcs();
            // selectorSystem.selector = selector;
            // selectorSystem.placeSelectorAtMapCenter();
            localMap.init();
            Debug.Log("model initialized");
        }

        private void initEcs() {
            systems = new EcsSystems(ecsWorld);
            systems
                .Add(new UnitTaskAssignmentSystem()) // find or create tasks for units
                .Add(new UnitActionCheckingSystem()) // check action condition and target reachability, creates sub actions
                .Add(new UnitPathfindingSystem()) // find paths to action targets
                .Add(new UnitMovementSystem()) // move unit along path
                .Add(new UnitActionPerformingSystem()) // add progress to unit's action and remove it when finished
                .Add(new UnitTaskCompletionSystem()) // handle unit with completed tasks
                
                .Add(new TaskCompletionSystem()) // handle completed tasks
                .Add(new DesignationCompletionSystem()) // handle designation with completed tasks
                .Add(new DesignationTaskCreationSystem()) // create tasks for designations
                .Add(new DesignationTaskCreationTimeoutSystem())
                
                .Add(new UnitWearNeedSystem())
                .Add(new UnitNeedSystem())
                .Add(new PlantRemovingSystem())
                .Init();
        }

        public EcsEntity createEntity() {
            return ecsWorld.NewEntity();
        }

        public void clear() {
            if(_ecsWorld != null) _ecsWorld.Destroy();
            _ecsWorld = new EcsWorld();
            world = new World();
            // TODO clear model
        }

        public string getDebugInfo() {
            return "TaskContainer: open: " + taskContainer.openTaskCount + " assigned: " + taskContainer.assignedTaskCount + " \n";
        }
    }
}