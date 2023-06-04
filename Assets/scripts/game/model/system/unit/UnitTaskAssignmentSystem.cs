using System.Collections.Generic;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.unit;
using game.model.localmap.passage;
using Leopotam.Ecs;
using types.action;
using types.unit;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.system.unit {
    /**
     * Find and assigns tasks for units without tasks.
     * 
     * When searching task for unit:
     * For unit's jobs in priority descending order:
     *      Tasks of these jobs and with available targets are taken from container.
     *      For task priorities in descending order:
     *          Select tasks of current priority
     *          For tasks by ascending of distance to target
     *              Check task conditions with creating sub actions
     *              If success, assign task to unit
     *              If fail, add timeout component to task, return to container.
     */
    public class UnitTaskAssignmentSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskFinishedComponent> filter; // units without tasks
        private readonly UnitNeedActionCreator needActionCreator = new();

        public override void Run() {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                EcsEntity task = tryCreateTask(unit);
                if (task != EcsEntity.Null) assignTask(ref unit, task);
            }
        }

        // TODO compare max priority of needs and tasks before lookup in container
        private EcsEntity tryCreateTask(EcsEntity unit) {
            if (unit.Has<UnitNextTaskComponent>()) return createNextTask(unit);
            EcsEntity jobTask = getTaskFromContainer(unit);
            EcsEntity needTask = needActionCreator.selectAndCreateAction(model, unit);
            EcsEntity task = priority(jobTask) > priority(needTask) ? jobTask : needTask;
            if (task.IsNull()) task = createIdleTask(unit);
            return task;
        }

        private EcsEntity createNextTask(EcsEntity unit) {
            return model.taskContainer.generator
                .createTask(unit.take<UnitNextTaskComponent>().action, Jobs.NONE, model.createEntity(), model);
        }

        private EcsEntity getTaskFromContainer(EcsEntity unit) {
            UnitJobsComponent jobs = unit.take<UnitJobsComponent>();
            PassageMap passageMap = model.localMap.passageMap;
            byte area = passageMap.area.get(unit.pos());
            for (int jobPriority = TaskPriorities.range.max; jobPriority >= TaskPriorities.range.min; jobPriority--) {
                List<string> jobsList = jobs.getByPriority(jobPriority);
                if (jobsList.Count <= 0) continue;
                Dictionary<int, List<EcsEntity>> tasks = model.taskContainer.getTasksByJobs(jobsList);
                for (int taskPriority = TaskPriorities.range.max; taskPriority >= TaskPriorities.range.min; taskPriority--) {
                    if (!tasks.ContainsKey(taskPriority)) continue;
                    foreach (EcsEntity task in tasks[taskPriority]) {
                        if (checkTaskTarget(task, area, passageMap)) return task;
                        model.taskContainer.moveOpenTaskToDelayed(task);
                        task.Replace(new TaskTimeoutComponent { timeout = 100 });
                    }
                }
            }
            return EcsEntity.Null;
        }

        private bool checkTaskTarget(EcsEntity task, byte performerArea, PassageMap passageMap) {
            ActionTargetTypeEnum targetType = task.take<TaskActionsComponent>().initialAction.target.type;
            Vector3Int target = task.take<TaskActionsComponent>().initialAction.target.pos;
            if (target == Vector3Int.back) {
                Debug.LogError("target position for " + task.name() + " not found ");
                return false;
            }
            // target position in same area with performer
            if (targetType == EXACT || targetType == ANY) {
                if (passageMap.area.get(target) == performerArea) return true;
            }
            // target position is accessible from performer area
            if (targetType == NEAR || targetType == ANY) {
                NeighbourPositionStream stream = new(target, model);
                stream = task.Has<TaskBlockOverrideComponent>()
                    ? stream.filterConnectedToCenterWithOverrideTile(task.take<TaskBlockOverrideComponent>().blockType)
                    : stream.filterConnectedToCenter();
                return stream.collectAreas().Contains(performerArea);
            }
            return false;
        }

        private EcsEntity createIdleTask(EcsEntity unit) {
            Vector3Int current = unit.pos();
            // Debug.Log("creating idle task for unit in position " + current);
            Vector3Int? position = model.localMap.util.getRandomPosition(current, 10, 4);
            return position.HasValue
                ? model.taskContainer.generator.createTask(new MoveAction(position.Value), Jobs.NONE, model.createEntity(), model)
                : EcsEntity.Null;
        }

        // bind unit and task entities
        private void assignTask(ref EcsEntity unit, EcsEntity task) {
            Debug.Log("[UnitTaskAssignmentSystem] assigning task [" + task.name() + "] to " + unit.name());
            unit.Replace(new TaskComponent { task = task });
            task.Replace(new TaskPerformerComponent { performer = unit });
            task.Replace(new TaskAssignedComponent());
            model.taskContainer.claimTask(task, unit);
        }
    
        private int priority(EcsEntity task) {
            return task.IsNull() ? TaskPriorities.NONE : task.take<TaskJobComponent>().priority;
        }
    }
}