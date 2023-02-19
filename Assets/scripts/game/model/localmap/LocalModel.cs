using game.model.container;
using game.model.container.item;
using game.model.system.building;
using game.model.system.plant;
using game.model.system.task;
using game.model.system.task.designation;
using game.model.system.unit;
using game.model.system.zone;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.localmap { // contains LocalMap and ECS world for its entities
    public class LocalModel {
        public LocalMap localMap;

        public EcsWorld ecsWorld = new EcsWorld();
        public EcsSystems systems; // model systems

        // containers for referencing and CRUD on entities
        public readonly UnitContainer unitContainer = new();
        public readonly DesignationContainer designationContainer;
        public readonly TaskContainer taskContainer;
        public readonly ItemContainer itemContainer;
        public readonly PlantContainer plantContainer;
        public readonly BuildingContainer buildingContainer;
        public readonly ZoneContainer zoneContainer;
        
        public LocalModel() {
            designationContainer = new(this);
            taskContainer = new(this);
            itemContainer = new(this);
            plantContainer = new(this);
            buildingContainer = new(this);
            zoneContainer = new(this);
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
                .Add(new UnitActionCheckingSystem(this)) // check action condition and target reachability, creates sub actions
                .Add(new UnitPathfindingSystem(this)) // find paths to action targets
                .Add(new UnitMovementSystem(this)) // move unit along path
                .Add(new UnitActionPerformingSystem(this)) // add progress to unit's action and remove it when finished
                .Add(new UnitTaskCompletionSystem(this)) // handle unit with completed tasks

                .Add(new TaskCompletionSystem(this)) // handle completed tasks
                .Add(new DesignationCompletionSystem(this)) // handle designation with completed tasks
                .Add(new DesignationTaskCreationSystem(this)) // create tasks for designations
                .Add(new DesignationTaskCreationTimeoutSystem())

                .Add(new UnitWearNeedSystem())
                .Add(new UnitNeedSystem())
            
                .Add(new PlantRemovingSystem(this))
                .Add(new SubstrateGrowingSystem(this))

                .Add(new WorkbenchOrderSelectionSystem())
                .Add(new WorkbenchTaskCreationSystem(this))
                .Add(new WorkbenchTaskCompletionSystem())
                
                .Add(new ZoneDeletionSystem())
                .Init();
        }

        //TODO move ecs world to global game model. (as units should travel between regions)
        public EcsEntity createEntity() {
            EcsEntity entity = ecsWorld.NewEntity();
            // Debug.Log("created entity: " + entity.GetInternalId());
            return entity;
        } 

        public string getDebugInfo() {
            return "TaskContainer: open: " + taskContainer.openTaskCount + " assigned: " + taskContainer.assignedTaskCount + " \n";
        }
    }

    // model-aware component
    public abstract class LocalMapModelComponent {
        protected readonly LocalModel model;

        public LocalMapModelComponent(LocalModel model) => this.model = model;
    }
}