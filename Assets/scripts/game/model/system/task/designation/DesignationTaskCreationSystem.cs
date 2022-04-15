using enums.unit;
using game.model.component;
using game.model.component.task.action;
using Leopotam.Ecs;
using UnityEngine;
using static game.model.component.task.DesignationComponents;
using static game.model.component.task.TaskComponents;

namespace game.model.system.task.designation {
    // creates tasks for designations without tasks. stores tasks in TaskContainer
    public class DesignationTaskCreationSystem : IEcsRunSystem {
        public EcsFilter<DesignationComponent, PositionComponent>.Exclude<TaskComponent> filter;

        public void Run() {
            foreach (var i in filter) {
                EcsEntity designation = filter.GetEntity(i);
                if (!validateDesignation(designation)) continue;
                EcsEntity? task = createTaskForDesignation(filter.Get1(i), filter.Get2(i));
                if (task.HasValue) {
                    designation.Replace(new TaskComponent { task = task.Value });
                    task.Value.Replace(new TaskDesignationComponent {designation = designation});
                    GameModel.get().taskContainer.addOpenTask(task.Value);
                }
            }
        }

        private bool validateDesignation(EcsEntity entity) {
            return true; // TODO
        }

        private EcsEntity? createTaskForDesignation(DesignationComponent designation, PositionComponent position) {
            if (designation.type.JOB.Equals(JobsEnum.MINER.name)) {
                Debug.Log("mining task created.");
                EcsEntity taskEntity = GameModel.get().taskContainer.generator.createTask(new DigAction(position.position, designation.type));
                taskEntity.Replace(new TaskJobComponent { job = JobsEnum.MINER.name });
                return taskEntity;
            }
            // switch (designation.type.NAME) {
            //     case "cutting stairs": {
            //         return null;
            //     }
            //     case "cutting downstairs": {
            //         return null;
            //     }
            //     case "cutting ramp": {
            //         return null;
            //     }
            //     case "digging channel": {
            //         return null;
            //     }
            //     case "chopping trees": {
            //         return null;
            //     }
            //     case "cutting plants": {
            //         return null;
            //     }
            //     case "harvesting plants": {
            //         return null;
            //     }
            //     case "building": {
            //         return null;
            //     }
            //     case "hoeing": {
            //         return null;
            //     }
            //     case "planting": {
            //         return null;
            //     }
            // }
            return null;
        }

        //    private DesignationTaskCreator designationTaskCreator;
        //
        //    public DesignationSystem(TaskContainer container) {
        //        this.container = container;
        //        designationTaskCreator = new DesignationTaskCreator();
        //    }
        //
        //    public void update() {
        //        // remove finished designations
        //        container.designations.entrySet().removeIf(entry->entry.getValue().isFinished());
        //        for (Designation designation :
        //        container.designations.values()) {
        //            if (designation instanceof BuildingDesignation) {
        //                if (!((BuildingDesignation)designation).checkSite()) {
        //                    designation.task.status = CANCELED;
        //                    Logger.DESIGNATION.logWarn("Place for building became invalid.");
        //                }
        //            }
        //            if (designation.task == null) {
        //                container.addTask(designationTaskCreator.createTaskForDesignation(designation, 1));
        //                Logger.DESIGNATION.logDebug("Create task for designation " + designation.type);
        //            }
        //        }
        //        container.designations.entrySet().removeIf(entry->entry.getValue().task == null); // remove designations with not created tasks.
        //    }
        //
        //    /**
        // * Validates designation and creates comprehensive task.
        // * All simple orders like digging and foraging submitted through this method.
        // */
        //    public void submitDesignation(IntVector3 position, DesignationTypeEnum type) {
        //        if (!type.VALIDATOR.apply(position)) return;
        //        removeDesignation(position); // remove previous designation
        //        if (type != DesignationTypeEnum.D_NONE) {
        //            container.designations.put(position, new Designation(position, type)); // put new designation
        //            Logger.DESIGNATION.logDebug("Designation " + type + " added to " + position);
        //        }
        //    }
        //
        //    /**
        // * Adds designation and creates comprehensive task.
        // * All single-tile buildings are constructed through this method.
        // */
        //    public void submitBuildingDesignation(BuildingOrder order, int priority) {
        //        Optional.ofNullable(order)
        //            .filter(order1->PlaceValidatorEnum.getValidator(order1.blueprint.placing).apply(order1.position))
        //            .map(BuildingDesignation::new)
        //            .ifPresent(designation->container.designations.put(designation.position, designation));
        //    }
        //
        //    public void submitPlantingDesignation(IntVector3 position, String specimen) {
        //        if (PlaceValidatorEnum.FARM.VALIDATOR.apply(position))
        //            container.designations.put(position, new PlantingDesignation(position, specimen));
        //    }
        //
        //    public void removeDesignation(IntVector3 position) {
        //        Optional.ofNullable(container.designations.get(position)) // cancel previous designation
        //            .map(foundDesignation->foundDesignation.task)
        //            .ifPresent(task-> {
        //            task.status = CANCELED; // task will be removed in TaskStatusSystem
        //            container.designations.remove(position);
        //        });
        //    }
    }
}