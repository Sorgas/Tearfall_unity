using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.unit;
using game.model.localmap.passage;
using Leopotam.Ecs;
using MoreLinq;
using types.action;
using types.unit;
using UnityEngine;
using util.geometry;
using util.lang;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;
using static types.action.TaskPriorities;

namespace game.model.system.unit {
/**
 * 1. find tasks for all units (without assignment)
 *      2. if multiple units selected one task, keep only nearest unit, drop others
 *      3. assign tasks to remaining units (should be 1-to-1)
 * 4. repeat 1 for units without tasks
 */
public class UnitTaskAssignmentSystem : LocalModelUnscalableEcsSystem {
    public EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskTimeoutComponent> filter; // units without tasks
    private readonly UnitNeedActionCreator needActionCreator = new();
    private readonly int maxPriority = range.max;
    private readonly int minPriority = range.min;

    public UnitTaskAssignmentSystem() {
        name = "UnitTaskAssignmentSystem";
        debug = true;
    }

    public override void Run() {
        List<EcsEntity> remainingUnits = new();
        foreach (int i in filter) {
            remainingUnits.Add(filter.GetEntity(i));
        }
        
        while (remainingUnits.Count > 0) {
            MultiValueDictionary<TaskTargetDescriptor, TaskPerformerDescriptor> assignments = new();
            MoreEnumerable.ForEach();
            remainingUnits
                .Select(findTaskForUnit)
                .Where(assignment => assignment != null)
                .MoreEnumerable.ForEach(assignment => assignments.add(assignment.target, assignment.performer));
            if (assignments.Count == 0) return;
            foreach ((TaskTargetDescriptor target, List<TaskPerformerDescriptor> units) in assignments) {
                TaskPerformerDescriptor performer = units.Count > 0
                    ? selectUnitForTask(target, units)
                    : units[0];
                assignTask(performer.unit, target);
                remainingUnits.Remove(performer.unit);
            }
        }
    }

    private TaskAssignmentDescriptor findTaskForUnit(EcsEntity unit) {
        if (unit.Has<UnitNextTaskComponent>()) return createNextTask(unit);
        TaskAssignmentDescriptor needDescriptor = needActionCreator.getMaxPriorityPerformableNeedAction(model, unit);
        int needPriority = needDescriptor?.performer.priority ?? NONE;
        EcsEntity jobTask = getTaskFromContainer(unit, needPriority);
        if (jobTask != EcsEntity.Null) { // more prioritized job task exists
            Vector3Int target = jobTask.take<TaskActionsComponent>().initialAction.target.pos;
            return new TaskAssignmentDescriptor(jobTask, target, "task", unit, jobTask.take<TaskJobComponent>().priority);
        }
        return null;
        // return createIdleTask(unit);
    }

    private TaskAssignmentDescriptor createNextTask(EcsEntity unit) {
        Action action = unit.take<UnitNextTaskComponent>().action;
        unit.Del<UnitNextTaskComponent>();
        EcsEntity task = model.taskContainer.generator
            .createTask(action, Jobs.NONE, model.createEntity(), model);
        return new TaskAssignmentDescriptor(task, action.target.pos, "task", unit, JOB);
    }

    // selection order: priority of unit's job, priority of task, task target proximity
    // if task priority is lower than given need priority, task will be ignored
    private EcsEntity getTaskFromContainer(EcsEntity unit, int needPriority) {
        UnitJobsComponent jobs = unit.take<UnitJobsComponent>();
        Vector3Int position = unit.pos();
        byte area = model.localMap.passageMap.area.get(position);
        for (int jobPriority = maxPriority; jobPriority >= minPriority; jobPriority--) {
            List<string> jobsList = jobs.getByPriority(jobPriority);
            if (jobsList.Count <= 0) continue;
            Dictionary<int, List<EcsEntity>> tasks = model.taskContainer.getTasksByJobs(jobsList, needPriority + 1); // priority -> tasks
            if (tasks.Count == 0) continue;
            for (int taskPriority = maxPriority; taskPriority >= needPriority + 1; taskPriority--) {
                if (!tasks.ContainsKey(taskPriority)) continue;
                List<EcsEntity> tasksOfPriority = tasks[taskPriority];
                while (tasksOfPriority.Count > 0) {
                    EcsEntity nearestTask = selectNearestTask(position, tasksOfPriority);
                    if (checkTaskTarget(nearestTask, area) && checkTaskAssignmentCondition(nearestTask, unit)) return nearestTask;
                    tasksOfPriority.Remove(nearestTask);
                }
            }
        }
        return EcsEntity.Null;
    }

    private bool checkTaskAssignmentCondition(EcsEntity task, EcsEntity unit) {
        return task.take<TaskActionsComponent>().initialAction.assignmentCondition.Invoke(unit) != ActionCheckingEnum.FAIL;
    }

    // checks that task target is accessible from area
    private bool checkTaskTarget(EcsEntity task, byte performerArea) {
        PassageMap passageMap = model.localMap.passageMap;
        ActionTargetTypeEnum targetType = task.take<TaskActionsComponent>().initialAction.target.type;
        Vector3Int target = task.take<TaskActionsComponent>().initialAction.target.pos;
        if (target == Vector3Int.back) {
            logError("target position for " + task.name() + " not found ");
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

    private TaskPerformerDescriptor selectUnitForTask(TaskTargetDescriptor target, List<TaskPerformerDescriptor> units) {
        int maxPriority = units.Max(performer => performer.priority);
        return units
            .Where(performer => performer.priority == maxPriority)
            .MinBy(performer => performer.unit.pos().fastDistance(target.targetPosition));
    }

    // bind unit and task entities
    private void assignTask(EcsEntity unit, TaskTargetDescriptor target) {
        // log("[UnitTaskAssignmentSystem] assigning task [" + task.name() + "] to " + unit.name());
        EcsEntity task = target.createTask();
        unit.Replace(new TaskComponent { task = task });
        task.Replace(new TaskPerformerComponent { performer = unit });
        task.Replace(new TaskAssignedComponent());
        model.taskContainer.claimTask(task, unit);
    }

    private EcsEntity selectNearestTask(Vector3Int position, List<EcsEntity> tasks) {
        return tasks
            .MinBy(task => position.fastDistance(task.take<TaskActionsComponent>().initialAction.target.pos));
    }
}

}