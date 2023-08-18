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

namespace game.model.system.unit { 
/**
 * 1. find tasks for all units (without assignment)
 *      2. if only one unit selected task, assign
 *      3. if multiple units selected one task, assign to nearest unit
 * 4. repeat 1 for units without tasks
 */
public class UnitTaskAssignmentSystem : LocalModelUnscalableEcsSystem {
    public EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskTimeoutComponent> filter; // units without tasks
    private readonly UnitNeedActionCreator needActionCreator = new();
    private readonly int maxPriority = TaskPriorities.range.max;
    private readonly int minPriority = TaskPriorities.range.min;
    
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
            MultiValueDictionary<EcsEntity, EcsEntity> taskToUnits = new();
            foreach (EcsEntity unit in remainingUnits) {
                EcsEntity task = findTaskForUnit(unit);
                if (task != EcsEntity.Null) {
                    taskToUnits.add(task, unit);
                }
            }
            if (taskToUnits.Keys.All(task => task == EcsEntity.Null)) return;
            foreach ((EcsEntity task, List<EcsEntity> units) in taskToUnits) {
                EcsEntity unit = units.Count > 0
                    ? selectUnitForTask(task, units)
                    : units[0];
                assignTask(unit, task);
                remainingUnits.Remove(unit);
            }
        }
    }

    // TODO compare max priority of needs and tasks before lookup in container
    private EcsEntity findTaskForUnit(EcsEntity unit) {
        if (unit.Has<UnitNextTaskComponent>()) return createNextTask(unit);
        EcsEntity jobTask = getTaskFromContainer(unit);
        EcsEntity needTask = needActionCreator.selectAndCreateAction(model, unit);
        EcsEntity task = priority(jobTask) > priority(needTask) ? jobTask : needTask;
        // if (task.IsNull()) task = createIdleTask(unit);
        return task;
    }
    
    private EcsEntity getNearestTask(List<EcsEntity> tasks, Vector3Int position) {
        return tasks
            .MinBy(task => (task.take<TaskActionsComponent>().initialAction.target.pos - position).sqrMagnitude);
    }
    
    private EcsEntity createNextTask(EcsEntity unit) {
        return model.taskContainer.generator
            .createTask(unit.take<UnitNextTaskComponent>().action, Jobs.NONE, model.createEntity(), model);
    }

    // selection order: priority of unit's job, priority of task, task target proximity
    private EcsEntity getTaskFromContainer(EcsEntity unit) {
        UnitJobsComponent jobs = unit.take<UnitJobsComponent>();
        PassageMap passageMap = model.localMap.passageMap;
        Vector3Int position = unit.pos();
        byte area = passageMap.area.get(position);
        for (int jobPriority = maxPriority; jobPriority >= minPriority; jobPriority--) {
            List<string> jobsList = jobs.getByPriority(jobPriority);
            if (jobsList.Count <= 0) continue;
            Dictionary<int, List<EcsEntity>> tasks = model.taskContainer.getTasksByJobs(jobsList); // priority -> tasks
            for (int taskPriority = maxPriority; taskPriority >= minPriority; taskPriority--) {
                if (!tasks.ContainsKey(taskPriority)) continue;
                List<EcsEntity> tasksOfPriority = tasks[taskPriority];
                while (tasksOfPriority.Count > 0) {
                    EcsEntity nearestTask = selectNearestTask(position, tasksOfPriority);
                    if(checkTaskTarget(nearestTask, area, passageMap) && checkTaskAssignmentCondition(nearestTask, unit)) return nearestTask;
                    tasksOfPriority.Remove(nearestTask);
                }
            }
        }
        return EcsEntity.Null;
    }

    private bool checkTaskAssignmentCondition(EcsEntity task, EcsEntity unit) {
        return task.take<TaskActionsComponent>().initialAction.assignmentCondition.Invoke(unit) != ActionCheckingEnum.FAIL;
    }
    
    private bool checkTaskTarget(EcsEntity task, byte performerArea, PassageMap passageMap) {
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

    private EcsEntity selectUnitForTask(EcsEntity task, List<EcsEntity> units) {
        Vector3Int taskTargetPosition = task.take<TaskActionsComponent>().initialAction.target.pos;
        return units.MinBy(unit => unit.pos().fastDistance(taskTargetPosition));
    }
    
    // bind unit and task entities
    private void assignTask(EcsEntity unit, EcsEntity task) {
        log("[UnitTaskAssignmentSystem] assigning task [" + task.name() + "] to " + unit.name());
        unit.Replace(new TaskComponent { task = task });
        task.Replace(new TaskPerformerComponent { performer = unit });
        task.Replace(new TaskAssignedComponent());
        model.taskContainer.claimTask(task, unit);
    }

    private int priority(EcsEntity task) {
        return task.IsNull() ? TaskPriorities.NONE : task.take<TaskJobComponent>().priority;
    }
    
    private EcsEntity selectNearestTask(Vector3Int position, List<EcsEntity> tasks) {
        return tasks
            .MinBy(task => position.fastDistance(task.take<TaskActionsComponent>().initialAction.target.pos));
    }
}
}