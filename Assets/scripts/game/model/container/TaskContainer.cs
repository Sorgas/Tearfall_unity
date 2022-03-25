using System.Collections.Generic;
using System.Linq;
using enums.unit;
using game.model.component.task.action;
using game.model.localmap.passage;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.container {
    public class TaskContainer {
        private Dictionary<string, HashSet<EcsEntity>> openTasks = new();
        private Dictionary<string, HashSet<EcsEntity>> tasks = new();

        public TaskContainer() {
            foreach (var job in JobsEnum.jobs) {
                openTasks.Add(job.name, new HashSet<EcsEntity>());
                tasks.Add(job.name, new HashSet<EcsEntity>());
            }
            openTasks.Add("none", new HashSet<EcsEntity>());
            tasks.Add("none", new HashSet<EcsEntity>());
        }

        public void addTask(EcsEntity task) {
            TaskJobComponent? job = task.optional<TaskJobComponent>();
            string jobName = job.HasValue ? job.Value.job : "none";
            openTasks[jobName].Add(task);
        }

        // gets task with given job and reachable target
        // TODO add priority sorting
        public EcsEntity? getTask(string job, Vector3Int position) {
            if (openTasks[job].Count > 0) {
                PassageMap passageMap = GameModel.localMap.passageMap;
                EcsEntity task = openTasks[job]
                    .First(entity => passageMap.inSameArea(position,
                        entity.Get<TaskActionsComponent>()
                            .initialAction.target.getPos()
                            .Value));
                openTasks[job].Remove(task);
            }
            return null;
        }

        public EcsEntity createTask(Action initialAction) {
            EcsEntity task = GameModel.get().createEntity();
            task.Replace(new TaskActionsComponent { initialAction = initialAction, preActions = new List<Action>() });
            initialAction.task = task;
            return task;
        }

        public void removeTask(EcsEntity task) {
            string job = task.take<TaskJobComponent>().job;
            if (openTasks[job].Contains(task)) {
                openTasks[job].Remove(task);
                task.Destroy();
            }
            if (tasks[job].Contains(task)) {
                tasks[job].Remove(task);
                task.Destroy();
            }
        }
    }
}