using System.Collections.Generic;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using types.unit;

namespace game.model.container.task {
    // creates task entities
    public class TaskGenerator {
        public EcsEntity createTask(Action initialAction, Job job, EcsEntity entity, LocalModel model) =>
            createTask(initialAction, job, TaskPriorities.JOB, entity, model);

        public EcsEntity createTask(Action initialAction, Job job, int priority, EcsEntity entity, LocalModel model) {
            entity.Replace(new TaskActionsComponent {
                initialAction = initialAction, preActions = new List<Action>(),
                model = model
            });
            entity.Replace(new TaskLockedItemsComponent { lockedItems = new() });
            entity.Replace(new NameComponent { name = initialAction.name });
            entity.Replace(new TaskJobComponent { job = job.name, priority = priority });
            initialAction.task = entity;
            return entity;
        }
    }
}