using System.Collections.Generic;
using enums.action;
using enums.unit;
using game.model.localmap.passage;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static enums.action.ActionTargetTypeEnum;
using static enums.PassageEnum;
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
            if (openTasks[jobName].Contains(task)) Debug.LogError("Task " + task.name() + "already registered!");
            openTasks[jobName].Add(task);
        }

        // returns task appropriate for unit, but does not removes task from container
        // TODO add priority sorting
        public EcsEntity findTask(List<string> jobs, Vector3Int position) {
            PassageMap passageMap = GameModel.localMap.passageMap;
            byte performerArea = passageMap.area.get(position);
            foreach (var job in jobs) {
                if (openTasks[job].Count > 0) {
                    EcsEntity task = openTasks[job].firstOrDefault(task => {
                        ActionTargetTypeEnum targetType = task.take<TaskActionsComponent>().initialAction.target.type;
                        Vector3Int target = getTaskTargetPosition(task);
                        // target position in same area with performer
                        return ((targetType == EXACT || targetType == ANY) && passageMap.area.get(target) == performerArea) ||
                        // performer can access target tile from his area
                        (targetType == NEAR || targetType == ANY) && new NeighbourPositionStream(target).filterByPassage(PASSABLE).collectAreas().Contains(performerArea);
                    }, EcsEntity.Null);
                    if (!task.IsNull()) return task;
                }
            }
            return EcsEntity.Null;
        }

        public void removeTask(EcsEntity task) {
            if (task.Has<TaskJobComponent>()) {
                string job = task.take<TaskJobComponent>().job;
                if (task.Has<TaskPerformerComponent>()) Debug.LogError("Task with performer is removed from container!");
                if (openTasks[job].Contains(task)) {
                    openTasks[job].Remove(task);
                }
            }
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
            Vector3Int? target = task.take<TaskActionsComponent>().initialAction.target.getPos();
            if (target.HasValue) return target.Value;
            throw new EcsException("Task " + task.name() + " has no target position ");
        }

        private TaskPriorityEnum priority(EcsEntity task) {
            return task.take<TaskPriorityComponent>().priority;
        }
    }
}