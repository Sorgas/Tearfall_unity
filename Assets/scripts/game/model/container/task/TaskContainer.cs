using System.Collections.Generic;
using game.model.component.task;
using game.model.localmap;
using game.model.localmap.passage;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.container.task {
    // contains all shared tasks for settlers. Personal tasks like eating or resting are not handled
    // only contains tasks with TaskJobComponent
    public class TaskContainer : LocalModelContainer {
        public readonly TaskGenerator generator = new();
        private readonly TaskCompletionUtil taskCompletionUtil;
        private readonly OpenTaskCollection open;
        private readonly Dictionary<EcsEntity, EcsEntity> assigned = new(); // task -> performer
        private readonly HashSet<EcsEntity> delayedTasks = new();
        private int assignedTaskCount => assigned.Count;
        private const bool debug = true;

        public TaskContainer(LocalModel model) : base(model) {
            open = new(debug);
            taskCompletionUtil = new(model);
        }

        // registers open task in container. Then it can be assigned with UnitTaskAssignmentSystem
        public void addOpenTask(EcsEntity task) => open.add(task);

        public Dictionary<int, List<EcsEntity>> getTasksByJobs(List<string> jobs) => open.get(jobs);

        public void moveOpenTaskToDelayed(EcsEntity task) {
            if (task.Has<TaskPerformerComponent>()) {
                throw new GameException("Task with performer moved to timeout map");
            }
            open.remove(task);
            delayedTasks.Add(task);
            Debug.Log("Open task " + task.name() + " moved to delayed");
        }

        public void moveDelayedTaskToOpen(EcsEntity task) {
            delayedTasks.Remove(task);
            open.add(task);
            Debug.Log("Delayed task " + task.name() + " moved to open");
        }

        // removes and destroys task. updates linked entities
        public void removeTask(EcsEntity task, TaskStatusEnum status) {
            removeTaskFromContainer(task); // removes task from container maps
            taskCompletionUtil.complete(task, status); // updates linked entities
            log(task.name() + " destroyed");
            task.Destroy();
        }

        // moves task from open tasks to assigned
        public void claimTask(EcsEntity task, EcsEntity performer) {
            if (open.contains(task)) open.remove(task);
            assigned.Add(task, performer);
        }

        private void removeTaskFromContainer(EcsEntity task) {
            if (open.contains(task)) {
                open.remove(task);
            } else if (assigned.ContainsKey(task)) {
                assigned.Remove(task);
            } else if (delayedTasks.Contains(task)) {
                delayedTasks.Remove(task);
            } else {
                Debug.LogErrorFormat("[TaskContainer] Deleting task {0}, but not found in task container!", task.name());
            }
        }

        private void log(string message) {
            if (debug) Debug.Log("[TaskContainer]: " + message);
        }

        public string getDebugIngo() => "TaskContainer: assigned: " + assignedTaskCount + " \n";
    }
}