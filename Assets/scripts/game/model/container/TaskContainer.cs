using System.Collections.Generic;
using game.model.component.task;
using game.model.localmap;
using game.model.localmap.passage;
using Leopotam.Ecs;
using types.action;
using types.unit;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionTargetTypeEnum;

namespace game.model.container {
    // contains all shared tasks for settlers. Personal tasks like eating or resting are not handled
    // only contains tasks with TaskJobComponent
    public class TaskContainer : LocalModelContainer {
        public TaskGenerator generator = new();
        public bool debug = true;
        
        // TODO have separate open tasks dictionaries for different priorities
        private Dictionary<string, HashSet<EcsEntity>> openTasks = new(); // job name to tasks
        private Dictionary<EcsEntity, EcsEntity> assigned = new(); // task to performer
        public int openTaskCount => openTasks.Count;
        public int assignedTaskCount => assigned.Count;

        public TaskContainer(LocalModel model) : base(model) {
            foreach (var job in Jobs.jobs) {
                openTasks.Add(job.name, new HashSet<EcsEntity>());
            }
            openTasks.Add(Jobs.NONE.name, new HashSet<EcsEntity>());
        }

        // registers open task in container. Then it can be assigned with UnitTaskAssignmentSystem
        public void addOpenTask(EcsEntity task) {
            string jobName = getTaskJob(task);
            Debug.Log("[TaskContainer] adding task " + task.name() + " to " + jobName);
            if (openTasks[jobName].Contains(task)) Debug.LogError("Task " + task.name() + "already registered!");
            openTasks[jobName].Add(task);
        }

        // finds task for unit. checks unit's job and task target. does not removes task from container
        // TODO add priority sorting
        public EcsEntity findTask(List<string> jobs, Vector3Int position) {
            PassageMap passageMap = model.localMap.passageMap;
            byte area = passageMap.area.get(position);
            foreach (var job in jobs) {
                if (openTasks[job].Count <= 0) continue;
                EcsEntity task = openTasks[job]
                    .firstOrDefault(task => checkTaskTarget(task, area, passageMap), EcsEntity.Null);
                if (task != EcsEntity.Null) return task;
            }
            return EcsEntity.Null;
        }

        // removes and destroys task
        public void removeTask(EcsEntity task) {
            string job = task.Has<TaskJobComponent>()
                ? task.take<TaskJobComponent>().job
                : Jobs.NONE.name; 
            if (openTasks[job].Contains(task)) {
                openTasks[job].Remove(task);
            } else if (assigned.ContainsKey(task)) {
                assigned.Remove(task);
            } else {
                Debug.LogError("Deleting task " + task.name() + " with job " + job + ", but not found in task container!");
            }
            log(task.name() + " destroyed");
            task.Destroy();
        }

        // removes task from open tasks
        public void claimTask(EcsEntity task, EcsEntity performer) {
            string job = task.take<TaskJobComponent>().job;
            openTasks[job].Remove(task);
            assigned.Add(task, performer);
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
            if (task.Has<TaskJobComponent>()) return task.take<TaskJobComponent>().job;
            return Jobs.NONE.name;
        }
        
        private void log(string message) {
            if(debug) Debug.Log("[TaskContainer]: " + message);
        }

        public string getDebugIngo() => "TaskContainer: open: " + openTaskCount + " assigned: " + assignedTaskCount + " \n";
    }
}