using System.Collections.Generic;
using enums.action;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using Leopotam.Ecs;

namespace game.model.container {
    // creates task entities
    public class TaskGenerator {
        public EcsEntity createTask(Action initialAction, EcsEntity entity, LocalModel model) => createTask(initialAction, TaskPriorityEnum.JOB, entity, model);
        
        public EcsEntity createTask(Action initialAction, TaskPriorityEnum priority, EcsEntity entity, LocalModel model) {
            entity.Replace(new TaskComponents.TaskActionsComponent { initialAction = initialAction, preActions = new List<Action>(), model = model });
            entity.Replace(new TaskComponents.TaskPriorityComponent { priority = priority });
            entity.Replace(new NameComponent { name = initialAction.name });
            initialAction.task = entity;
            return entity;
        }
    }
}