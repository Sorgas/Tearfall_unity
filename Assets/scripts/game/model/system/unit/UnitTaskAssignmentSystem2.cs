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
public class UnitTaskAssignmentSystem2 : LocalModelUnscalableEcsSystem {
    public EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskTimeoutComponent> filter; // units without tasks
    private readonly UnitNeedActionCreator needActionCreator = new();

    public UnitTaskAssignmentSystem2() {
        name = "UnitTaskAssignmentSystem";
        debug = true;
    }

    public override void Run() {
        List<EcsEntity> units = new();
        foreach (int i in filter) {
            units.Add(filter.GetEntity(i));
        }
        
        while (units.Count > 0) {   
            EcsEntity unit = units[0];
            Vector3Int unitPosition = unit.pos();
            UnitJobsComponent jobs = unit.take<UnitJobsComponent>();
            for (int jobPriority = TaskPriorities.range.max; jobPriority >= TaskPriorities.range.min; jobPriority--) {
                List<string> jobList = jobs.getByPriority(jobPriority);
                if (jobList.Count == 0) continue;
                Dictionary<int, List<EcsEntity>> tasks = model.taskContainer.getTasksByJobs(jobList);
                for (int taskPriority = TaskPriorities.range.max; jobPriority >= TaskPriorities.range.min; jobPriority--) {
                    if (!tasks.ContainsKey(taskPriority)) continue;
                    EcsEntity nearestTask = getNearestTask(tasks[taskPriority], unitPosition);
                    if (checkTaskAssignmentCondition(nearestTask, unit)) {
                        // start looking unit for task
                    }
                }
            }
        }
        
        foreach (int i in filter) {
            ref EcsEntity unit = ref filter.GetEntity(i);
            // EcsEntity task = tryCreateTask(unit);
            // if (task != EcsEntity.Null) assignTask(ref unit, task);
        }
    }

    private void findUnitForTask(EcsEntity task, List<EcsEntity> units) {
        string job = task.take<TaskJobComponent>().job;
        float distance = -1;
        foreach (var unit in units) {
            if (unit.take<UnitJobsComponent>().enabledJobs.ContainsKey(job)) {
                
            }
        }
    }

    private void findTaskForUnit(EcsEntity unit, List<EcsEntity> allUnits) {
        
    }

    private EcsEntity getNearestTask(List<EcsEntity> tasks, Vector3Int position) {
        return tasks
            .MinBy(task => (task.take<TaskActionsComponent>().initialAction.target.pos - position).sqrMagnitude);
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
                    if (checkTaskTarget(task, area, passageMap) && checkTaskAssignmentCondition(task, unit)) return task;
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

    // bind unit and task entities
    private void assignTask(ref EcsEntity unit, EcsEntity task) {
        log("[UnitTaskAssignmentSystem] assigning task [" + task.name() + "] to " + unit.name());
        unit.Replace(new TaskComponent { task = task });
        task.Replace(new TaskPerformerComponent { performer = unit });
        task.Replace(new TaskAssignedComponent());
        model.taskContainer.claimTask(task, unit);
    }

    private int priority(EcsEntity task) {
        return task.IsNull() ? TaskPriorities.NONE : task.take<TaskJobComponent>().priority;
    }

    private void findPairs(List<Vector3Int> units, List<Vector3Int> tasks) {
        Vector3Int currentUnit = units[0];
        while (units.Count > 0) {
            KeyValuePair<Vector3Int, Vector3Int> pair = selectTaskForUnit(currentUnit, units, tasks);
            
            Vector3Int nearestTask = selectNearest(currentUnit, tasks);
            Vector3Int nearestUnit = selectNearest(nearestTask, units);
            if (nearestUnit == currentUnit) { // make pair
                units.Remove(currentUnit);
                tasks.Remove(nearestTask);
            } else { // repeat from task perspective
                
            }
        }
    }

    private KeyValuePair<Vector3Int, Vector3Int> selectTaskForUnit(Vector3Int unit, List<Vector3Int> units, List<Vector3Int> tasks) {
        Vector3Int nearestTask = selectNearest(unit, tasks);
        Vector3Int nearestUnit = selectNearest(nearestTask, units);
        if (nearestUnit == unit) {
            return new KeyValuePair<Vector3Int, Vector3Int>(unit, nearestTask);
        } else {
            return selectUnitForTask(nearestTask, units, tasks);
        }
    }
    
    private KeyValuePair<Vector3Int, Vector3Int> selectUnitForTask(Vector3Int task, List<Vector3Int> units, List<Vector3Int> tasks) {
        Vector3Int nearestUnit = selectNearest(task, units);
        Vector3Int nearestTask = selectNearest(nearestUnit, tasks);
        if (nearestTask == task) {
            return new KeyValuePair<Vector3Int, Vector3Int>(nearestUnit, task);
        } else {
            return selectTaskForUnit(nearestUnit, units, tasks);
        }
    }

    private Vector3Int selectNearest(Vector3Int reference, List<Vector3Int> vectors) {
        return vectors
            .MinBy(vector => reference.fastDistance(vector));
    }
}
}