using System.Collections.Generic;
using System.Linq;
using game.model.component.task;
using game.model.localmap;
using game.model.localmap.passage;
using Leopotam.Ecs;
using types.action;
using types.unit;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.container.task {
    // contains all shared tasks for settlers. Personal tasks like eating or resting are not handled
    // only contains tasks with TaskJobComponent
    public class TaskContainer : LocalModelContainer {
        public readonly TaskGenerator generator = new();
        private const bool debug = true;
        private readonly TaskCompletionUtil taskCompletionUtil = new();

        private readonly Dictionary<string, Dictionary<int, HashSet<EcsEntity>>> openTasks = new(); // job name -> priority -> tasks
        private readonly Dictionary<EcsEntity, EcsEntity> assigned = new(); // task -> performer
        private int openTaskCount => openTasks.Count;
        private int assignedTaskCount => assigned.Count;

        public TaskContainer(LocalModel model) : base(model) {
            foreach (Job job in Jobs.all) {
                openTasks.Add(job.name, new Dictionary<int, HashSet<EcsEntity>>());
                for (int i = TaskPriorities.range.min; i <= TaskPriorities.range.max; i++) {
                    openTasks[job.name].Add(i, new HashSet<EcsEntity>());
                }
            }
        }

        // registers open task in container. Then it can be assigned with UnitTaskAssignmentSystem
        public void addOpenTask(EcsEntity task) {
            string jobName = getTaskJob(task);
            int priority = task.take<TaskJobComponent>().priority;
            if (!openTasks[jobName][priority].Contains(task)) {
                openTasks[jobName][priority].Add(task);
                Debug.Log("[TaskContainer] added task [" + task.name() + "], " + jobName + " " + priority);
            } else {
                Debug.LogError("Task " + task.name() + "already registered!");
            }
        }

        // finds task for unit. checks unit's job and task target. does not removes task from container
        // TODO add priority sorting
        public EcsEntity findTask(List<string> jobs, Vector3Int position) {
            PassageMap passageMap = model.localMap.passageMap;
            byte area = passageMap.area.get(position);
            List<EcsEntity> result = new();
            foreach (var job in jobs) {
                Dictionary<int, HashSet<EcsEntity>> tasks = openTasks[job];
                for (int i = TaskPriorities.range.max; i <= TaskPriorities.range.min; i--) {
                    tasks[i]
                        .Where(task => checkTaskTarget(task, area, passageMap))
                        
                    openTasks[job.name].Add(i, new HashSet<EcsEntity>());
                }
                if (openTasks[job].Count <= 0) continue;
                EcsEntity task = openTasks[job]
                    .firstOrDefault(task => checkTaskTarget(task, area, passageMap), EcsEntity.Null);
                if (task != EcsEntity.Null) return task;
            }
            return EcsEntity.Null;
        }

        public List<EcsEntity> findTasks(List<string> jobs, Vector3Int position) {
            List<EcsEntity> result = new();
            PassageMap passageMap = model.localMap.passageMap;
            byte area = passageMap.area.get(position);
            foreach (var job in jobs) {
                Dictionary<int, HashSet<EcsEntity>> tasks = openTasks[job];
                for (int i = TaskPriorities.range.max; i <= TaskPriorities.range.min; i--) {
                    tasks[i]
                        .Where(task => checkTaskTarget(task, area, passageMap))
                        
                    openTasks[job.name].Add(i, new HashSet<EcsEntity>());
                }
                if (openTasks[job].Count <= 0) continue;
                EcsEntity task = openTasks[job]
                    .firstOrDefault(task => checkTaskTarget(task, area, passageMap), EcsEntity.Null);
                if (task != EcsEntity.Null) return task;
            }
            return result;
        }

        // removes and destroys task. updates linked entities
        public void removeTask(EcsEntity task, TaskStatusEnum status) {
            removeTaskFromContainer(task); // removes task from container maps
            taskCompletionUtil.complete(task, status); // updates linked entities
            task.Destroy();
            log(task.name() + " destroyed");
        }

        // moves task from open tasks to assigned
        public void claimTask(EcsEntity task, EcsEntity performer) {
            string job = task.take<TaskJobComponent>().job;
            
            openTasks[job].Remove(task);
            assigned.Add(task, performer);
        }

        private void removeTaskFromContainer(EcsEntity task) {
            string job = getTaskJob(task);
            if (openTasks[job].Contains(task)) {
                openTasks[job].Remove(task);
            } else if (assigned.ContainsKey(task)) {
                assigned.Remove(task);
            } else {
                Debug.LogErrorFormat("Deleting task {0} with job {1}, but not found in task container!", task.name(), job);
            }
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
                NeighbourPositionStream stream = new NeighbourPositionStream(target, model);
                stream = task.Has<TaskBlockOverrideComponent>()
                    ? stream.filterConnectedToCenterWithOverrideTile(task.take<TaskBlockOverrideComponent>().blockType)
                    : stream.filterConnectedToCenter();
                return stream.collectAreas().Contains(performerArea);
            }
            return false;
        }

        private string getTaskJob(EcsEntity task) {
            return task.Has<TaskJobComponent>() ? task.take<TaskJobComponent>().job : Jobs.NONE.name;
        }

        private void log(string message) {
            if (debug) Debug.Log("[TaskContainer]: " + message);
        }

        public string getDebugIngo() => "TaskContainer: open: " + openTaskCount + " assigned: " + assignedTaskCount + " \n";
    }
}