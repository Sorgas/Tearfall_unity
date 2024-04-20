using game.model.action.plant;
using game.model.component;
using game.model.component.plant;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.plant;
using game.model.component.task.order;
using game.model.localmap;
using game.model.util.validation;
using generation.item;
using Leopotam.Ecs;
using types;
using types.unit;
using UnityEngine;
using util.lang.extension;
using static types.action.TaskPriorities;
using static types.unit.Jobs;

namespace game.model.system.task.designation {
    // creates tasks for designations without tasks. stores tasks in TaskContainer
    public class DesignationTaskCreationSystem : LocalModelUnscalableEcsSystem {
        private CraftingOrderGenerator generator = new();
        // TODO remove TaskFinishedComponent
        public EcsFilter<DesignationComponent>.Exclude<TaskComponent> filter;

        public override void Run() {
            foreach (var i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                if (entity.Has<TaskCreationTimeoutComponent>()) continue;
                EcsEntity task = createTaskForDesignation(entity, filter.Get1(i));
                if (task != EcsEntity.Null) {
                    entity.Replace(new TaskComponent { task = task });
                    task.Replace(new TaskDesignationComponent { designation = entity });
                    model.taskContainer.addOpenTask(task);
                }
            }
        }

        private EcsEntity createTaskForDesignation(EcsEntity entity, DesignationComponent designation) {
            int priority = designation.priority;
            Vector3Int position = entity.pos();
            if (designation.type.job.Equals(MINER.name)) {
                Action action = new DigAction(position, designation.type); 
                EcsEntity task = model.taskContainer.generator.createTask(action, MINER, priority, model.createEntity(), model);
                task.Replace(new TaskBlockOverrideComponent { blockType = designation.type.getDiggingBlockType() });
                Debug.Log("mining task created.");
                return task;
            }
            if (designation.type.job.Equals(WOODCUTTER.name)) {
                if (PlaceValidators.TREE_EXISTS.validate(position, model)) {
                    EcsEntity tree = model.plantContainer.getPlant(position);
                    Action action = new ChopTreeAction(tree); 
                    EcsEntity task = model.taskContainer.generator.createTask(action, WOODCUTTER, priority, model.createEntity(), model);
                    Debug.Log("woodcutting task created.");
                    return task;
                }
            }
            if (designation.type.job.Equals(BUILDER.name)) {
                if (entity.Has<DesignationConstructionComponent>()) {
                    return createConstructionTask(entity, designation);
                } else {
                    return createBuildingTask(entity, designation);
                }
            }
            if (designation.type == DesignationTypes.D_HARVEST_PLANT) {
                EcsEntity zone = model.zoneContainer.getZone(position);
                Job job = FORAGER;
                if (zone != EcsEntity.Null && zone.take<ZoneComponent>().type == ZoneTypes.FARM.value) {
                    job = FARMER;
                }
                EcsEntity plant = model.plantContainer.getPlant(position);
                if (plant != EcsEntity.Null && plant.Has<PlantHarvestableComponent>()) {
                    Action action = new PlantHarvestAction(plant);
                    return model.taskContainer.generator.createTask(action, job, priority, model.createEntity(), model);
                }
            }
            return EcsEntity.Null;
        }

        private EcsEntity createConstructionTask(EcsEntity entity, DesignationComponent designation) {
            DesignationConstructionComponent comp = entity.take<DesignationConstructionComponent>();
            ConstructionOrder order = comp.order;
            Action action = new ConstructionAction(entity, order);
            EcsEntity taskEntity = model.taskContainer.generator.createTask(action, BUILDER, designation.priority, model.createEntity(), model);
            taskEntity.Replace(new TaskBlockOverrideComponent { blockType = order.blockType });
            Debug.Log("construction task created.");
            return taskEntity;
        }

        private EcsEntity createBuildingTask(EcsEntity entity, DesignationComponent designation) {
            DesignationBuildingComponent comp = entity.take<DesignationBuildingComponent>();
            BuildingOrder order = comp.order;
            Action action = new BuildingAction(entity, order);
            EcsEntity taskEntity = model.taskContainer.generator.createTask(action, BUILDER, designation.priority, model.createEntity(), model);
            Debug.Log("construction task created.");
            return taskEntity;
        }
    }
}