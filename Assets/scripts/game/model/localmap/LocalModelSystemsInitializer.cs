using game.model.system;
using game.model.system.building;
using game.model.system.plant;
using game.model.system.task;
using game.model.system.task.designation;
using game.model.system.unit;
using game.model.system.util;
using game.model.system.zone;
using Leopotam.Ecs;
using util;

namespace game.model.localmap {
    public class LocalModelSystemsInitializer {
        public void init(LocalModel model) {
            model.scalableSystems = new EcsSystems(model.ecsWorld);
            model.unscalableSystems = new EcsSystems(model.ecsWorld);
            model.timeIndependentSystems = new EcsSystems(model.ecsWorld);
            // tasks
            addSystem(model, new UnitTaskAssignmentSystem()); // find or create tasks for units
            addSystem(model, new UnitActionCheckingSystem()); // check action condition and target reachability, creates sub actions
            addSystem(model, new UnitMovementSystem()); // move unit along path
            addSystem(model, new UnitActionPerformingSystem()); // add progress to unit's action and remove it when finished
            addSystem(model, new TaskCreationTimeoutSystem());
            addSystem(model, new TaskAssignmentHandlingEcsSystem()); // performs additional actions for just assigned tasks
            addSystem(model, new UnitPathfindingSystem()); // find paths to action targets
            addSystem(model, new TaskTimeoutSystem()); // counts delays for tasks that failed assignment
            // addSystem(model, new TaskCompletionSystem()); // handle completed tasks

            addSystem(model, new UnitNeedSystem());
            addSystem(model, new UnitDiseaseSystem());
            addSystem(model, new UnitWearNeedSystem());
            addSystem(model, new UnitEquipmentSlotCooldownSystem()); // rolls cooldowns for unit attacks
            
            // addSystem(model, new DesignationCompletionSystem()); // handle designation with completed tasks
            addSystem(model, new DesignationTaskCreationSystem()); // create tasks for designations
            addSystem(model, new TileUpdateSystem()); // dispatches entities updates to other update systems
            addSystem(model, new WorkbenchTaskCreationSystem());
            
            addSystem(model, new DoorClosingSystem());
            // addSystem(model, new WorkbenchTaskCompletionSystem());
            addSystem(model, new StockpileTaskCreationSystem());
            addSystem(model, new FarmTaskCreationSystem());

            // plants
            addSystem(model, new PlantAgeSystem()); // kills plants on max age
            addSystem(model, new PlantGrowthSystem()); // grows plants to maturity
            addSystem(model, new PlantWaitingSystem()); // tracks time for growing an keeping products
            addSystem(model, new PlantProductGrowthSystem()); // grows products on plants
            // addSystem(model, new PlantRemovingSystem()); // removes plants
            addSystem(model, new SubstrateGrowingSystem()); // spreads substrates to free tiles
            addSystem(model, new PlantHarvestSystem()); // kills plants or restarts product growth
            // addSystem(model, new PlantVisualUpdateSystem()); // updates sprites of plants
            
            
            addSystem(model, new ModelUpdateEcsSystem());
            
            model.scalableSystems.Inject(GameModel.get().globalSharedData).Inject(model).Init();
            model.unscalableSystems.Inject(GameModel.get().globalSharedData).Inject(model).Init();
            model.timeIndependentSystems.Inject(GameModel.get().globalSharedData).Inject(model).Init();
        }

        private void addSystem(LocalModel model, IEcsSystem system) {
            if (system is LocalModelUnscalableEcsSystem) model.unscalableSystems.Add(system);
            else if (system is LocalModelScalableEcsSystem) model.scalableSystems.Add(system);
            else if (system is LocalModelTimeIndependentEcsSystem) model.timeIndependentSystems.Add(system);
            else throw new GameException($"Unsupported EcsSystem {system.GetType().Name}");
        }
    }
}