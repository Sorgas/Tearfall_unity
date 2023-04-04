using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.plant;
using game.model.component.task.order;
using game.model.localmap;
using Leopotam.Ecs;
using types.unit;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.task.designation {
    // creates tasks for designations without tasks. stores tasks in TaskContainer
    public class DesignationTaskCreationSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<DesignationComponent>.Exclude<TaskComponent, TaskFinishedComponent> filter;

        public override void Run() {
            foreach (var i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                if (entity.Has<TaskCreationTimeoutComponent>()) continue;
                EcsEntity task = createTaskForDesignation(entity, filter.Get1(i));
                if (task == EcsEntity.Null) continue;
                entity.Replace(new TaskComponent { task = task });
                task.Replace(new TaskDesignationComponent { designation = entity });
                model.taskContainer.addOpenTask(task);
            }
        }

        private EcsEntity createTaskForDesignation(EcsEntity entity, DesignationComponent designation) {
            if (designation.type.job.Equals(Jobs.MINER.name)) {
                EcsEntity taskEntity = model.taskContainer.generator.createTask(new DigAction(entity.pos(), designation.type), model.createEntity(), model);
                taskEntity.Replace(new TaskJobComponent { job = Jobs.MINER.name });
                taskEntity.Replace(new TaskBlockOverrideComponent { blockType = designation.type.getDiggingBlockType() });
                Debug.Log("mining task created.");
                return taskEntity;
            }
            if (designation.type.job.Equals(Jobs.WOODCUTTER.name)) {
                EcsEntity taskEntity = model.taskContainer.generator.createTask(new ChopTreeAction(entity.pos()), model.createEntity(), model);
                taskEntity.Replace(new TaskJobComponent { job = Jobs.WOODCUTTER.name });
                Debug.Log("woodcutting task created.");
                return taskEntity;
            }
            if (designation.type.job.Equals(Jobs.BUILDER.name)) {
                if (entity.Has<DesignationConstructionComponent>()) {
                    return createConstructionTask(entity);
                } else {
                    return createBuildingTask(entity);
                }
            }
            return EcsEntity.Null;
        }

        private EcsEntity createConstructionTask(EcsEntity entity) {
            DesignationConstructionComponent comp = entity.take<DesignationConstructionComponent>();
            ConstructionOrder order = 
                new(comp.type.blockType, comp.itemType, comp.material, comp.amount, entity.pos());
            EcsEntity taskEntity = model.taskContainer.generator.createTask(new ConstructionAction(entity, order), model.createEntity(), model);
            taskEntity.Replace(new TaskJobComponent { job = Jobs.BUILDER.name });
            taskEntity.Replace(new TaskBlockOverrideComponent { blockType = order.blockType });
            Debug.Log("construction task created.");
            return taskEntity;
        }

        private EcsEntity createBuildingTask(EcsEntity designation) {
            DesignationBuildingComponent comp = designation.take<DesignationBuildingComponent>();
            BuildingOrder order = new(comp.itemType, comp.material, comp.amount, designation.pos());
            order.type = comp.type;
            order.orientation = comp.orientation;
            EcsEntity taskEntity = model.taskContainer.generator.createTask(new BuildingAction(designation, order), model.createEntity(), model);
            taskEntity.Replace(new TaskJobComponent { job = Jobs.BUILDER.name });
            Debug.Log("construction task created.");
            return taskEntity;
        }
    }
}