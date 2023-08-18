using System.Collections.Generic;
using game.model.component.task;
using Leopotam.Ecs;
using types.action;
using types.unit;
using UnityEditor.SceneManagement;
using UnityEngine;
using util;
using util.lang.extension;

namespace game.model.container.task {

    // wrapper class for open tasks. handles storing tasks by jobs and priorities
    public class OpenTaskCollection {
        private readonly Dictionary<string, Dictionary<int, HashSet<EcsEntity>>> openTasks = new(); // job -> priority -> tasks
        private readonly bool debug;
        
        public OpenTaskCollection(bool debug) {
            this.debug = debug;
            foreach (Job job in Jobs.all) {
                openTasks.Add(job.name, new Dictionary<int, HashSet<EcsEntity>>());
                for (int i = TaskPriorities.range.min; i <= TaskPriorities.range.max; i++) {
                    openTasks[job.name].Add(i, new HashSet<EcsEntity>());
                }
            }
        }

        public void add(EcsEntity task) {
            TaskJobComponent component = task.take<TaskJobComponent>();
            string job = component.job;
            int priority = component.priority;
            if (openTasks[job][priority].Contains(task))
                throw new GameException("[TaskContainer.open] Task " + task.name() + " already registered!");
            openTasks[job][priority].Add(task);
            Debug.Log("[TaskContainer.open] added task [" + task.name() + "], " + job + " " + priority);
        }

        public void remove(EcsEntity task) {
            TaskJobComponent component = task.take<TaskJobComponent>();
            string job = component.job;
            int priority = component.priority;
            if(!openTasks[job][priority].Contains(task))
                throw new GameException("[TaskContainer.open] Task " + task.name() + " not registered!");
            openTasks[job][priority].Remove(task);
        }

        // returns map of (priority -> tasks)
        public Dictionary<int, List<EcsEntity>> get(List<string> jobs) {
            Dictionary<int, List<EcsEntity>> result = new();
            for (int priority = TaskPriorities.range.max; priority >= TaskPriorities.range.min; priority--) {
                result.Add(priority, new());
                foreach (string job in jobs) {
                    result[priority].AddRange(openTasks[job][priority]);
                }
            }
            return result;
        }

        public bool contains(EcsEntity task) {
            TaskJobComponent component = task.take<TaskJobComponent>(); // TODO 
            string job = component.job;
            int priority = component.priority;
            return openTasks[job][priority].Contains(task);
        }
    }
}