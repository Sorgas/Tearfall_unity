using game.model.component;
using game.model.system;
using game.model.system.building;
using game.model.system.plant;
using game.model.system.task;
using game.model.system.task.designation;
using game.model.system.unit;
using game.model.system.util;
using game.model.system.zone;
using Leopotam.Ecs;

namespace game.model.localmap {
    public class LocalModelSystemsInitializer {
        public void init(LocalModel model) {
            model.scalableSystems = new EcsSystems(model.ecsWorld);
            model.unscalableSystems = new EcsSystems(model.ecsWorld);
            addSystem(model, new UnitMovementSystem()); // move unit along path
            addSystem(model, new UnitActionPerformingSystem()); // add progress to unit's action and remove it when finished
            addSystem(model, new TaskCreationTimeoutSystem());
            addSystem(model, new UnitNeedSystem());
            addSystem(model, new UnitWearNeedSystem());
            addSystem(model, new SubstrateGrowingSystem());
            addSystem(model, new UnitActionCheckingSystem()); // check action condition and target reachability, creates sub actions
            addSystem(model, new UnitTaskAssignmentSystem()); // find or create tasks for units
            addSystem(model, new TaskAssignmentHandlingSystem()); // performs additional actions for just assigned tasks
            addSystem(model, new UnitPathfindingSystem()); // find paths to action targets
            addSystem(model, new UnitTaskCompletionSystem()); // handle unit with completed tasks
            addSystem(model, new TaskCompletionSystem()); // handle completed tasks
            addSystem(model, new DesignationCompletionSystem()); // handle designation with completed tasks
            addSystem(model, new DesignationTaskCreationSystem()); // create tasks for designations
            addSystem(model, new TileUpdatingSystem()); // dispatches entities updates to other update systems
            addSystem(model, new PlantRemovingSystem());
            addSystem(model, new WorkbenchOrderSelectionSystem());
            addSystem(model, new WorkbenchTaskCreationSystem());
            addSystem(model, new WorkbenchTaskCompletionSystem());
            addSystem(model, new ZoneUpdateSystem()); // updates zones
            addSystem(model, new ZoneDeletionSystem());
            addSystem(model, new StockpileTaskCreationSystem());
            addSystem(model, new FarmTaskCreationSystem());
            model.scalableSystems
                .Inject(GameModel.get().globalSharedData)
                .Inject(model)
                .Init();
            model.unscalableSystems
                .Inject(GameModel.get().globalSharedData)
                .Inject(model)
                .Init();
            model.updateEntity = model.createEntity();
            model.updateEntity.Replace(new TileUpdateComponent { tiles = new() });

        }
        
        private void addSystem(LocalModel model, IEcsSystem system) {
            (system is LocalModelScalableEcsSystem ? model.scalableSystems : model.unscalableSystems).Add(system);
        }
    }
}