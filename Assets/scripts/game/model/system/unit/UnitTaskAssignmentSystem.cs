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
    public class UnitTaskAssignmentSystem : IEcsRunSystem {
        EcsFilter<UnitComponent>.Exclude<TaskComponent> filter; // units without tasks

        public void Run() {
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
            EcsEntity task = GameModel.get().taskContainer.findTask(jobs.enabledJobs, unit.pos());
            if (!task.IsNull()) return task;
            return EcsEntity.Null; // TODO get from task container
        }

        //TODO add other needs
        private EcsEntity createNeedsTask(EcsEntity unit) {
            List<EcsEntity> taskList = new List<EcsEntity>();
            if (unit.Has<UnitCalculatedWearNeedComponent>()) {
                UnitCalculatedWearNeedComponent wear = unit.take<UnitCalculatedWearNeedComponent>();
                ItemSelector selector = new WearWithSlotItemSelector(wear.slotsToFill);
                List<EcsEntity> foundItems = selector.selectItems(GameModel.get().itemContainer.onMapItems.all);
                if (foundItems.Count > 0) {
                    EcsEntity task = GameModel.get().taskContainer.generator.createTask(new EquipWearItemAction(foundItems[0]), TaskPriorityEnum.HEALTH_NEEDS);
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
            Vector3Int? position = GameModel.localMap.util.getRandomPosition(current, 10, 4);
            return position.HasValue
                ? GameModel.get().taskContainer.generator.createTask(new MoveAction(position.Value))
                : EcsEntity.Null;
        }

        // bind unit and task entities
        private void assignTask(ref EcsEntity unit, EcsEntity task) {
            unit.Replace(new TaskComponent { task = task });
            task.Replace(new TaskPerformerComponent { performer = unit });
            if (task.Has<TaskJobComponent>()) {
                GameModel.get().taskContainer.claimTask(task, unit);
            }
        }

        private TaskPriorityEnum priority(EcsEntity task) {
            return !task.IsNull() ? task.take<TaskPriorityComponent>().priority : TaskPriorityEnum.NONE;
        }
    }
} 