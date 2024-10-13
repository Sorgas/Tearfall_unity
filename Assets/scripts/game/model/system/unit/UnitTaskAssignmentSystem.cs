using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.combat;
using game.model.component.task.action.target;
using game.model.component.unit;
using Leopotam.Ecs;
using MoreLinq;
using types;
using types.action;
using types.unit;
using UnityEngine;
using util;
using util.geometry;
using util.lang;
using util.lang.extension;
using static types.action.TaskPriorities;

namespace game.model.system.unit {
/**
 * 1. Find task targets for all units (without assignment), save into assignments map.
 * 2. If target is selected by one unit, create and assign task.  
 * 3. If target is selected by multiple units, create and assign task to nearest one. (tasks for others will be selected on next tick).
 */
public class UnitTaskAssignmentSystem : LocalModelUnscalableEcsSystem {
    public EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskTimeoutComponent> filter; // units without tasks
    private readonly UnitNeedActionCreator needActionCreator = new();
    private readonly int maxPriority = range.max;
    private readonly int minPriority = range.min;
    // for cases when many units select same task
    private readonly MultiValueDictionary<TaskTargetDescriptor, TaskPerformerDescriptor> assignments = new();
    
    public UnitTaskAssignmentSystem() {
        name = "UnitTaskAssignmentSystem";
        debug = true;
    }
    
    public override void Run() {
        assignments.Clear();
        foreach (int i in filter) {
            EcsEntity unit = filter.GetEntity(i);
            if(unit.Has<UnitDraftedComponent>()) continue; // TODO add special UnitTaskReceivingComponent
            string faction = unit.take<FactionComponent>().name;
            if ("player".Equals(faction)) {
                addAssignment(findTaskAssignmentForPlayerUnit(unit));
            } else {
                createTaskForNonPlayerUnit(unit);
            }
        }
        foreach ((TaskTargetDescriptor target, List<TaskPerformerDescriptor> performers) in assignments) {
            TaskPerformerDescriptor performer = performers.Count > 0
                ? selectUnitForTask(target, performers)
                : performers[0];
            assignTaskFromDescriptors(performer, target);
        }
    }

    // Creates task assignment with task in TaskContainer or with task from by unit's needs.
    private UnitTaskAssignment findTaskAssignmentForPlayerUnit(EcsEntity unit) {
        if (unit.Has<UnitNextTaskComponent>()) {
            return createNextTask(unit);
        }
        UnitTaskAssignment needAssignment = needActionCreator.getMaxPriorityPerformableNeedAction(model, unit);
        int needPriority = needAssignment?.performer.priority ?? NONE;
        EcsEntity jobTask = getTaskFromContainer(unit, needPriority + 1);
        if (jobTask != EcsEntity.Null) { // more prioritized job task exists
            Vector3Int position = jobTask.take<TaskActionsComponent>().initialAction.target.pos;
            return new UnitTaskAssignment(jobTask, position, "task", unit, jobTask.take<TaskJobComponent>().priority);
        }
        return needAssignment;
        // return createIdleTask(unit);
    }
    
    private void addAssignment(UnitTaskAssignment assignment) {
        if (assignment != null) {
            assignments.add(assignment.target, assignment.performer);
        }
    }
    
    // Creates task for unit by its group mission.
    private void createTaskForNonPlayerUnit(EcsEntity unit) {
        string faction = unit.take<FactionComponent>().name;
        string group = unit.take<FactionComponent>().unitGroup;
        Debug.Log($"looking task for unit from {faction}-{group}" );
        string mission = model.factionContainer.factions[faction].groups[group].mission;
        EcsEntity task = EcsEntity.Null;
        if (mission.Equals(UnitGroupMissions.HOSTILE_AGGRESSIVE)) {
            Action action = new AggressiveCombatAction();
            task = model.taskContainer.generator
                .createTask(action, Jobs.NONE, model.createEntity(), model);
        } else {
            throw new GameException($"Unsupported unit group mission: {mission}");
        }
        if (task != EcsEntity.Null) assignTask(unit, task);
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
        for (int jobPriority = Jobs.PRIORITIES_COUNT - 1; jobPriority >= 1; jobPriority--) { // by unit's jobs in priority order
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
        ActionTarget actionTarget = task.take<TaskActionsComponent>().initialAction.target;
        Vector3Int target = task.take<TaskActionsComponent>().initialAction.target.pos;
        if (target == Vector3Int.back) {
            logError("target position for " + task.name() + " not found ");
            return false;
        }
        // any position of target is accessible by performer
        return actionTarget.getAcceptablePositions(model)
            .Select(position => model.localMap.passageMap.defaultHelper.area.get(position))
            .Any(area => area == performerArea);
    }

    private EcsEntity createIdleTask(EcsEntity unit) {
        Vector3Int current = unit.pos();
        // Debug.Log("creating idle task for unit in position " + current);
        Vector3Int? position = model.localMap.util.getRandomPosition(current, 10, 4);
        return position.HasValue
            ? model.taskContainer.generator.createTask(new MoveAction(position.Value), Jobs.NONE, model.createEntity(), model)
            : EcsEntity.Null;
    }

    // selects nearest performer for task
    private TaskPerformerDescriptor selectUnitForTask(TaskTargetDescriptor target, List<TaskPerformerDescriptor> units) {
        int priority = units.Max(performer => performer.priority);
        return units
            .Where(performer => performer.priority == priority)
            .MinBy(performer => performer.unit.pos().fastDistance(target.targetPosition));
    }

    // bind unit and task entities
    private void assignTaskFromDescriptors(TaskPerformerDescriptor performer, TaskTargetDescriptor target) {
        EcsEntity task = target.createTask(performer, model);
        assignTask(performer.unit, task);
    }

    private void assignTask(EcsEntity unit, EcsEntity task) {
        log($"[UnitTaskAssignmentSystem] assigning task [{task.name()}] to {unit.name()}");
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