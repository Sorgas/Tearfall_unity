using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.target;
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
 * 2. if multiple units selected one task, keep only nearest unit, drop others (tasks for others will be selected on next tick)
 * 3. assign tasks to remaining units (should be 1-to-1)
 * 4. repeat 1 for units without tasks
 */
public class UnitTaskAssignmentSystem : LocalModelUnscalableEcsSystem {
    public EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskTimeoutComponent> filter; // units without tasks
    private readonly UnitNeedActionCreator needActionCreator = new();
    private readonly int maxPriority = range.max;
    private readonly int minPriority = range.min;
    private readonly MultiValueDictionary<TaskTargetDescriptor, TaskPerformerDescriptor> assignments = new();

    public UnitTaskAssignmentSystem() {
        name = "UnitTaskAssignmentSystem";
    }

    public override void Run() {
        assignments.Clear();
        foreach (int i in filter) {
            UnitTaskAssignment assignment = findTaskForUnit(filter.GetEntity(i));
            if (assignment != null) {
                assignments.add(assignment.target, assignment.performer);
            }
        }
        foreach ((TaskTargetDescriptor target, List<TaskPerformerDescriptor> performers) in assignments) {
            TaskPerformerDescriptor performer = performers.Count > 0
                ? selectUnitForTask(target, performers)
                : performers[0];
            assignTask(performer, target);
        }
    }

    private UnitTaskAssignment findTaskForUnit(EcsEntity unit) {
        if (unit.Has<UnitNextTaskComponent>()) return createNextTask(unit);
        UnitTaskAssignment needAssignment = needActionCreator.getMaxPriorityPerformableNeedAction(model, unit);
        int needPriority = needAssignment?.performer.priority ?? NONE;
        EcsEntity jobTask = getTaskFromContainer(unit, needPriority + 1);
        if (jobTask != EcsEntity.Null) { // more prioritized job task exists
            Vector3Int position = jobTask.take<TaskActionsComponent>().initialAction.target.pos;
            return new UnitTaskAssignment(jobTask, position, "task", unit, jobTask.take<TaskJobComponent>().priority);
        }
        if (needAssignment != null) return needAssignment;
        // return createIdleTask(unit);
        return null;
    }

    private UnitTaskAssignment createNextTask(EcsEntity unit) {
        Action action = unit.take<UnitNextTaskComponent>().action;
        unit.Del<UnitNextTaskComponent>();
        EcsEntity task = model.taskContainer.generator
            .createTask(action, Jobs.NONE, model.createEntity(), model);
        return new UnitTaskAssignment(task, action.target.pos, "task", unit, JOB);
    }

    // selection order: priority of unit's job, priority of task, task target proximity
    // if task priority is lower than given priority, task will be ignored
    private EcsEntity getTaskFromContainer(EcsEntity unit, int minTaskPriority) {
        UnitJobsComponent jobs = unit.take<UnitJobsComponent>();
        Vector3Int position = unit.pos();
        ushort area = model.localMap.passageMap.defaultHelper.area.get(position);
        for (int jobPriority = maxPriority; jobPriority >= minPriority; jobPriority--) { // by unit's jobs in priority order
            List<string> jobsList = jobs.getByPriority(jobPriority);
            if (jobsList.Count <= 0) continue;
            Dictionary<int, List<EcsEntity>> tasks = model.taskContainer.getTasksByJobs(jobsList, minTaskPriority); // priority -> tasks
            if (tasks.Count == 0) continue;
            for (int taskPriority = maxPriority; taskPriority >= minTaskPriority; taskPriority--) { // by tasks of jobs in priority order
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
    private bool checkTaskTarget(EcsEntity task, ushort performerArea) {
        PassageMap passageMap = model.localMap.passageMap;
        ActionTarget actionTarget = task.take<TaskActionsComponent>().initialAction.target;
        ActionTargetTypeEnum targetType = task.take<TaskActionsComponent>().initialAction.target.type;
        Vector3Int target = task.take<TaskActionsComponent>().initialAction.target.pos;
        if (target == Vector3Int.back) {
            logError("target position for " + task.name() + " not found ");
            return false;
        }
        return actionTarget.getAcceptablePositions(model)
            .Select(position => model.localMap.passageMap.defaultHelper.area.get(position))
            .Any(area => area == performerArea);
        // target position in same area with performer
        // if (targetType == EXACT || targetType == ANY) {
        //     if (passageMap.area.get(target) == performerArea) return true;
        // }
        // // target position is accessible from performer area
        // if (targetType == NEAR || targetType == ANY) {
        //     NeighbourPositionStream stream = new(target, model);
        //     stream = task.Has<TaskBlockOverrideComponent>()
        //         ? stream.filterConnectedToCenterWithOverrideTile(task.take<TaskBlockOverrideComponent>().blockType)
        //         : stream.filterConnectedToCenter();
        //     return stream.collectAreas().Contains(performerArea);
        // }
        // return false;
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
        int priority = units.Max(performer => performer.priority);
        return units
            .Where(performer => performer.priority == priority)
            .MinBy(performer => performer.unit.pos().fastDistance(target.targetPosition));
    }

    // bind unit and task entities
    private void assignTask(TaskPerformerDescriptor performer, TaskTargetDescriptor target) {
        EcsEntity task = target.createTask(performer, model);
        log($"[UnitTaskAssignmentSystem] assigning task [{task.name()}] to {performer.unit.name()}");
        performer.unit.Replace(new TaskComponent { task = task });
        task.Replace(new TaskPerformerComponent { performer = performer.unit });
        task.Replace(new TaskAssignedComponent());
        model.taskContainer.claimTask(task, performer.unit);
    }

    private EcsEntity selectNearestTask(Vector3Int position, List<EcsEntity> tasks) {
        return tasks
            .MinBy(task => position.fastDistance(task.take<TaskActionsComponent>().initialAction.target.pos));
    }
}
}