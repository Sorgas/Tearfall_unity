using System.Collections.Generic;
using System.Linq;
using enums.action;
using game.model.component;
using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.item;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit {
    // finds and assigns appropriate tasks to units
    public class UnitTaskAssignmentSystem : LocalModelEcsSystem {
        EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskFinishedComponent> filter; // units without tasks

        public UnitTaskAssignmentSystem(LocalModel model) : base(model) {}

        public override void Run() {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                EcsEntity task = tryCreateTask(unit);
                if (!task.IsNull()) assignTask(ref unit, task);
            }
        }

        private EcsEntity tryCreateTask(EcsEntity unit) {
            EcsEntity jobTask = getTaskFromContainer(unit);
            // EcsEntity needTask = createNeedsTask(unit);
            // EcsEntity task = priority(jobTask) > priority(needTask) ? jobTask : needTask;
            // if (task.IsNull()) task = createIdleTask(unit);
            return jobTask;
        }

        // TODO add jobs priorities
        private EcsEntity getTaskFromContainer(EcsEntity unit) {
            UnitJobsComponent jobs = unit.take<UnitJobsComponent>();
            return model.taskContainer.findTask(jobs.enabledJobs, unit.pos());
        }

        //TODO add other needs
        private EcsEntity createNeedsTask(EcsEntity unit) {
            List<EcsEntity> taskList = new List<EcsEntity>();
            if (unit.Has<UnitCalculatedWearNeedComponent>()) {
                UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
                ItemSelector selector = new WearWithSlotItemSelector(wear.slotsToFill);
                List<EcsEntity> foundItems = selector.selectItems(model.itemContainer.onMap.all);
                if (foundItems.Count > 0) {
                    EcsEntity task = model.taskContainer.generator.createTask(new EquipWearItemAction(foundItems[0]), TaskPriorityEnum.HEALTH_NEEDS, model.createEntity(), model);
                    taskList.Add(task);
                }
            }
            if (taskList.Count > 0) {
                return taskList.Aggregate((task1, task2) => priority(task1) > priority(task2) ? task1 : task2);
            }
            return EcsEntity.Null;
        }

        private EcsEntity createIdleTask(EcsEntity unit) {
            Vector3Int current = unit.pos();
            // Debug.Log("creating idle task for unit in position " + current);
            Vector3Int? position = model.localMap.util.getRandomPosition(current, 10, 4);
            return position.HasValue
                ? model.taskContainer.generator.createTask(new MoveAction(position.Value), model.createEntity(), model)
                : EcsEntity.Null;
        }

        // bind unit and task entities
        private void assignTask(ref EcsEntity unit, EcsEntity task) {
            unit.Replace(new TaskComponent { task = task });
            task.Replace(new TaskPerformerComponent { performer = unit });
            if (task.Has<TaskJobComponent>()) {
                model.taskContainer.claimTask(task, unit);
            }
        }

        private TaskPriorityEnum priority(EcsEntity task) {
            return !task.IsNull() ? task.take<TaskPriorityComponent>().priority : TaskPriorityEnum.NONE;
        }
    }
} 