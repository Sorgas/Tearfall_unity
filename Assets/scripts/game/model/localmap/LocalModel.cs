using game.model.component;
using game.model.container;
using game.model.container.item;
using game.model.system;
using game.model.system.building;
using game.model.system.plant;
using game.model.system.task;
using game.model.system.task.designation;
using game.model.system.unit;
using game.model.system.zone;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.localmap { // contains LocalMap and ECS world for its entities
    public class LocalModel {
        public LocalMap localMap;

        public EcsWorld ecsWorld = new();
        public EcsSystems systems; // model systems

        // containers for referencing and CRUD on entities
        public readonly UnitContainer unitContainer = new();
        public readonly DesignationContainer designationContainer;
        public readonly TaskContainer taskContainer;
        public readonly ItemContainer itemContainer;
        public readonly PlantContainer plantContainer;
        public readonly BuildingContainer buildingContainer;
        public readonly ZoneContainer zoneContainer;
        public readonly FarmContainer farmContainer;
        private EcsEntity updateEntity;
        
        public LocalModel() {
            Debug.Log("creating EcsWorld");
            designationContainer = new(this);
            taskContainer = new(this);
            itemContainer = new(this);
            plantContainer = new(this);
            buildingContainer = new(this);
            zoneContainer = new(this);
            farmContainer = new(this);
        }

        public void update() {
            systems?.Run();
        }

        public void init() {
            Debug.Log("initializing local model");
            initEcs();
            localMap.init();
            Debug.Log("local model initialized");
        }

        private void initEcs() {
            systems = new EcsSystems(ecsWorld);
            systems
                .Add(new UnitTaskAssignmentSystem(this)) // find or create tasks for units
                .Add(new TaskAssignmentHandlingSystem()) // performs additional actions for just assigned tasks
                .Add(new UnitActionCheckingSystem(this)) // check action condition and target reachability, creates sub actions
                .Add(new UnitPathfindingSystem(this)) // find paths to action targets
                .Add(new UnitMovementSystem(this)) // move unit along path
                .Add(new UnitActionPerformingSystem(this)) // add progress to unit's action and remove it when finished
                .Add(new UnitTaskCompletionSystem(this)) // handle unit with completed tasks

                .Add(new TaskCompletionSystem(this)) // handle completed tasks
                .Add(new DesignationCompletionSystem(this)) // handle designation with completed tasks
                .Add(new DesignationTaskCreationSystem(this)) // create tasks for designations
                .Add(new TaskCreationTimeoutSystem())

                .Add(new UnitWearNeedSystem())
                .Add(new UnitNeedSystem())
            
                .Add(new TileUpdatingSystem(this)) // dispatches entities updates to other update systems
                
                .Add(new PlantRemovingSystem(this))
                .Add(new SubstrateGrowingSystem(this))

                .Add(new WorkbenchOrderSelectionSystem())
                .Add(new WorkbenchTaskCreationSystem(this))
                .Add(new WorkbenchTaskCompletionSystem())
                
                .Add(new ZoneUpdateSystem(this)) // updates zones
                .Add(new ZoneDeletionSystem())
                .Add(new StockpileTaskCreationSystem(this))
                .Add(new FarmTaskCreationSystem(this))
                .Init();
            updateEntity = createEntity();
            updateEntity.Replace(new TileUpdateComponent { tiles = new() });
        }

        //TODO move ecs world to global game model. (as units should travel between regions)
        public EcsEntity createEntity() {
            EcsEntity entity = ecsWorld.NewEntity();
            Debug.Log("created entity: " + entity.GetInternalId());
            return entity;
        }

        public string getDebugInfo() {
            return "TaskContainer: open: " + taskContainer.openTaskCount + " assigned: " + taskContainer.assignedTaskCount + " \n";
        }
    }

    // model-aware component
    public abstract class LocalModelComponent {
        protected readonly LocalModel model;

        protected LocalModelComponent(LocalModel model) => this.model = model;
    }

    // for containers which can update positions
    public abstract class LocalModelUpdateComponent : LocalModelComponent {
        protected EcsEntity updateEntity;

        protected LocalModelUpdateComponent(LocalModel model) : base(model) {
            Debug.Log("creating localModelUpdateComponent");
            updateEntity = model.createEntity();
            updateEntity.Replace(new TileUpdateComponent { tiles = new() });
        }

        public void addPositionForUpdate(Vector3Int position) {
            updateEntity.take<TileUpdateComponent>().tiles.Add(position);
        }
    }
}