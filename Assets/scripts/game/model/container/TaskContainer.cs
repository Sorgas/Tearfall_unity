using System.Collections.Generic;
using enums.action;
using game.model.localmap.passage;
using Leopotam.Ecs;
using types.unit;
using UnityEngine;
using util.lang.extension;
using static enums.action.ActionTargetTypeEnum;
using static game.model.component.task.TaskComponents;

namespace game.model.container {
    // contains all shared tasks for settlers. Personal tasks like eating or resting are not handled
    // only contains tasks with TaskJobComponent
    public class TaskContainer : LocalMapModelComponent {
        public TaskGenerator generator = new();

        // private Dictionary<string, HashSet<EcsEntity>> tasks = new();
        private Dictionary<string, HashSet<EcsEntity>> openTasks = new(); // job name to tasks
        private Dictionary<EcsEntity, EcsEntity> assigned = new(); // task to performer
        public int openTaskCount = 0;
        public int assignedTaskCount = 0;

        public TaskContainer(LocalModel model) : base(model) {
            foreach (var job in Jobs.jobs) {
                openTasks.Add(job.name, new HashSet<EcsEntity>());
            }
            openTasks.Add("none", new HashSet<EcsEntity>());
        }

        // registers open task in container. Then it can be assigned with UnitTaskAssignmentSystem
        public void addOpenTask(EcsEntity task) {
            TaskJobComponent? job = task.optional<TaskJobComponent>();
            string jobName = job.HasValue ? job.Value.job : "none";
            if (openTasks[jobName].Contains(task)) Debug.LogError("Task " + task.name() + "already registered!");
            openTasks[jobName].Add(task);
            openTaskCount++;
        }

        // returns task appropriate for unit, but does not removes task from container
        // TODO add priority sorting
        public EcsEntity findTask(List<string> jobs, Vector3Int position) {
            PassageMap passageMap = model.localMap.passageMap;
            byte performerArea = passageMap.area.get(position);
            foreach (var job in jobs) {
                if (openTasks[job].Count > 0) {
                    EcsEntity task = openTasks[job].firstOrDefault(task => {
                        ActionTargetTypeEnum targetType = task.take<TaskActionsComponent>().initialAction.target.type;
                        Vector3Int target = getTaskTargetPosition(task);
                        // target position in same area with performer
                        if (targetType == EXACT || targetType == ANY) {
                            if (passageMap.area.get(target) == performerArea) return true;
                        }
                        // target position is accessible from performer area
                        if (targetType == NEAR || targetType == ANY) {
                            if (task.Has<TaskBlockOverrideComponent>()) {
                                return new NeighbourPositionStream(target, model)
                                    .filterConnectedToCenterWithOverrideTile(task.take<TaskBlockOverrideComponent>().blockType)
                                    .collectAreas().Contains(performerArea);
                            }
                            return new NeighbourPositionStream(target, model)
                                .filterConnectedToCenter()
                                .collectAreas().Contains(performerArea);
                        }
                        return false;
                    }, EcsEntity.Null);
                    if (!task.IsNull()) return task;
                }
            }
            return EcsEntity.Null;
        }

        public void removeTask(EcsEntity task) {
            if (task.Has<TaskJobComponent>()) {
                string job = task.take<TaskJobComponent>().job;
                if (openTasks[job].Contains(task)) {
                    openTasks[job].Remove(task);
                    openTaskCount--;
                } else if (assigned.ContainsKey(task)) {
                    assigned.Remove(task);
                    assignedTaskCount--;
                } else {
                    Debug.LogError("Deleting task " + task.name() + "with job " + job + " but not from container!");
                }
            }
            Debug.Log(task.name() + " destroyed");
            task.Destroy();
        }

        // removes task from open tasks
        public void claimTask(EcsEntity task, EcsEntity performer) {
            string job = task.take<TaskJobComponent>().job;
            openTasks[job].Remove(task);
            openTaskCount--;
            assigned.Add(task, performer);
            assignedTaskCount++;
        }

        public void taskCompleted(EcsEntity task) {
            assigned.Remove(task);
            assignedTaskCount--;
        }

        // tasks in container always have target
        private Vector3Int getTaskTargetPosition(EcsEntity task) {
            Vector3Int? target = task.take<TaskActionsComponent>().initialAction.target.getPos();
            if (target.HasValue) return target.Value;
            throw new EcsException("Task " + task.name() + " has no target position ");
        }

        private TaskPriorityEnum priority(EcsEntity task) {
            return task.take<TaskPriorityComponent>().priority;
        }
    }
}