using System.Collections.Generic;
using enums.action;
using enums.unit;
using game.model.localmap.passage;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.container {
    // contains all shared tasks for settlers. Personal tasks like eating or resting are not handled
    // only contains tasks with TaskJobComponent
    public class TaskContainer {
        public TaskGenerator generator = new();
        
        
        // private Dictionary<string, HashSet<EcsEntity>> tasks = new();
        private Dictionary<string, HashSet<EcsEntity>> openTasks = new(); // job name to tasks
        private Dictionary<EcsEntity, EcsEntity> assigned = new(); // task to performer

        public TaskContainer() {
            foreach (var job in JobsEnum.jobs) {
                openTasks.Add(job.name, new HashSet<EcsEntity>());
                // tasks.Add(job.name, new HashSet<EcsEntity>());
            }
            openTasks.Add("none", new HashSet<EcsEntity>());
            // tasks.Add("none", new HashSet<EcsEntity>());
        }

        // registers open task in container. Then it can be assigned with UnitTaskAssignmentSystem
        public void addOpenTask(EcsEntity task) {
            TaskJobComponent? job = task.optional<TaskJobComponent>();
            string jobName = job.HasValue ? job.Value.job : "none";
            if(openTasks[jobName].Contains(task)) Debug.LogError("Task "+ task.name() + "already registered!");
            openTasks[jobName].Add(task);
        }
        
        // returns task appropriate for unit, but does not removes task from container
        // TODO add priority sorting
        public EcsEntity findTask(List<string> jobs, Vector3Int position) {
            PassageMap passageMap = GameModel.localMap.passageMap;
            foreach (var job in jobs) {
                if (openTasks[job].Count > 0) {
                    EcsEntity task = openTasks[job].firstOrDefault(task => passageMap.inSameArea(position, getTaskTargetPosition(task)), EcsEntity.Null);
                    if (!task.IsNull()) return task;
                }
            }
            return EcsEntity.Null;
        }

        public void removeTask(EcsEntity task) {
            string job = task.take<TaskJobComponent>().job;
            if(task.Has<TaskPerformerComponent>()) Debug.LogError("Task with performer is removed from container!");
            if (openTasks[job].Contains(task)) {
                openTasks[job].Remove(task);
            }
            // if (tasks[job].Contains(task)) {
            //     tasks[job].Remove(task);
            // }
            task.Destroy();
        }

        // removes task from open tasks
        public void claimTask(EcsEntity task, EcsEntity performer) {
            string job = task.take<TaskJobComponent>().job;
            openTasks[job].Remove(task);
            assigned.Add(task, performer);
        }

        public void taskCompleted(EcsEntity task) {
            assigned.Remove(task);
        }

        // tasks in container always have target
        private Vector3Int getTaskTargetPosition(EcsEntity task) {
            Vector3Int? target = task.takeRef<TaskActionsComponent>().initialAction.target.getPos();
            if (target.HasValue) return target.Value;
            throw new EcsException("Task " + task.name() + " has no target position ");
        }

        private TaskPriorityEnum priority(EcsEntity task) {
            return task.take<TaskPriorityComponent>().priority;
        }
    }
}