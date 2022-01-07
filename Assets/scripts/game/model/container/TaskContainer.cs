using System.Collections.Generic;
using Leopotam.Ecs;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.container {
    public class TaskContainer {
        private Dictionary<string, HashSet<EcsEntity>> tasks = new Dictionary<string, HashSet<EcsEntity>>();

        public void addTask(EcsEntity task) {
            TaskJobComponent? job = task.get<TaskJobComponent>();
            string jobName = job.HasValue ? job.Value.job : "none";
            tasks[jobName].Add(task);
        }
    }
}